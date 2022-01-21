using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TracksEditor : EditorWindow
{
    public static TracksEditor tracksEditor;

    protected TrackBehavior[] selectedTracks;
    protected TrackBehavior lockedTrack;
    protected TrackBehavior selectedTrack { get { return selectedTracks[0]; } set { selectedTracks[0] = value; } }

    [MenuItem("Window/TracksEditor")]
    public static void Init()
    {
        // initialize window, show it, set the properties
        tracksEditor = GetWindow<TracksEditor>(false, "TracksEditor", true);
        tracksEditor.Show();
        tracksEditor.UpdateSelection();
    }
    protected void OnGUI()
    {
        // one-track mode
        if (selectedTracks.Length == 1)
        {
            selectedTrack.UpdateConnections();
            string buttonMsg;
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            if (lockedTrack)
            {
                buttonMsg = "Unlock Track";
                buttonStyle.normal.textColor = Color.green;
            }
            else
            {
                buttonMsg = "Lock Track";
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
                    lockedTrack = selectedTrack;
                }
            }
            Object target = selectedTrack;
            SerializedObject serializedObject = new SerializedObject(target);
            SerializedProperty conncetionsList = serializedObject.FindProperty("connections");
            EditorGUILayout.PropertyField(conncetionsList, true);
            serializedObject.ApplyModifiedProperties();
        }
        // multi-track mode
        else if (selectedTracks.Length > 1)
        {

        }
        else
        {
            EditorGUILayout.LabelField("No tracks selected.", EditorStyles.boldLabel);
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
