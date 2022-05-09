using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Logic
{
    public class LogicActiveObjectIf : MonoBehaviour
    {
        [SerializeField, Required]
        private LogicBoolOutput input;

        [SerializeField, Required]
        private GameObject objectToSet;

        private bool lastValue = false;

        private void Start()
        {
            InvokeRepeating(nameof(CheckValue), 0.3f, 0.3f);
            UpdateActive();
        }

        public void CheckValue()
        {
            if (input.LogicValue == lastValue)
                return;

            lastValue = input.LogicValue;

            UpdateActive();
        }
        private void UpdateActive()
        {
            if (objectToSet)
                objectToSet.SetActive(input.LogicValue);
        }
    }
}
