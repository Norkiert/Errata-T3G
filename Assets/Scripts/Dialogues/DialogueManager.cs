using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Ink.Runtime;
using TMPro;
using DG.Tweening;
using Audio;

namespace Dialogues
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private GameObject dialoguePanel;
        [SerializeField, Min(0)] private float openTime = 0.2f;
        [SerializeField, Min(0)] private float closeTime = 0.2f;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private GameObject prevText;
        [SerializeField] private GameObject nextText;
        [SerializeField] private GameObject pressSpaceText;
        [SerializeField, Min(0)] private float textDisplayDelay = 0.04f;
        [SerializeField, Min(0)] private int maxNumberOfLetterInLine = 100;
        [SerializeField, Min(0)] private float lineHeight = 45f;
        [SerializeField, Min(0)] private float defalutPanelHeight = 155f;

        [SerializeField] GameObject __cube;

        [SerializeField] private GameObject[] choices;
        private TextMeshProUGUI[] choicesText;

        public TextAsset inkJSONTest;

        [SerializeField] List<AudioClipSO> talkingSounds = new List<AudioClipSO>();

        private Story currentStory;
        public bool IsDialoguePlaying { get; private set; }

        private bool isHandlingText = false;

        public static DialogueManager instance;

        private void Awake()
        {
            if (instance != null)
                Destroy(gameObject);

            instance = this;
        }

        private void Start()
        {
            ExitDialogueMode();

            choicesText = new TextMeshProUGUI[choices.Length];
            int index = 0;
            foreach (GameObject choice in choices)
            {
                choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
                index++;
            }

            EnterDialogueMode(inkJSONTest);
        }

        private void playTalkingSound() {
            int n = Random.Range(0, talkingSounds.Count - 1);
            AudioManager.PlaySFX(talkingSounds[n]);
        }

        private void Update()
        {
            if (!IsDialoguePlaying) return;

            if (Input.GetKeyDown(KeyCode.Alpha1) && currentStory.currentChoices.Count >= 1) MakeChoice(0);
            if (Input.GetKeyDown(KeyCode.Alpha2) && currentStory.currentChoices.Count >= 2) MakeChoice(1);
            if (Input.GetKeyDown(KeyCode.Alpha3) && currentStory.currentChoices.Count >= 3) MakeChoice(2);
            if (Input.GetKeyDown(KeyCode.Alpha4) && currentStory.currentChoices.Count >= 4) MakeChoice(3);
            if (Input.GetKeyDown(KeyCode.Alpha5) && currentStory.currentChoices.Count >= 5) MakeChoice(4);

            if (Input.GetKeyDown(KeyCode.Space))
                StartCoroutine(ContinueStory());

            Debug.Log(isHandlingText);
        }

        private void SetPanelHeight(string text) {
            float height = defalutPanelHeight + Mathf.CeilToInt(text.Length / (float)maxNumberOfLetterInLine) * lineHeight;
            RectTransform dialoguePanelRT = dialoguePanel.GetComponent<RectTransform>();
            dialoguePanelRT.sizeDelta = new Vector2(dialoguePanelRT.sizeDelta.x, height);
        }

        IEnumerator TextHandler;
        private IEnumerator HandleLongText(string s, float waitTime = 0) {
            isHandlingText = true;
            DisplayChoices();
            yield return new WaitForSeconds(waitTime);

            string[] texts = s.Split(new string[] {"<n>"}, System.StringSplitOptions.None);
            
            int current_text = 0;

            if (texts.Length > 1) nextText.SetActive(true);
            else {
                isHandlingText = false;
                nextText.SetActive(false);
            }

            prevText.SetActive(false);

            playTalkingSound();
            SetPanelHeight(texts[0]);

            while (current_text < texts.Length) {
                if (current_text == texts.Length - 1) DisplayChoices();

                if (texts[current_text] != dialogueText.text)
                {
                    dialogueText.text = "";
                    foreach (char c in texts[current_text])
                    {
                        dialogueText.text += c;
                        yield return new WaitForSeconds(textDisplayDelay);
                    }
                }
                

                if (Input.GetKeyDown(KeyCode.Mouse0)) {
                    if (current_text > 0) {
                        current_text--;
                        dialogueText.text = "";
                        playTalkingSound();

                        SetPanelHeight(texts[current_text]);
                        nextText.SetActive(true);
                    }

                    if (current_text == 0) prevText.SetActive(false);
                }

                if (Input.GetKeyDown(KeyCode.Mouse1)) {  
                    if (current_text < texts.Length - 1) {
                        SetPanelHeight(texts[current_text]);
                        dialogueText.text = "";
                        playTalkingSound();
                        prevText.SetActive(true);
                    }
                    current_text++;

                    if (current_text == texts.Length - 1) {
                        isHandlingText = false;
                        nextText.SetActive(false);
                    }
                }

                yield return null;
            }
        
            dialogueText.text = "";
            yield break;

        }

        private IEnumerator ContinueStory(float waitTime = 0)
        {
            if (currentStory.canContinue)
            {
                if (isHandlingText) yield break;
                string text = currentStory.Continue();
                Debug.Log("CanContinue text: " + text);

                
                if (TextHandler != null) StopCoroutine(TextHandler);
                dialogueText.text = "";
                TextHandler = HandleLongText(text, waitTime);
                StartCoroutine(TextHandler);
            }
            else
            {
                if (currentStory.currentChoices.Count > 0) yield break;
                if (isHandlingText) yield break;

                yield return new WaitForSeconds(0.2f);
                CloseDialoguePanel();
            }
        }

        public void EnterDialogueMode(TextAsset inkJSON)
        {
            if (IsDialoguePlaying)
            {
                Debug.LogError("Another dialogue is already playing");
                return;
            }

            currentStory = new Story(inkJSON.text);

            currentStory.BindExternalFunction("test", (string color) => {
                SetCubeColor(color);
            });

            IsDialoguePlaying = true;
            dialogueText.text = "";
            dialoguePanel.SetActive(true);
            for (int index = 0; index < choices.Length; index++)
                choices[index].gameObject.SetActive(false);

            // anim open
            CanvasGroup cg = dialoguePanel.GetComponent<CanvasGroup>();
            dialoguePanel.transform.localScale = Vector3.zero;
            DOTween.Sequence()
                .Append(DOVirtual.Float(0, 1, openTime, (v) => cg.alpha = v).SetEase(Ease.InQuad))
                .Join(dialoguePanel.transform.DOScale(Vector3.one, openTime).SetEase(Ease.InQuad))
                ;

            StartCoroutine(ContinueStory(openTime));
        }

        public void CloseDialoguePanel(bool anim = true)
        {
            if (anim)
            {
                CanvasGroup cg = dialoguePanel.GetComponent<CanvasGroup>();
                DOTween.Sequence()
                    .Append(DOVirtual.Float(1, 0, closeTime, (v) => cg.alpha = v).SetEase(Ease.InQuad))
                    .Join(dialoguePanel.transform.DOScale(Vector3.zero, closeTime).SetEase(Ease.InQuad))
                    .OnComplete(ExitDialogueMode)
                    ;
            }
            else
            {
                ExitDialogueMode();
            }
        }
        private void ExitDialogueMode()
        {
            IsDialoguePlaying = false;
            dialoguePanel.SetActive(false);
            dialogueText.text = "";
        }

        private void DisplayChoices()
        {
            List<Choice> currentChoices = currentStory.currentChoices;

            pressSpaceText.SetActive(currentChoices.Count == 0);

            if (currentChoices.Count > choices.Length)
            {
                Debug.LogWarning("More choices were given than the UI can support. Number of choices given: "
                    + currentChoices.Count);
            }

            int index = 0;
            for (; index < Mathf.Min(currentChoices.Count, choices.Length); index++)
            {
                choices[index].gameObject.SetActive(true);
                choicesText[index].text = (index + 1).ToString() + ". " + currentChoices[index].text;
            }
            for (; index < choices.Length; index++)
                choices[index].gameObject.SetActive(false);
        }

        public void MakeChoice(int choiceIndex)
        {
            currentStory.ChooseChoiceIndex(choiceIndex);
            StartCoroutine(ContinueStory());
            dialogueText.text = "";
        }

        private void SetCubeColor(string color)
        {
            Color c = new Color(255, 255, 255);
            switch (color)
            {
                case "yellow":
                    c = new Color(255, 255, 0);
                    break;
                case "red":
                    c = new Color(255, 0, 0);
                    break;
            }
            __cube.GetComponent<MeshRenderer>().material.color = c;
        }
    }

}