using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogues
{
    public class DialogueZone : MonoBehaviour
    {
        [SerializeField, Required] private TextAsset dialogueText;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<PlayerController>())
            {
                DialogueInkKeeper.AddNewText(dialogueText);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.GetComponent<PlayerController>())
            {
                DialogueInkKeeper.RemoveText(dialogueText);
            }
        }
    }
}