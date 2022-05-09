using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Logic
{
    [System.Serializable]
    public class LogicBoolMath : LogicBoolOutput
    {
        public enum Operation { Multiplication, Sum, Not }

        [SerializeField, ValidateInput(nameof(UseOnlyFirstInput), "This operation using only first input")]
        private Operation operation = Operation.Sum;

        [SerializeField]
        private List<LogicBoolOutput> inputs = new List<LogicBoolOutput>();

        [SerializeField, ReadOnly]
        private bool value = false;

        [Button("Update value")]
        public void UpdateValue() => value = GetValue();

        private void Start() => InvokeRepeating(nameof(UpdateValue), 0.3f, 0.3f);

        public override bool LogicValue => value;

        private bool GetValue()
        {
            switch (operation)
            {
                case Operation.Multiplication:
                    {
                        foreach (var input in inputs)
                            if (input != null && input.LogicValue == false)
                                return false;
                        return inputs.Count > 0;
                    }
                case Operation.Sum:
                    {
                        foreach (var input in inputs)
                            if (input != null && input.LogicValue == true)
                                return true;
                        return false;
                    }
                case Operation.Not:
                    {
                        if (inputs.Count > 0)
                            return !inputs[0].LogicValue;
                        return false;
                    }

                default:
                    return false;
            }
        }

        private bool UseOnlyFirstInput() => !(inputs.Count > 1 && operation == Operation.Not);
    }
}
