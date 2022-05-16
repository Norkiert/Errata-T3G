using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System.IO;

public class HroberPrefsReset : MonoBehaviour
{
    [Button]
    public static void ResetHroberPrefs()
    {
        string path = Application.persistentDataPath + "/hrober.json";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    [Button]
    public static void ResetSaveGame()
    {
        string path = Application.persistentDataPath + "/errata.json";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    [Button]
    public static void ResetAll()
    {
        ResetSaveGame();
        ResetHroberPrefs();
    }

    public static void ResetSave()
    {
        string path = Application.persistentDataPath + "/errata.json";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
