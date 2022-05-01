using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public static class HroberPrefs
{
    public static HroberPrefsData data;
    public static bool ReadBool(string key, bool def = false)
    {
        Load();
        foreach (var kv in data.bools) {
            if (kv.key == key) return kv.value;
        }
        return def;
    }

    public static int ReadInt(string key, int def = 0)
    {
        Load();
        foreach (var kv in data.ints) {
            if (kv.key == key) return kv.value;
        }
        return def;
    }

    public static float ReadFloat(string key, float def = 0)
    {
        Load();
        foreach (var kv in data.floats) {
            if (kv.key == key) return kv.value;
        }
        return def;
    }

    public static void SaveBool(string key, bool value)
    {
        Load();
        data.bools.Add(new SKeyValue<bool>(key, value));
        Save();
    }

    public static void SaveInt(string key, int value)
    {
        Load();
        data.ints.Add(new SKeyValue<int>(key, value));
        Save();
    }

    public static void SaveFloat(string key, float value)
    {
        Load();
        data.floats.Add(new SKeyValue<float>(key, value));
        Save();
    }

    public static void Load()
    {
        string path = Application.persistentDataPath + "/hrober.json";

        if (File.Exists(path)) {
            string json = File.ReadAllText(path);
            data = JsonUtility.FromJson<HroberPrefsData>(json);
        } else {
            data = new HroberPrefsData();
        }
    }

    public static void Save()
    {
        string path = Application.persistentDataPath + "/hrober.json";

        string saveJson = JsonUtility.ToJson(data);
        File.WriteAllText(path, saveJson);
    }
}
