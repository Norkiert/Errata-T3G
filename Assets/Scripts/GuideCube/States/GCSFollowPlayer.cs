using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GuideCube
{
    public class GCSFollowPlayer : GCubeState
    {
        private readonly PlayerController player;
        private readonly float maxDistanceFromPlayer;

        private Vector3 target;
        private const float updateSafeDistance = 2f;
        private const float targetUpdateDelay = 0.5f;
        private float timeToUpdateTarget = 0;

        public GCSFollowPlayer(GCubeController controller, PlayerController player, float maxDistanceFromPlayer) : base(controller)
        {
            this.player = player;
            this.maxDistanceFromPlayer = maxDistanceFromPlayer;
        }

        public override void Start()
        {
            controller.SetRotating(true);
        }

        public override void Update()
        {
            timeToUpdateTarget -= Time.deltaTime;
            if (timeToUpdateTarget < 0)
            {
                timeToUpdateTarget = targetUpdateDelay;

                if (Vector3.Distance(target, player.transform.position) > maxDistanceFromPlayer + updateSafeDistance)
                {
                    target = player.transform.position;
                    controller.GoToTarget(target, maxDistanceFromPlayer);
                }
            }

            controller.SetVerticalOscylation(!controller.IsOnTheWay);
        }
    }
}