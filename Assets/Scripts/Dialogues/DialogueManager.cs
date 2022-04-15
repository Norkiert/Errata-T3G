using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Ink.Runtime;
using TMPro;
using DG.Tweening;

namespace Dialogues
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private GameObject dialoguePanel;
        [SerializeField, Min(0)] private float openTime = 0.2f;
        [SerializeField, Min(0)] private float closeTime = 0.2f;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private GameObject pressSpaceText;
        [SerializeField, Min(0)] private float textDisplayDelay = 0.04f;
        [SerializeField, Min(0)] private int maxNumberOfLetterInLine = 100;
        [SerializeField, Min(0)] private float lineHeight = 45f;
        [SerializeField, Min(0)] private float defalutPanelHeight = 155f;

        [SerializeField] GameObject __cube;

        [SerializeField] private GameObject[] choices;
        private TextMeshProUGUI[] choicesText;

        public TextAsset inkJSONTest;

        private Story currentStory;
        public bool IsDialoguePlaying { get; private set; }

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
        }

        private void Update()
        {
            if (!IsDialoguePlaying) return;

            if (Input.GetKeyDown(KeyCode.Alpha1) && currentStory.currentChoices.Count >= 1) MakeChoice(0);
            if (Input.GetKeyDown(KeyCode.Alpha2) && currentStory.currentChoices.Count >= 2) MakeChoice(1);
            if (Input.GetKeyDown(KeyCode.Alpha3) && currentStory.currentChoices.Count >= 3) MakeChoice(2);

            if (Input.GetKeyDown(KeyCode.Space))
                StartCoroutine(ContinueStory());
        }

        private IEnumerator ContinueStory(float waitTime = 0)
        {
            if (currentStory.canContinue)
            {
                string text = currentStory.Continue();

                DisplayChoices();

                int index = 0;
                dialogueText.text = "";

                float height = defalutPanelHeight + Mathf.CeilToInt(text.Length / (float)maxNumberOfLetterInLine) * lineHeight;
                RectTransform dialoguePanelRT = dialoguePanel.GetComponent<RectTransform>();
                dialoguePanelRT.sizeDelta = new Vector2(dialoguePanelRT.sizeDelta.x, height);

                yield return new WaitForSeconds(waitTime);

                while (dialogueText.text != text && index < text.Length)
                {
                    dialogueText.text += text[index];
                    index++;
                    yield return new WaitForSeconds(textDisplayDelay);
                }
            }
            else
            {
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