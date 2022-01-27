#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TracksEditor : EditorWindow
{
    public enum Mode
    {
        trackSelection,
        trackAddition
    }
    public static TracksEditor tracksEditor;
    public static GameObject defaultAsset;

    protected TrackBehavior[] selectedTracks;
    protected TrackBehavior SelectedTrack { get { if (selectedTracks.Length == 1) return selectedTracks[0]; else return null; } set { selectedTracks[0] = value; } }
    protected TrackBehavior lockedTrack;
    protected TrackBehavior addedTrack = null;

    public Mode mode = Mode.trackSelection;

    [MenuItem("Window/Errata/SteamPunk/Tracks Editor")]
    public static void Init()
    {
        tracksEditor = GetWindow<TracksEditor>(false, "TracksEditor", true);
        tracksEditor.Show();
        tracksEditor.UpdateSelection();
        if (!defaultAsset)
            defaultAsset = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Art/Dimensions/Steampunk/Prefabs/HalfPipe.prefab");
    }
    protected void OnGUI()
    {
        if (!tracksEditor)
            Init();
        switch (mode)
        {
            case Mode.trackAddition:
                {
                    if (!addedTrack)
                    {
                        //addedTrack = Instantiate(defaultAsset, SelectedTrack.transform.parent).GetComponent<TrackBehavior>();
                        GameObject tempTrack = (GameObject)PrefabUtility.InstantiatePrefab(defaultAsset, SelectedTrack.transform.parent);
                        mode = Mode.trackSelection;
                        return;
                    }
                }
                break;
            case Mode.trackSelection:
                {
                    if (selectedTracks.Length == 1)
                    {
                        SelectedTrack.UpdateConnections();
                        string buttonMsg = "Lock Track";
                        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
                        {
                            fontSize = 20
                        };
                        if (lockedTrack)
                        {
                            buttonMsg = "Unlock Track";
                            buttonStyle.normal.textColor = Color.green;
                        }
                        else
                        {
                            buttonStyle.normal.textColor = Color.red;
                        }
                        if (GUILayout.Button(buttonMsg, buttonStyle))
                        {
                            if (lockedTrack)
                            {
                                lockedTrack = null;
                                UpdateSelection();
                            }
                            else
                            {
                                lockedTrack = SelectedTrack;
                            }
                        }


                        Object target = SelectedTrack;
                        SerializedObject serializedObject = new SerializedObject(target);
                        SerializedProperty conncetionsList = serializedObject.FindProperty("connections");
                        EditorGUILayout.PropertyField(conncetionsList, true);
                        serializedObject.ApplyModifiedProperties();

                        int buttonsPerRow = 1;
                        float buttonsWidth = tracksEditor.position.width - GUI.skin.button.margin.left * (float)(buttonsPerRow + 1);
                        buttonsWidth /= buttonsPerRow;
                        if (GUILayout.Button("Add Track", GUILayout.Width(buttonsWidth)))
                        {
                            mode = Mode.trackAddition;
                            return;
                        }
                    }
                    else if (selectedTracks.Length > 1)
                    {

                    }
                    else
                    {
                        EditorGUILayout.LabelField("No tracks selected.", EditorStyles.boldLabel);
                    }
                }
                break;
        }
    }
    protected void UpdateSelection()
    {
        if(!lockedTrack)
            selectedTracks = Selection.GetFiltered<TrackBehavior>(SelectionMode.Assets);
    }
    protected void OnSelectionChange() { UpdateSelection(); Repaint(); }
    protected void OnEnable() { UpdateSelection(); }
    protected void OnFocus() { UpdateSelection(); }
}
#endif