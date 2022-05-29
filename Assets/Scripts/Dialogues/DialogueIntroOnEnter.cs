using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogues;

public class DialogueIntroOnEnter : MonoBehaviour
{
    [SerializeField] Dimension bugsFromDimension;
    [SerializeField] private TextAsset dialogueText;
    [SerializeField] private TextAsset worldBugsDialogueText;
    [SerializeField] private string uniqueSaveKeyOnEnter = "undefined";

    private void Start()
    {
        if (HroberPrefs.ReadBool(uniqueSaveKeyOnEnter, false))
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerInteractions>() != null)
            PlayDialogue();
    }

    private void PlayDialogue()
    {
        if (HroberPrefs.ReadBool(uniqueSaveKeyOnEnter, false))
            return;

        DialogueManager.instance.EnterDialogueMode(dialogueText);

        HroberPrefs.SaveBool(uniqueSaveKeyOnEnter, true);
        
        if (SaveManager.IsLevelFinished(bugsFromDimension))
            gameObject.SetActive(false);
        else
            StartCoroutine(PlayBugsDialogueAfterCurrentDialogueEnd());
    }

    private IEnumerator PlayBugsDialogueAfterCurrentDialogueEnd()
    {
        yield return null;

        while (DialogueManager.instance.IsDialoguePlaying)
            yield return null;

        yield return new WaitForSeconds(0.5f);

        DialogueManager.instance.EnterDialogueMode(worldBugsDialogueText);
        gameObject.SetActive(false);
    }
}
