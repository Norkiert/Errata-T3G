using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Logic
{
    public class LogicBoolOutputValue : LogicBoolOutput
    {
        public override bool LogicValue => value;

        [SerializeField] private bool value = false;

        public void SetValue(bool value) => this.value = value;
    }
}
