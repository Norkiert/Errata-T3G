using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Logic
{
    [System.Serializable]
    public class LogicBoolMath : MonoBehaviour, ILogicBoolOutput
    {
        public enum Operation { Multiplication, Sum }

        [SerializeField]
        private Operation operation = Operation.Sum;

        [SerializeField, ValidateInput(nameof(IsValid), "Object need inheritance by ILogicBoolOutput")]
        private List<MonoBehaviour> inputs = new List<MonoBehaviour>();

        [SerializeField, ReadOnly]
        private bool value;

        [Button]
        public void UpdateValue() => value = GetValue();

        private void Start() => InvokeRepeating(nameof(UpdateValue), 0.3f, 0.3f);

        public bool LogicValue => value;

        private bool GetValue()
        {
            switch (operation)
            {
                case Operation.Multiplication:
                    foreach (var input in inputs)
                        if (input != null && input.TryGetComponent(out ILogicBoolOutput logic) && logic.LogicValue == false)
                            return false;
                    return inputs.Count > 0;

                case Operation.Sum:
                    foreach (var input in inputs)
                        if (input != null && input.TryGetComponent(out ILogicBoolOutput logic) && logic.LogicValue == true)
                            return true;
                    return false;

                default:
                    return false;
            }
        }

        private bool IsValid()
        {
            foreach (var input in inputs)
                if (input == null || !input.TryGetComponent(out ILogicBoolOutput _))
                    return false;
            return true;
        }
    }
}
