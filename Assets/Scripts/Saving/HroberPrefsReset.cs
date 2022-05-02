using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System.IO;

public class HroberPrefsReset : MonoBehaviour
{
    [Button]
    public void ResetHroberPrefs()
    {
        HroberPrefs.Load();
        HroberPrefs.data.bools.Clear();
        HroberPrefs.data.ints.Clear();
        HroberPrefs.data.floats.Clear();
        HroberPrefs.Save();
    }

    [Button]
    public void ResetSaveGame()
    {
        string path = Application.persistentDataPath + "/errata.json";
        if (File.Exists(path))
        {
            SaveData save = new SaveData();
            string json = JsonUtility.ToJson(save);
            File.WriteAllText(path, json);
        }
    }

    [Button]
    public void ResetAll()
    {
        ResetSaveGame();
        ResetHroberPrefs();
    }

    public static void ResetSave()
    {
        string path = Application.persistentDataPath + "/errata.json";
        if (File.Exists(path))
        {
            SaveData save = new SaveData();
            string json = JsonUtility.ToJson(save);
            File.WriteAllText(path, json);
        }
    }
}
