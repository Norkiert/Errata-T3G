using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Logic
{
    public class LogicActiveObjectIf : MonoBehaviour
    {
        [SerializeField, ValidateInput(nameof(IsValid), "Object need inheritance by ILogicBoolOutput")]
        private MonoBehaviour input;

        [SerializeField, Required]
        private GameObject objectToSet;

        private void Start() => InvokeRepeating(nameof(Set), 0.3f, 0.3f);

        private void Set()
        {
            if (input.TryGetComponent(out ILogicBoolOutput logic))
                objectToSet?.SetActive(logic.LogicValue);
        }

        private bool IsValid()
        {
            return input != null && input.GetComponent<ILogicBoolOutput>() != null;
        }
    }
}
