using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GuideCube
{
    public class GCSIdle : GCubeState
    {
        public GCSIdle(GCubeController controller) : base(controller)
        {
        }

        public override void Start()
        {
            if (Vector3.Distance(controller.Position, controller.NearestPointPosition) > 0.5f)
            {
                controller.SetState(new GCSGoTo(controller, controller.NearestPointPosition), this);
                return;
            }

            controller.SetVerticalOscylation(true);
            controller.SetRotating(true);
            controller.SetHightlithing(true);
        }

        public override void OnClicked()
        {
            controller.SetState(new GCSDialogue(controller, Dialogues.DialogueInkKeeper.CurrentText));
        }
    }
}