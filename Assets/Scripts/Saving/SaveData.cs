using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveData
{
    public List<SVector3> mirrorsRotations = new List<SVector3>();

    public List<string> cableStartTarget = new List<string>();
    public List<string> cableEndTarget = new List<string>();
    public List<ListWrapper> cablePartsLocation = new List<ListWrapper>();
}
