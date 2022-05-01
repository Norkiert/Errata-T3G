using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HroberPrefsData
{
    public List<SKeyValue<bool>> bools = new List<SKeyValue<bool>>();
    public List<SKeyValue<int>> ints = new List<SKeyValue<int>>();
    public List<SKeyValue<float>> floats = new List<SKeyValue<float>>();
}
