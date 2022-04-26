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
        [Header("Main text")]
        [SerializeField] private GameObject dialoguePanel;
        [SerializeField, Min(0)] private float openTime = 0.2f;
        [SerializeField, Min(0)] private float closeTime = 0.2f;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private TextMeshProUGUI choicesText;
        private List<string> textHistory = new List<string>();

        [Header("Tooltips")]
        [SerializeField] private GameObject toolTipNext;
        [SerializeField] private GameObject toolTipPrev;
        [SerializeField] private GameObject toolTipSelectOption;
        [SerializeField, Min(0)] private float textDisplayDelay = 0.04f;
        [SerializeField, Min(0)] private int maxNumberOfLetterInLine = 50;

        [Header("Ink")]
        [SerializeField] private TextAsset inkJSONTest;

        [Header("Sounds")]
        [SerializeField] private List<AudioClipSO> talkingSounds = new List<AudioClipSO>();

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

            EnterDialogueMode(inkJSONTest);
        }

        private void Update()
        {
            if (!IsDialoguePlaying || GameManagment.GameManager.IsGamePaused) return;

            if (Input.GetKeyDown(KeyCode.Alpha1) && currentStory.currentChoices.Count >= 1) MakeChoice(0);
            if (Input.GetKeyDown(KeyCode.Alpha2) && currentStory.currentChoices.Count >= 2) MakeChoice(1);
            if (Input.GetKeyDown(KeyCode.Alpha3) && currentStory.currentChoices.Count >= 3) MakeChoice(2);
            if (Input.GetKeyDown(KeyCode.Alpha4) && currentStory.currentChoices.Count >= 4) MakeChoice(3);
            if (Input.GetKeyDown(KeyCode.Alpha5) && currentStory.currentChoices.Count >= 5) MakeChoice(4);

            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1))
                StartCoroutine(ContinueStory());
        }
        private void PlayTalkingSound() {
            int n = Random.Range(0, talkingSounds.Count - 1);
            AudioManager.PlaySFX(talkingSounds[n]);
        }

        private IEnumerator textHandler;
        private IEnumerator DisplayText(string text)
        {
            dialogueText.text = "";
            foreach (char c in text)
            {
                dialogueText.text += c;
                yield return new WaitForSeconds(textDisplayDelay);
            }
        }

        private void CoroutineResetStart(string text)
        {
            if (textHandler != null) StopCoroutine(textHandler);
            textHandler = DisplayText(text);
            StartCoroutine(textHandler);
        }

        private int historyIndex = 0;
        private IEnumerator ContinueStory(float waitTime = 0, bool start = false)
        {
            if (start)
            {
                if (currentStory.canContinue)
                {
                    string text = currentStory.Continue();
                    textHistory.Add(text);

                    StartCoroutine(DisplayText(text));
                    DisplayChoices(MainTextLines(text));

                    if (currentStory.currentChoices.Count == 0) toolTipNext.SetActive(true);
                }
                yield break;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (historyIndex < textHistory.Count - 1) {
                    historyIndex++;

                    CoroutineResetStart(textHistory[historyIndex]);

                    if (currentStory.currentChoices.Count == 0) toolTipNext.SetActive(true);
                    else toolTipNext.SetActive(false);

                    if (historyIndex > 0) toolTipPrev.SetActive(true);
                }

                else if (currentStory.canContinue)
                {
                    string text = currentStory.Continue();
                    textHistory.Add(text);
                    historyIndex++;

                    CoroutineResetStart(text);
                    DisplayChoices(MainTextLines(text));

                    if (currentStory.currentChoices.Count == 0) toolTipNext.SetActive(true);
                    else toolTipNext.SetActive(false);

                    if (historyIndex > 0) toolTipPrev.SetActive(true);
                }
                else
                {
                    if (currentStory.currentChoices.Count > 0) yield break;

                    yield return new WaitForSeconds(0.2f);
                    CloseDialoguePanel();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                if (historyIndex > 0)
                {
                    historyIndex--;
                    Debug.Log(historyIndex);
                    CoroutineResetStart(textHistory[historyIndex]);
                
                    if (historyIndex == 0) toolTipPrev.SetActive(false);

                    if (historyIndex < textHistory.Count - 1) toolTipNext.SetActive(true);
                }
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
            choicesText.text = "";
            dialoguePanel.SetActive(true);


            // anim open
            CanvasGroup cg = dialoguePanel.GetComponent<CanvasGroup>();
            dialoguePanel.transform.localScale = Vector3.zero;
            DOTween.Sequence()
                .Append(DOVirtual.Float(0, 1, openTime, (v) => cg.alpha = v).SetEase(Ease.InQuad))
                .Join(dialoguePanel.transform.DOScale(Vector3.one, openTime).SetEase(Ease.InQuad))
                ;

            StartCoroutine(ContinueStory(openTime, true));
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
            if (textHandler != null)
                StopCoroutine(textHandler);

            IsDialoguePlaying = false;
            dialoguePanel.SetActive(false);
            dialogueText.text = "";
        }

        private void DisplayChoices(int mainTextLines)
        {
            List<Choice> currentChoices = currentStory.currentChoices;

            choicesText.text = "";
            for (int i = 0; i < mainTextLines; i++)
                choicesText.text += "\n";

            for (int i = 0; i < currentChoices.Count; i++)
                choicesText.text += $"\n{i + 1}. {currentChoices[i].text}";
        }
        private int MainTextLines(string text) => Mathf.FloorToInt(text.Length / (float)maxNumberOfLetterInLine);


        public void MakeChoice(int choiceIndex)
        {
            currentStory.ChooseChoiceIndex(choiceIndex);
            historyIndex = 0;
            textHistory.Clear();
            StartCoroutine(ContinueStory(0f, true));
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
            Debug.Log($"Selected color {c}");
        }
    }

}