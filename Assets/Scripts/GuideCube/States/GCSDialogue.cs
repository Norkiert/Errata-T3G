using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            controller.SetVerticalOscylation(true);
            controller.SetRotating(true);
            DialogueManager.instance.EnterDialogueMode(dialogueText);
        }

        public override void Update()
        {
            Debug.Log(Vector3.Distance(controller.Position, controller.Player.transform.position));
            if (Vector3.Distance(controller.Position, controller.Player.transform.position) > distanceToCloseDialogue)
            {
                DialogueManager.instance.ExitDialogueMode();
                controller.EndCurrentState();
            }
        }

        public override void End()
        {
            DialogueManager.instance.ExitDialogueMode();
        }
    }
}