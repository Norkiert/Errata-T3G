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
        save = SaveElectrical(save);

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
            LoadElectrical(save);
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

    static SaveData SaveElectrical(SaveData save)
    {
        if (!SceneManager.GetSceneByName("Electrical_Scene").isLoaded)
        {
            return save;
        }

        PhysicCable[] cables = GameObject.FindObjectsOfType<PhysicCable>();

        save.cableEndTarget.Clear();
        save.cableStartTarget.Clear();
        save.cablePartsLocation.Clear();

        for (int i = 1; i < cables.Length; i++)
        {
            Connector[] startEnd = cables[i].gameObject.GetComponentsInChildren<Connector>();
            SpringJoint[] points = cables[i].gameObject.GetComponentsInChildren<SpringJoint>();

            Connector start = startEnd[0];
            Connector end = startEnd[1];

            if (start.ConnectedTo != null) {
                Connector female = start.ConnectedTo;
                save.cableStartTarget.Add(female.gameObject.name);

            } else {
                save.cableStartTarget.Add("");
            }

            if (end.ConnectedTo != null) {
                Connector female = end.ConnectedTo;
                save.cableEndTarget.Add(female.gameObject.name);
            } else {
                save.cableEndTarget.Add("");
            }

            Debug.Log(points.Length);
            save.cablePartsLocation.Add(new ListWrapper());
            for (int j = 0; j < points.Length; j++)
            {
                save.cablePartsLocation[i-1].list.Add(points[j].gameObject.transform.position);
            }
        }

        return save;
    }

    static void LoadElectrical(SaveData save)
    {
        if (!SceneManager.GetSceneByName("Electrical_Scene").isLoaded)
        {
            return;
        }
        
        if (save.cablePartsLocation.Count == 0) return;
        

        PhysicCable[] cables = GameObject.FindObjectsOfType<PhysicCable>();
        for (int i = 1; i < cables.Length; i++)
        {
            if (i == 1) continue;

            Connector[] startEnd = cables[i].gameObject.GetComponentsInChildren<Connector>();
            SpringJoint[] points = cables[i].gameObject.GetComponentsInChildren<SpringJoint>();
            
            Connector start = startEnd[0];
            Connector end = startEnd[1];

            Debug.Log(points.Length + " p: " + save.cablePartsLocation[i-1].list.Count);
            for (int j = 0; j < save.cablePartsLocation[i-1].list.Count; j++)
            {
                points[j].gameObject.transform.position = save.cablePartsLocation[i-1].list[j];
            }

            Debug.Break();

            if (save.cableStartTarget[i-1] != "") {
                Connector target = GameObject.Find(save.cableStartTarget[i-1]).GetComponent<Connector>();
                target.Connect(start, false);
            }

            if (save.cableEndTarget[i-1] != "") {
                Connector target = GameObject.Find(save.cableEndTarget[i-1]).GetComponent<Connector>();
                target.Connect(end, false);
            }
        }
    }
}
