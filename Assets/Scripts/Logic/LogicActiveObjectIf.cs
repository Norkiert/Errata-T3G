using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Logic
{
    public class LogicActiveObjectIf : LogicBoolInput
    {
        [SerializeField, Required]
        private GameObject objectToSet;

        protected override void ValueChaged(bool value)
        {
            if (objectToSet)
                objectToSet.SetActive(value);
        }
    }
}
