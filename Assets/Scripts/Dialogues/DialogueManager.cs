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
        [SerializeField] private Color choicesColor;
        private readonly List<string> textHistory = new List<string>();

        [Header("Tooltips")]
        [SerializeField] private GameObject toolTipNext;
        [SerializeField] private GameObject toolTipPrev;
        [SerializeField] private GameObject toolTipSelectOption;
        [SerializeField, Min(0)] private float textDisplayDelay = 0.04f;

        [Header("Sounds")]
        [SerializeField] private List<AudioClipSO> talkingSounds = new List<AudioClipSO>();
        [SerializeField, Min(1)] private int letterPerSound = 2;

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
            {
                if (continueCoroutine == null)
                {
                    continueCoroutine = ContinueStory();
                    StartCoroutine(continueCoroutine);
                }
            }
        }


        private void PlayTalkingSound()
        {
            int n = Random.Range(0, talkingSounds.Count);
            AudioManager.PlaySFX(talkingSounds[n]);
        }

        private IEnumerator textHandler;
        private IEnumerator DisplayText(string text)
        {
            continueCoroutine = null;
            Debug.Log("Index: " + historyIndex);
            text = text.Replace("<n>", "\n");
            dialogueText.text = text;
            dialogueText.maxVisibleCharacters = 0;

            for (int i = 0; i < text.Length; i++)
            {
                dialogueText.maxVisibleCharacters++;
                if (i % letterPerSound == 0)
                    PlayTalkingSound();
                yield return new WaitForSeconds(textDisplayDelay);
            }

            
            
            string choices = ChoicesString();
            dialogueText.maxVisibleCharacters += choices.Length;
            dialogueText.text += choices;

            textHandler = null;
        }

        private void CoroutineResetStart(string text)
        {
            if (textHandler != null)
                StopCoroutine(textHandler);

            textHandler = DisplayText(text);
            StartCoroutine(textHandler);

            UpdateToolTips();
        }

        private int historyIndex = 0;
        private IEnumerator continueCoroutine = null;
        private IEnumerator ContinueStory(float waitTime = 0, bool start = false)
        {
            Debug.Log("start: " + start);
            if (waitTime > 0)
                yield return new WaitForSeconds(waitTime);

            if (start)
            {
                Debug.Log("start");
                if (currentStory.canContinue)
                {
                    string text = currentStory.Continue();
                    textHistory.Add(text);

                    if (text.Length < 3)
                        text = "";

                    if (text == "")
                    {
                        ExitDialogueMode();
                        yield break;
                    }

                    CoroutineResetStart(text);

                    UpdateToolTips();
                }
                yield break;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Debug.Log("dupa");

                if (textHandler != null)
                {
                    StopCoroutine(textHandler);
                    textHandler = null;
                    continueCoroutine = null;
                    dialogueText.maxVisibleCharacters = dialogueText.text.Length;
                }
                else if (historyIndex < textHistory.Count - 1)
                {
                    historyIndex++;

                    string text = textHistory[historyIndex];
                    if (text.Length < 3)
                        text = "";
                    CoroutineResetStart(text);
                }
                else if (currentStory.canContinue)
                {
                    string text = currentStory.Continue();
                    textHistory.Add(text);
                    historyIndex++;

                    if (text.Length < 3)
                        text = "";
                    CoroutineResetStart(text);
                }
                else
                {
                    if (currentStory.currentChoices.Count > 0)
                        yield break;

                    yield return new WaitForSeconds(0.2f);
                    CloseDialoguePanel();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                if (historyIndex > 0)
                {
                    historyIndex--;
                    CoroutineResetStart(textHistory[historyIndex]);
                }
            }
        }

        private void UpdateToolTips()
        {
            bool chcivedChoices = currentStory.currentChoices.Count > 0;
            toolTipNext.SetActive(historyIndex < textHistory.Count - 1 || chcivedChoices == false);
            toolTipPrev.SetActive(historyIndex > 0);

            bool showChoices = chcivedChoices && historyIndex == textHistory.Count - 1;
            toolTipSelectOption.SetActive(showChoices);
        }

        public void EnterDialogueMode(TextAsset inkJSON)
        {
            if (IsDialoguePlaying)
            {
                Debug.LogError("Another dialogue is already playing");
                return;
            }

            Debug.Log($"Play story: {inkJSON.name}");
            currentStory = new Story(inkJSON.text);

            currentStory.BindExternalFunction("test", (string color) => {
                SetCubeColor(color);
            });

            IsDialoguePlaying = true;
            dialogueText.text = "";
            textHistory.Clear();
            historyIndex = 0;
            dialoguePanel.SetActive(true);


            // anim open
            CanvasGroup cg = dialoguePanel.GetComponent<CanvasGroup>();
            dialoguePanel.transform.localScale = Vector3.zero;
            DOTween.Sequence()
                .Append(DOVirtual.Float(0, 1, openTime, (v) => cg.alpha = v).SetEase(Ease.InQuad))
                .Join(dialoguePanel.transform.DOScale(Vector3.one, openTime).SetEase(Ease.InQuad))
                ;

            continueCoroutine = ContinueStory(openTime, true);
            StartCoroutine(continueCoroutine);
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
            {
                StopCoroutine(textHandler);
                textHandler = null;
            }

            IsDialoguePlaying = false;
            dialoguePanel.SetActive(false);
            dialogueText.text = "";
        }

        private string ChoicesString()
        {
            List<Choice> currentChoices = currentStory.currentChoices;

            if (currentChoices.Count == 0)
                return "";

            string s = $"<color=#{ColorToHex(choicesColor)}>";
            for (int i = 0; i < currentChoices.Count; i++)
                s += $"{i + 1}. {currentChoices[i].text}\n";

            return s;
        }

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

        private string ColorToHex(Color color) => Mathf.RoundToInt(color.r * 255).ToString("X2") + Mathf.RoundToInt(color.g * 255).ToString("X2") + Mathf.RoundToInt(color.b * 255).ToString("X2");
    }
}