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
        save = SaveSteampunk(save);

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
            LoadSteampunk(save);
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

        GameObject sun = GameObject.Find("SunModelFinal");
        save.isLaserFinished = sun != null;

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
        save.cableStartPosition.Clear();
        save.cableEndPosition.Clear();
        save.cableNames.Clear();

        for (int i = 0; i < cables.Length; i++)
        {
            PhysicCable cable = cables[i];

            save.cableNames.Add(cable.gameObject.name);
            SpringJoint[] points = cable.gameObject.GetComponentsInChildren<SpringJoint>();

            Connector start = cable.StartConnector;
            Connector end = cable.EndConnector;

            save.cableStartPosition.Add(start.gameObject.transform.position);
            save.cableEndPosition.Add(end.gameObject.transform.position);

            if (start.ConnectedTo != null && start.ConnectionType != Connector.ConType.Female) {
                Connector female = start.ConnectedTo;
                save.cableStartTarget.Add(female.gameObject.name);

            } else{
                save.cableStartTarget.Add("");
            }

            if (end.ConnectedTo != null && end.ConnectionType != Connector.ConType.Female) {
                Connector female = end.ConnectedTo;
                save.cableEndTarget.Add(female.gameObject.name);
            } else {
                save.cableEndTarget.Add("");
            }

            //Debug.Log(points.Length);
            save.cablePartsLocation.Add(new SKeyValue<ListWrapper>(cables[i].gameObject.name, new ListWrapper()));
            for (int j = 0; j < points.Length - 1; j++)
            {
                save.cablePartsLocation[i].value.list.Add(points[j].gameObject.transform.position);
            }
        }

        ElectricalQuestCompleted q = GameObject.FindObjectOfType<ElectricalQuestCompleted>();
        
        if (q.Q1State && q.Q2State && q.Q3State && q.Q4State) 
            save.isElectricalFinished = true;

        return save;
    }
    static void LoadElectrical(SaveData save)
    {
        if (!SceneManager.GetSceneByName("Electrical_Scene").isLoaded)
        {
            return;
        }
        
        if (save.cablePartsLocation.Count == 0) return;
        if (save.cableStartPosition.Count == 0) return;
        

        for (int i = 0; i < save.cableNames.Count; i++)
        {
            GameObject cGO = GameObject.Find(save.cableNames[i]);

            if (cGO == null)
            {
                Debug.LogError($"DontfoundCable {save.cableNames[i]}");
                continue;
            }

            PhysicCable cable = cGO.GetComponent<PhysicCable>();
            if (cable.allowSaved == false) continue;

            SpringJoint[] points = cable.gameObject.GetComponentsInChildren<SpringJoint>();
            
            Connector start = cable.StartConnector;
            Connector end = cable.EndConnector;

            if (start.IsConnected && start.ConnectionType != Connector.ConType.Female) start.Disconnect(false);
            if (end.IsConnected && start.ConnectionType != Connector.ConType.Female) end.Disconnect(false);

            start.gameObject.transform.position = save.cableStartPosition[i];
            end.gameObject.transform.position = save.cableEndPosition[i];

            //Debug.Log(points.Length + " p: " + save.cablePartsLocation[i].list.Count);
            for (int j = 0; j < save.cablePartsLocation.Count; j++)
            {
                var cablePortLocationS = save.cablePartsLocation[j];
                if (cablePortLocationS.key == save.cableNames[i])
                {
                    //Debug.Log(save.cablePartsLocation[j].key + " : " + save.cablePartsLocation[j].value.list.Count);
                    for (int k = 0; k < cablePortLocationS.value.list.Count; k++)
                    {
                        if (k >= points.Length)
                        {
                            Debug.LogError("Zjebany zapis kable jest");
                            continue;
                        }
                        points[k].transform.position = cablePortLocationS.value.list[k];
                    }
                }
            }

            if (save.cableStartTarget[i] != "")
            {
                Connector target = GameObject.Find(save.cableStartTarget[i]).GetComponent<Connector>();
                target.Connect(start, false);
            }

            if (save.cableEndTarget[i] != "")
            {
                Connector target = GameObject.Find(save.cableEndTarget[i]).GetComponent<Connector>();
                target.Connect(end, false);
            }
        }
    }

    static SaveData SaveSteampunk(SaveData save)
    {
        if (!SceneManager.GetSceneByName("Steampunk_Scene").isLoaded)
        {
            return save;
        }

        BasicTrack[] tracks = GameObject.FindObjectsOfType<BasicTrack>();
        List<BasicTrack> rotatable = new List<BasicTrack>();

        for (int i = 0; i < tracks.Length; i++)
            if (tracks[i].rotateable) rotatable.Add(tracks[i]);

        save.rotatableRotations.Clear();
        for (int i = 0; i < rotatable.Count; i++)
            save.rotatableRotations.Add(rotatable[i].transform.eulerAngles);

        save.boxPositions.Clear();

        UnderTrackBox[] boxes = GameObject.FindObjectsOfType<UnderTrackBox>();
        for (int i = 0; i < boxes.Length; i++)
        {
            save.boxPositions.Add(boxes[i].gameObject.transform.position);
        }

        bool q1 = GameObject.Find("Q1Complete").GetComponent<SteampunkQGeneral>().completed;
        bool q2 = GameObject.Find("Q2Complete").GetComponent<SteampunkQGeneral>().completed;
        bool q3 = GameObject.Find("Q3Complete").GetComponent<SteampunkQGeneral>().completed;
        bool q4 = GameObject.Find("Q4Complete").GetComponent<SteampunkQGeneral>().completed;

        save.questsState.Clear();

        save.questsState.Add(q1);
        save.questsState.Add(q2);
        save.questsState.Add(q3);
        save.questsState.Add(q4);

        if (!save.questsState.Contains(false))
            save.isSteampunkFinished = true;

        return save;
    }

    static void LoadSteampunk(SaveData save)
    {
        if (!SceneManager.GetSceneByName("Steampunk_Scene").isLoaded)
        {
            return;
        }

        BasicTrack[] tracks = GameObject.FindObjectsOfType<BasicTrack>();
        List<BasicTrack> rotatable = new List<BasicTrack>();

        for (int i = 0; i < tracks.Length; i++)
            if (tracks[i].rotateable) rotatable.Add(tracks[i]);

        if (rotatable.Count != save.rotatableRotations.Count) return;

        for (int i = 0; i < save.rotatableRotations.Count; i++)
            rotatable[i].gameObject.transform.eulerAngles = save.rotatableRotations[i];

        UnderTrackBox[] boxes = GameObject.FindObjectsOfType<UnderTrackBox>();
        
        for (int i = 0; i < save.boxPositions.Count; i++)
        {
            boxes[i].gameObject.transform.position = save.boxPositions[i];
            boxes[i].UpdateTrackPosition();
        }

        if (save.questsState.Count == 4) {
            if (save.questsState[0]) {
                GameObject.Find("Q1Complete").GetComponent<SteampunkQ1>().OnCompletion();
                GameObject.Find("Door").transform.position = GameObject.Find("max").transform.position;
                GameObject.Find("AutoSpawnerTrack21").GetComponent<AutoSpawnerTrack>().SpawnBall();
            }
            if (save.questsState[1]) GameObject.Find("Q2Complete").GetComponent<SteampunkQ2>().OnCompletion();
            if (save.questsState[2]) GameObject.Find("Q3Complete").GetComponent<SteampunkQ3>().OnCompletion();
            if (save.questsState[3]) GameObject.Find("Q4Complete").GetComponent<SteampunkQ4>().OnCompletion();
        } 
    }

    public static bool IsLevelFinished(Dimension dim) {
        string path = Application.persistentDataPath + "/errata.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData save = JsonUtility.FromJson<SaveData>(json);

            switch (dim) {
                case Dimension.Laser:
                    return save.isLaserFinished;
                case Dimension.Electrical:
                    return save.isElectricalFinished;
                case Dimension.Steampunk:
                    return save.isSteampunkFinished;
            }

            return false;
        } else {
            return false;
        }
    }

    public static bool AreAllLevelFinished() => IsLevelFinished(Dimension.Electrical) && IsLevelFinished(Dimension.Laser) && IsLevelFinished(Dimension.Steampunk);
}
