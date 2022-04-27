using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogues;

namespace GuideCube
{
    public class GCSDialogue : GCubeState
    {
        private readonly TextAsset dialogueText;
        private readonly float distanceToCloseDialogue;

        public GCSDialogue(GCubeController controller, TextAsset dialogueText) : base(controller)
        {
            this.dialogueText = dialogueText;
            this.distanceToCloseDialogue = controller.MaxDistDialogue;
        }
        public GCSDialogue(GCubeController controller, TextAsset dialogueText, float distanceToCloseDialogue) : base(controller)
        {
            this.dialogueText = dialogueText;
            this.distanceToCloseDialogue = distanceToCloseDialogue;
        }

        public override void Start()
        {
            if (dialogueText == null)
            {
                Debug.LogWarning("GuideCube: Can't enter in dialogue state! text is null!");
                controller.EndCurrentState();
                return;
            }

            controller.SetVerticalOscylation(true);
            controller.SetRotating(true);
            DialogueManager.instance.EnterDialogueMode(dialogueText);
        }

        public override void Update()
        {
            if (
                !DialogueManager.instance.IsDialoguePlaying
                || Vector3.Distance(controller.Position, controller.Player.transform.position) > distanceToCloseDialogue
                )
            {
                controller.EndCurrentState();
            }    
        }

        public override void End()
        {
            if (DialogueManager.instance.IsDialoguePlaying)
                DialogueManager.instance.CloseDialoguePanel();
        }
    }
}