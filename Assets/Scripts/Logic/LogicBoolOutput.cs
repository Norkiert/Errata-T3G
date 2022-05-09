using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Logic
{
    public abstract class LogicBoolOutput : MonoBehaviour
    {
        public abstract bool LogicValue { get; }
    }
}