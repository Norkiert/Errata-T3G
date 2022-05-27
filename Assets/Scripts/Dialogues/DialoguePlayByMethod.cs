using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Dialogues
{
    public class DialoguePlayByMethod : MonoBehaviour
    {
        [Header("Dialogue")]
        [SerializeField] private TextAsset dialogueToPlay;

        [Header("Button relase")]
        [SerializeField] private bool waitForIteractButtonRelase = false;
        [SerializeField, ShowIf(nameof(waitForIteractButtonRelase))] private float waitForIteractButtonRelaseTime = 0.5f;

        [Header("Play only one time")]
        [SerializeField] private bool playOnlyOneTime = false;
        [SerializeField, ShowIf(nameof(playOnlyOneTime))] private string uniqueSaveKey = "undefined";

        public void PlayDialoge()
        {
            if (waitForIteractButtonRelase && Input.GetMouseButton(0))
                StartCoroutine(WaitForRealase());
            else
                TryPlayDialogue();
        }

        private IEnumerator WaitForRealase()
        {
            waitForIteractButtonRelase = false;

            float timeWithoutPress = 0;

            while (timeWithoutPress < waitForIteractButtonRelaseTime)
            {
                if (Input.GetMouseButton(0))
                    timeWithoutPress = 0;
                else
                    timeWithoutPress += Time.deltaTime;

                yield return null;
            }

            TryPlayDialogue();
            waitForIteractButtonRelase = true;
        }

        private void TryPlayDialogue()
        {
            if (playOnlyOneTime)
            {
                if (HroberPrefs.ReadBool(uniqueSaveKey, false))
                    return;

                HroberPrefs.SaveBool(uniqueSaveKey, true);
            }

            DialogueManager.instance.EnterDialogueMode(dialogueToPlay);
        }
    }
}
