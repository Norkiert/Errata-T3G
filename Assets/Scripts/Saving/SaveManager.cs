using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveManager
{
    public static void SaveGame()
    {
        string path = Application.persistentDataPath + "/errata.json";

        SaveData save;
        if (File.Exists(path)) {
            string json = File.ReadAllText(path);
            save = JsonUtility.FromJson<SaveData>(json);
        } else {
            save = new SaveData();
        }

        save = SaveLaser(save);

        string saveJson = JsonUtility.ToJson(save);

        File.WriteAllText(path, saveJson);

        Debug.Log("saved game");
    }

    public static void LoadGame()
    {
        string path = Application.persistentDataPath + "/errata.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData save = JsonUtility.FromJson<SaveData>(json);

            LoadLaser(save);
        }

        Debug.Log("loaded game");
    }

    static SaveData SaveLaser(SaveData save)
    {
        if (!SceneManager.GetSceneByName("SpaceLaser_Scene").isLoaded)
        {
            return save;
        }

        LaserMirror[] mirrors = GameObject.FindObjectsOfType<LaserMirror>();

        save.mirrorsRotations.Clear();
        for (int i = 0; i < mirrors.Length; i++)
        {
            save.mirrorsRotations.Add(mirrors[i].gameObject.transform.eulerAngles);
        }

        Debug.Log("saved laser");
        return save;
    }

    static void LoadLaser(SaveData save)
    {
        if (!SceneManager.GetSceneByName("SpaceLaser_Scene").isLoaded)
        {
            return;
        }

        LaserMirror[] mirrors = GameObject.FindObjectsOfType<LaserMirror>();

        if (mirrors.Length != save.mirrorsRotations.Count) return;

        for (int i = 0; i < save.mirrorsRotations.Count; i++)
        {
            mirrors[i].gameObject.transform.eulerAngles = save.mirrorsRotations[i];
        }
        Debug.Log("loaded laser");
    }

    
}
