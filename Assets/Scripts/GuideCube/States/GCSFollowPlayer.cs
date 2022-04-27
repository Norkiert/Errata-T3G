using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GuideCube
{
    public class GCSFollowPlayer : GCubeState
    {
        private readonly float maxDistanceFromPlayer;

        private Vector3 target;
        private const float updateSafeDistance = 2f;
        private const float targetUpdateDelay = 0.5f;
        private float timeToUpdateTarget = 0;

        public GCSFollowPlayer(GCubeController controller, float maxDistanceFromPlayer) : base(controller)
        {
            this.maxDistanceFromPlayer = maxDistanceFromPlayer;
        }
        public GCSFollowPlayer(GCubeController controller) : base(controller)
        {
            this.maxDistanceFromPlayer = controller.MaxDistFollowPlayer;
        }

        public override void Start()
        {
            controller.SetRotating(true);
            controller.SetHightlithing(true);
        }

        public override void Update()
        {
            timeToUpdateTarget -= Time.deltaTime;
            if (timeToUpdateTarget < 0)
            {
                timeToUpdateTarget = targetUpdateDelay;

                if (Vector3.Distance(target, controller.Player.transform.position) > maxDistanceFromPlayer + updateSafeDistance)
                {
                    target = controller.Player.transform.position;
                    controller.GoToTarget(target, maxDistanceFromPlayer);
                }
            }

            controller.SetVerticalOscylation(!controller.IsOnTheWay);
        }


        public override void OnClicked()
        {
            controller.SetState(new GCSDialogue(controller, Dialogues.DialogueInkKeeper.CurrentText), this);
        }
    }
}