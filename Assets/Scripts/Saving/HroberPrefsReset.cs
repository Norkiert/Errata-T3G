using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class HroberPrefsReset : MonoBehaviour
{
    [Button]
    public void Reset()
    {
        HroberPrefs.Load();
        HroberPrefs.data.bools.Clear();
        HroberPrefs.data.ints.Clear();
        HroberPrefs.data.floats.Clear();
        HroberPrefs.Save();
    }
}
