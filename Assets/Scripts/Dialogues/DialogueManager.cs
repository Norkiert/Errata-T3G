using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Ink.Runtime;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject pressSpaceText;
    [SerializeField] private float textDisplayDelay = 0.04f;

    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    public TextAsset inkJSONTest;

    private Story currentStory;
    public bool isDialoguePlaying { get; private set; }

    public static DialogueManager instance;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);

        instance = this;
    }

    private void Start()
    {
        isDialoguePlaying = false;
        dialoguePanel.SetActive(false);

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
        if (!isDialoguePlaying) return;

        if (Input.GetKeyDown(KeyCode.Alpha1)) MakeChoice(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) MakeChoice(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) MakeChoice(2);

        if (currentStory.currentChoices.Count == 0 && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(ContinueStory());
        }
    }

    private IEnumerator ContinueStory()
    {
        if (currentStory.canContinue)
        {
            dialogueText.text = "";
            string text = currentStory.Continue();

            int index = 0;
            while (dialogueText.text != text && index < text.Length)
            {
                dialogueText.text += text[index];
                index++;
                yield return new WaitForSeconds(textDisplayDelay);
            }

            DisplayChoices();
        }
        else
        {
            yield return new WaitForSeconds(0.2f);
            ExitDialogueMode();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        if (isDialoguePlaying)
        {
            Debug.LogError("Another dialogue is already playing");
            return;
        }

        currentStory = new Story(inkJSON.text);
        isDialoguePlaying = true;
        dialoguePanel.SetActive(true);

        StartCoroutine(ContinueStory());
    }

    public void ExitDialogueMode()
    {
        isDialoguePlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        if (currentChoices.Count == 0) pressSpaceText.SetActive(true);
        else pressSpaceText.SetActive(false);

        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can support. Number of choices given: "
                + currentChoices.Count);
        }

        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = (index+1).ToString() + ". " + choice.text;
            index++;
        }
        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

    }

    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
        StartCoroutine(ContinueStory());
    }
}
