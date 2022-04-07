using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GuideCube.States
{
    public class GCSGoTo : GCubeState
    {
        private Vector3 target;

        public GCSGoTo(GCubeController controller, Vector3 target) : base(controller)
        {
            this.target = target;
        }

        public override void Start()
        {
            controller.GoToTarget(target, 0);
        }

        public override void Update()
        {
            if (controller.IsOnTheWay)
                return;

            controller.EndCurrentState();
        }
    }
}