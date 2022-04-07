using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GuideCube.States
{
    public class GCSIdle : GCubeState
    {
        public GCSIdle(GCubeController controller) : base(controller)
        {
        }

        public override void Start()
        {
            controller.SetVerticalOscylation(true);
            controller.SetRotating(true);
        }
    }
}