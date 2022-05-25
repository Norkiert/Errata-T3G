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
    public List<SKeyValue<ListWrapper>> cablePartsLocation = new List<SKeyValue<ListWrapper>>();
    public List<SVector3> cableStartPosition = new List<SVector3>();
    public List<SVector3> cableEndPosition = new List<SVector3>();
    public List<string> cableNames = new List<string>();

    public List<SVector3> rotatableRotations = new List<SVector3>();
    public List<SVector3> boxPositions = new List<SVector3>();
    public List<bool> questsState = new List<bool>();

    public bool isLaserFinished = false;
    public bool isElectricalFinished = false;
    public bool isSteampunkFinished = false;
}
