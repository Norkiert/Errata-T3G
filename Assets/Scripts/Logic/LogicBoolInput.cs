using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Logic
{
    public class LogicBoolInput : MonoBehaviour
    {
        [SerializeField, ValidateInput(nameof(IsValid), "Object need inheritance by ILogicBoolOutput")]
        private MonoBehaviour input;
        private ILogicBoolOutput inputI;
        private bool lastValue = false;

        public bool Value => inputI != null ? inputI.LogicValue : false;

        private void OnEnable()
        {
            inputI = input.GetComponent<ILogicBoolOutput>();
        }

        private void Start()
        {
            if (inputI != null)
                ValueChaged(inputI.LogicValue);

            InvokeRepeating(nameof(Check), 0.3f, 0.3f);
        }

        private void Check()
        {
            if (inputI == null)
                return;

            if (lastValue == inputI.LogicValue)
                return;

            lastValue = inputI.LogicValue;
            ValueChaged(inputI.LogicValue);
        }

        protected virtual void ValueChaged(bool value) { }

        private bool IsValid()
        {
            return input != null && input.GetComponent<ILogicBoolOutput>() != null;
        }
    }
}
