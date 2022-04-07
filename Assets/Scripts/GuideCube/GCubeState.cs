using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GuideCube.States
{
    public abstract class GCubeState
    {
        protected readonly GCubeController controller;

        public GCubeState(GCubeController controller)
        {
            this.controller = controller;
        }

        public virtual void Start() { }

        public virtual void Update() { }
    }
}