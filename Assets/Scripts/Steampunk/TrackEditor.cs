#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEditor;
using NaughtyAttributes;

[ExecuteInEditMode()]

public class TrackEditor : EditorWindow
{
    public static TrackEditor trackEditor;

    public List<BasicTrack> selectedTracks;
    public bool lockedTrack = false;
    public TrackMapController selectedTrackMapController;

    public static GameObject defaultPrefab;
    public BasicTrack SelectedTrack
    {
        get
        {
            if (selectedTracks.Count == 1)
                return selectedTracks[0];
            else
                return null;
        }
        private set { }
    }

    [MenuItem("Window/Errata/Track Editor")]
    public static void Init()
    {
        trackEditor = GetWindow<TrackEditor>();
        trackEditor.Show();
        trackEditor.OnSelectionChange();
        if(!defaultPrefab)
            defaultPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(ModelTrack.prefabPath);
    }

    protected void OnInspectorUpdate()
    {
        Repaint();
    }

    private int selectedLevel = (int)BasicTrack.NeighborLevel.same;
    private Vector3 creationPoint = new Vector3(0, 0, 0);
    private Vector3Int selection = new Vector3Int(0, 0, 0);
    public void OnGUI()
    {
        if (!trackEditor)
        {
            Init();
        }
        //OnSelectionChange();

        if (SelectedTrack) // one-track mode
        {
            #region -Lock/Unlock button-
            GUIStyle localButtonStyle = new GUIStyle(GUI.skin.button)
            {
                fixedHeight = 40,
                fontSize = 20,
                normal =
                {
                    textColor = new Color(1, 0.2f, 0.2f)
                }
            };
            if (lockedTrack)
            {
                localButtonStyle.normal.textColor = Color.green;
            }
            if (GUILayout.Button(lockedTrack ? "Unlock track" : "Lock track", localButtonStyle))
            {
                lockedTrack = !lockedTrack;
                OnSelectionChange();
                return;
            }

            #endregion

            #region -Styles-

            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
            {
                fixedHeight = 40,
                fontSize = 20,
                normal =
                {
                    textColor = new Color(1, 0.2f, 0.2f)
                }
            };
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 20
            };
            EditorGUILayout.LabelField("Selected level:", labelStyle);
            string[] values = Enum.GetNames(typeof(BasicTrack.NeighborLevel));
            values = values.Take(values.Length - 1).ToArray();
            labelStyle = new GUIStyle("toggle")
            {
                fontSize = 20
            };
            selectedLevel = GUILayout.SelectionGrid(selectedLevel, values, values.Length, labelStyle);
            labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 20
            };
            float buttonWidthHeight = (trackEditor.position.width - 4 * buttonStyle.margin.right) / 3;
            buttonStyle = new GUIStyle(buttonStyle)
            {
                fixedWidth = buttonWidthHeight
            };
            GUIStyle addButtonStyle = new GUIStyle(buttonStyle)
            {
                normal =
                {
                    textColor = Color.green
                },
                fixedHeight = buttonWidthHeight
            };
            GUIStyle removeButtonStyle = new GUIStyle(buttonStyle)
            {
                normal =
                {
                    textColor = new Color(1, 0.25f, 0.25f)
                },
                fixedHeight = buttonWidthHeight / 2
            };

            #endregion

            #region -Track Info-

            EditorGUILayout.LabelField($"Position in TrackGroup: (X: {SelectedTrack.position.x}, Y: {SelectedTrack.position.y}, Z: {SelectedTrack.position.z})", labelStyle, GUILayout.MinHeight(30));

            #endregion

            #region -Neighbor Track Management-

            bool[] buttonType = new bool[(int)BasicTrack.NeighborPosition.end];
            for(int position = 0; position != (int)BasicTrack.NeighborPosition.end; ++position)
            {
                buttonType[position] = SelectedTrack.NeighborTracks[selectedLevel, position] == null;
            }

            BasicTrack.NeighborPosition cameraRotation = BasicTrack.NeighborPosition.end;
            var scene = SceneView.lastActiveSceneView;
            if (scene)
            {
                var cameraAngle = scene.camera.transform.rotation.eulerAngles.y;

                if (cameraAngle > 45f && cameraAngle <= 135f) // Xplus
                {
                    cameraRotation = BasicTrack.NeighborPosition.Xplus;
                }
                else if (cameraAngle > 135f && cameraAngle <= 225f) // Zminus
                {
                    cameraRotation = BasicTrack.NeighborPosition.Zminus;
                }
                else if (cameraAngle > 225f && cameraAngle <= 315f) // Xminus
                {
                    cameraRotation = BasicTrack.NeighborPosition.Xminus;
                }
                else if (cameraAngle > 315f || cameraAngle <= 45f) // Zplus
                {
                    cameraRotation = BasicTrack.NeighborPosition.Zplus;
                }
            }

            switch (cameraRotation)
            {
                case BasicTrack.NeighborPosition.Xplus: // 315..45 degrees
                    {
                        #region -Add/Remove Track Xplus-

                        GUILayout.BeginHorizontal();

                        GUILayout.FlexibleSpace();
                        if (buttonType[(int)BasicTrack.NeighborPosition.Xplus]) // add track
                        {
                            if (GUILayout.Button("Add Track\nX+", addButtonStyle))
                            {
                                AddTrack(new BasicTrack.TrackConnectionInfo(SelectedTrack, (BasicTrack.NeighborLevel)selectedLevel, BasicTrack.NeighborPosition.Xplus));
                                OnGUI();
                                return;
                            }
                        }
                        else // remove track
                        {
                            GUILayout.BeginVertical();
                            if (GUILayout.Button("Remove Track\nX+", removeButtonStyle))
                            {
                                RemoveTrack(new BasicTrack.TrackConnectionInfo(SelectedTrack, (BasicTrack.NeighborLevel)selectedLevel, BasicTrack.NeighborPosition.Xplus));
                                OnGUI();
                                GUILayout.EndVertical();
                                GUILayout.EndHorizontal();
                                return;
                            }
                            GUI.enabled = false;
                            EditorGUILayout.ObjectField(SelectedTrack.NeighborTracks[selectedLevel, (int)BasicTrack.NeighborPosition.Xplus].gameObject, typeof(GameObject), true, GUILayout.Height(buttonWidthHeight / 2), GUILayout.Width(buttonWidthHeight));
                            GUI.enabled = true;
                            GUILayout.EndVertical();
                        }
                        GUILayout.FlexibleSpace();

                        GUILayout.EndHorizontal();

                        #endregion

                        GUILayout.BeginHorizontal();

                        #region -Add/Remove Track Zplus- 

                        if (buttonType[(int)BasicTrack.NeighborPosition.Zplus]) // add track
                        {
                            if (GUILayout.Button("Add Track\nZ+", addButtonStyle))
                            {
                                AddTrack(new BasicTrack.TrackConnectionInfo(SelectedTrack, (BasicTrack.NeighborLevel)selectedLevel, BasicTrack.NeighborPosition.Zplus));
                                OnGUI();
                                return;
                            }
                        }
                        else // remove track
                        {
                            var verticalRemoveButtonStyle = new GUIStyle(removeButtonStyle)
                            {
                                fixedHeight = buttonWidthHeight,
                                fixedWidth = buttonWidthHeight / 2
                            };
                            if (GUILayout.Button("Remove\nTrack\nZ+", verticalRemoveButtonStyle))
                            {
                                RemoveTrack(new BasicTrack.TrackConnectionInfo(SelectedTrack, (BasicTrack.NeighborLevel)selectedLevel, BasicTrack.NeighborPosition.Zplus));
                                OnGUI();
                                return;
                            }
                            GUI.enabled = false;
                            EditorGUILayout.ObjectField(SelectedTrack.NeighborTracks[selectedLevel, (int)BasicTrack.NeighborPosition.Zplus].gameObject, typeof(GameObject), true, GUILayout.Height(buttonWidthHeight), GUILayout.Width(buttonWidthHeight / 2));
                            GUI.enabled = true;
                        }

                        #endregion

                        #region -Selected/Locked track GameObject-

                        EditorGUILayout.BeginVertical();

                        GUI.enabled = false;
                        EditorGUILayout.ObjectField(SelectedTrack.gameObject, typeof(GameObject), true, GUILayout.Height(buttonWidthHeight / 2), GUILayout.Width(buttonWidthHeight));
                        GUI.enabled = true;

                        if (GUILayout.Button("Remove Track", removeButtonStyle))
                        {
                            RemoveTrack(SelectedTrack);
                            OnGUI();
                            return;
                        }

                        EditorGUILayout.EndVertical();

                        #endregion

                        #region -Add/Remove Track Zminus- 

                        if (buttonType[(int)BasicTrack.NeighborPosition.Zminus]) // add track
                        {
                            if (GUILayout.Button("Add Track\nZ-", addButtonStyle))
                            {
                                AddTrack(new BasicTrack.TrackConnectionInfo(SelectedTrack, (BasicTrack.NeighborLevel)selectedLevel, BasicTrack.NeighborPosition.Zminus));
                                OnGUI();
                                return;
                            }
                        }
                        else // remove track
                        {
                            var verticalRemoveButtonStyle = new GUIStyle(removeButtonStyle)
                            {
                                fixedHeight = buttonWidthHeight,
                                fixedWidth = buttonWidthHeight / 2
                            };
                            GUI.enabled = false;
                            EditorGUILayout.ObjectField(SelectedTrack.NeighborTracks[selectedLevel, (int)BasicTrack.NeighborPosition.Zminus].gameObject, typeof(GameObject), true, GUILayout.Height(buttonWidthHeight), GUILayout.Width(buttonWidthHeight / 2));
                            GUI.enabled = true;
                            if (GUILayout.Button("Remove\nTrack\nZ-", verticalRemoveButtonStyle))
                            {
                                RemoveTrack(new BasicTrack.TrackConnectionInfo(SelectedTrack, (BasicTrack.NeighborLevel)selectedLevel, BasicTrack.NeighborPosition.Zminus));
                                OnGUI();
                                return;
                            }
                        }

                        #endregion

                        GUILayout.EndHorizontal();

                        #region -Add/Remove Track Xminus-

                        GUILayout.BeginHorizontal();

                        GUILayout.FlexibleSpace();
                        if (buttonType[(int)BasicTrack.NeighborPosition.Xminus]) // add track
                        {
                            if (GUILayout.Button("Add Track\nX-", addButtonStyle))
                            {
                                AddTrack(new BasicTrack.TrackConnectionInfo(SelectedTrack, (BasicTrack.NeighborLevel)selectedLevel, BasicTrack.NeighborPosition.Xminus));
                                OnGUI();
                                return;
                            }
                        }
                        else // remove track
                        {
                            GUILayout.BeginVertical();
                            GUI.enabled = false;
                            EditorGUILayout.ObjectField(SelectedTrack.NeighborTracks[selectedLevel, (int)BasicTrack.NeighborPosition.Xminus].gameObject, typeof(GameObject), true, GUILayout.Height(buttonWidthHeight / 2), GUILayout.Width(buttonWidthHeight));
                            GUI.enabled = true;
                            if (GUILayout.Button("Remove Track\nX-", removeButtonStyle))
                            {
                                RemoveTrack(new BasicTrack.TrackConnectionInfo(SelectedTrack, (BasicTrack.NeighborLevel)selectedLevel, BasicTrack.NeighborPosition.Xminus));
                                OnGUI();
                                GUILayout.EndVertical();
                                GUILayout.EndHorizontal();
                                return;
                            }
                            GUILayout.EndVertical();
                        }
                        GUILayout.FlexibleSpace();

                        GUILayout.EndHorizontal();

                        #endregion
                    }
                    break;
                case BasicTrack.NeighborPosition.Zminus: // 45..135 degrees
                    {
                        #region -Add/Remove Track Zminus-

                        GUILayout.BeginHorizontal();

                        GUILayout.FlexibleSpace();
                        if (buttonType[(int)BasicTrack.NeighborPosition.Zminus]) // add track
                        {
                            if (GUILayout.Button("Add Track\nZ-", addButtonStyle))
                            {
                                AddTrack(new BasicTrack.TrackConnectionInfo(SelectedTrack, (BasicTrack.NeighborLevel)selectedLevel, BasicTrack.NeighborPosition.Zminus));
                                OnGUI();
                                return;
                            }
                        }
                        else // remove track
                        {
                            GUILayout.BeginVertical();
                            if (GUILayout.Button("Remove Track\nZ-", removeButtonStyle))
                            {
                                RemoveTrack(new BasicTrack.TrackConnectionInfo(SelectedTrack, (BasicTrack.NeighborLevel)selectedLevel, BasicTrack.NeighborPosition.Zminus));
                                OnGUI();
                                GUILayout.EndVertical();
                                GUILayout.EndHorizontal();
                                return;
                            }
                            GUI.enabled = false;
                            EditorGUILayout.ObjectField(SelectedTrack.NeighborTracks[selectedLevel, (int)BasicTrack.NeighborPosition.Zminus].gameObject, typeof(GameObject), true, GUILayout.Height(buttonWidthHeight / 2), GUILayout.Width(buttonWidthHeight));
                            GUI.enabled = true;
                            GUILayout.EndVertical();
                        }
                        GUILayout.FlexibleSpace();

                        GUILayout.EndHorizontal();

                        #endregion

                        GUILayout.BeginHorizontal();

                        #region -Add/Remove Track Xplus- 

                        if (buttonType[(int)BasicTrack.NeighborPosition.Xplus]) // add track
                        {
                            if (GUILayout.Button("Add Track\nX+", addButtonStyle))
                            {
                                AddTrack(new BasicTrack.TrackConnectionInfo(SelectedTrack, (BasicTrack.NeighborLevel)selectedLevel, BasicTrack.NeighborPosition.Xplus));
                                OnGUI();
                                return;
                            }
                        }
                        else // remove track
                        {
                            var verticalRemoveButtonStyle = new GUIStyle(removeButtonStyle)
                            {
                                fixedHeight = buttonWidthHeight,
                                fixedWidth = buttonWidthHeight / 2
                            };
                            if (GUILayout.Button("Remove\nTrack\nX+", verticalRemoveButtonStyle))
                            {
                                RemoveTrack(new BasicTrack.TrackConnectionInfo(SelectedTrack, (BasicTrack.NeighborLevel)selectedLevel, BasicTrack.NeighborPosition.Xplus));
                                OnGUI();
                                return;
                            }
                            GUI.enabled = false;
                            EditorGUILayout.ObjectField(SelectedTrack.NeighborTracks[selectedLevel, (int)BasicTrack.NeighborPosition.Xplus].gameObject, typeof(GameObject), true, GUILayout.Height(buttonWidthHeight), GUILayout.Width(buttonWidthHeight / 2));
                            GUI.enabled = true;
                        }

                        #endregion

                        #region -Selected/Locked track GameObject-

                        EditorGUILayout.BeginVertical();

                        GUI.enabled = false;
                        EditorGUILayout.ObjectField(SelectedTrack.gameObject, typeof(GameObject), true, GUILayout.Height(buttonWidthHeight / 2), GUILayout.Width(buttonWidthHeight));
                        GUI.enabled = true;

                        if (GUILayout.Button("Remove Track", removeButtonStyle))
                        {
                            RemoveTrack(SelectedTrack);
                            OnGUI();
                            return;
                        }

                        EditorGUILayout.EndVertical();

                        #endregion

                        #region -Add/Remove Track Xminus- 

                        if (buttonType[(int)BasicTrack.NeighborPosition.Xminus]) // add track
                        {
                            if (GUILayout.Button("Add Track\nX-", addButtonStyle))
                            {
                                AddTrack(new BasicTrack.TrackConnectionInfo(SelectedTrack, (BasicTrack.NeighborLevel)selectedLevel, BasicTrack.NeighborPosition.Xminus));
                                OnGUI();
                                return;
                            }
                        }
                        else // remove track
                        {
                            var verticalRemoveButtonStyle = new GUIStyle(removeButtonStyle)
                            {
                                fixedHeight = buttonWidthHeight,
                                fixedWidth = buttonWidthHeight / 2
                            };
                            GUI.enabled = false;
                            EditorGUILayout.ObjectField(SelectedTrack.NeighborTracks[selectedLevel, (int)BasicTrack.NeighborPosition.Xminus].gameObject, typeof(GameObject), true, GUILayout.Height(buttonWidthHeight), GUILayout.Width(buttonWidthHeight / 2));
                            GUI.enabled = true;
                            if (GUILayout.Button("Remove\nTrack\nX-", verticalRemoveButtonStyle))
                            {
                                RemoveTrack(new BasicTrack.TrackConnectionInfo(SelectedTrack, (BasicTrack.NeighborLevel)selectedLevel, BasicTrack.NeighborPosition.Xminus));
                                OnGUI();
                                return;
                            }
                        }

                        #endregion

                        GUILayout.EndHorizontal();

                        #region -Add/Remove Track Zplus-

                        GUILayout.BeginHorizontal();

                        GUILayout.FlexibleSpace();
                        if (buttonType[(int)BasicTrack.NeighborPosition.Zplus]) // add track
                        {
                            if (GUILayout.Button("Add Track\nZ+", addButtonStyle))
                            {
                                AddTrack(new BasicTrack.TrackConnectionInfo(SelectedTrack, (BasicTrack.NeighborLevel)selectedLevel, BasicTrack.NeighborPosition.Zplus));
                                OnGUI();
                                return;
                            }
                        }
                        else // remove track
                        {
                            GUILayout.BeginVertical();
                            GUI.enabled = false;
                            EditorGUILayout.ObjectField(SelectedTrack.NeighborTracks[selectedLevel, (int)BasicTrack.NeighborPosition.Zplus].gameObject, typeof(GameObject), true, GUILayout.Height(buttonWidthHeight / 2), GUILayout.Width(buttonWidthHeight));
                            GUI.enabled = true;
                            if (GUILayout.Button("Remove Track\nZ+", removeButtonStyle))
                            {
                                RemoveTrack(new BasicTrack.TrackConnectionInfo(SelectedTrack, (BasicTrack.NeighborLevel)selectedLevel, BasicTrack.NeighborPosition.Zplus));
                                OnGUI();
                                GUILayout.EndVertical();
                                GUILayout.EndHorizontal();
                                return;
                            }
                            GUILayout.EndVertical();
                        }
                        GUILayout.FlexibleSpace();

                        GUILayout.EndHorizontal();

                        #endregion
                    }
                    break;
                case BasicTrack.NeighborPosition.Xminus: // 135..225 degrees
                    {
                        #region -Add/Remove Track Xminus-

                        GUILayout.BeginHorizontal();

                        GUILayout.FlexibleSpace();
                        if (buttonType[(int)BasicTrack.NeighborPosition.Xminus]) // add track
                        {
                            if (GUILayout.Button("Add Track\nX-", addButtonStyle))
                            {
                                AddTrack(new BasicTrack.TrackConnectionInfo(SelectedTrack, (BasicTrack.NeighborLevel)selectedLevel, BasicTrack.NeighborPosition.Xminus));
                                OnGUI();
                                return;
                            }
                        }
                        else // remove track
                        {
                            GUILayout.BeginVertical();
                            if (GUILayout.Button("Remove Track\nX-", removeButtonStyle))
                            {
                                RemoveTrack(new BasicTrack.TrackConnectionInfo(SelectedTrack, (BasicTrack.NeighborLevel)selectedLevel, BasicTrack.NeighborPosition.Xminus));
                                OnGUI();
                                GUILayout.EndVertical();
                                GUILayout.EndHorizontal();
                                return;
                            }
                            GUI.enabled = false;
                            EditorGUILayout.ObjectField(SelectedTrack.NeighborTracks[selectedLevel, (int)BasicTrack.NeighborPosition.Xminus].gameObject, typeof(GameObject), true, GUILayout.Height(buttonWidthHeight / 2), GUILayout.Width(buttonWidthHeight));
                            GUI.enabled = true;
                            GUILayout.EndVertical();
                        }
                        GUILayout.FlexibleSpace();

                        GUILayout.EndHorizontal();

                        #endregion

                        GUILayout.BeginHorizontal();

                        #region -Add/Remove Track Zminus- 

                        if (buttonType[(int)BasicTrack.NeighborPosition.Zminus]) // add track
                        {
                            if (GUILayout.Button("Add Track\nZ-", addButtonStyle))
                            {
                                AddTrack(new BasicTrack.TrackConnectionInfo(SelectedTrack, (BasicTrack.NeighborLevel)selectedLevel, BasicTrack.NeighborPosition.Zminus));
                                OnGUI();
                                return;
                            }
                        }
                        else // remove track
                        {
                            var verticalRemoveButtonStyle = new GUIStyle(removeButtonStyle)
                            {
                                fixedHeight = buttonWidthHeight,
                                fixedWidth = buttonWidthHeight / 2
                            };
                            if (GUILayout.Button("Remove\nTrack\nZ-", verticalRemoveButtonStyle))
                            {
                                RemoveTrack(new BasicTrack.TrackConnectionInfo(SelectedTrack, (BasicTrack.NeighborLevel)selectedLevel, BasicTrack.NeighborPosition.Zminus));
                                OnGUI();
                                return;
                            }
                            GUI.enabled = false;
                            EditorGUILayout.ObjectField(SelectedTrack.NeighborTracks[selectedLevel, (int)BasicTrack.NeighborPosition.Zminus].gameObject, typeof(GameObject), true, GUILayout.Height(buttonWidthHeight), GUILayout.Width(buttonWidthHeight / 2));
                            GUI.enabled = true;
                        }

                        #endregion

                        #region -Selected/Locked track GameObject-

                        EditorGUILayout.BeginVertical();

                        GUI.enabled = false;
                        EditorGUILayout.ObjectField(SelectedTrack.gameObject, typeof(GameObject), true, GUILayout.Height(buttonWidthHeight / 2), GUILayout.Width(buttonWidthHeight));
                        GUI.enabled = true;

                        if (GUILayout.Button("Remove Track", removeButtonStyle))
                        {
                            RemoveTrack(SelectedTrack);
                            OnGUI();
                            return;
                        }

                        EditorGUILayout.EndVertical();

                        #endregion

                        #region -Add/Remove Track Zplus- 

                        if (buttonType[(int)BasicTrack.NeighborPosition.Zplus]) // add track
                        {
                            if (GUILayout.Button("Add Track\nZ+", addButtonStyle))
                            {
                                AddTrack(new BasicTrack.TrackConnectionInfo(SelectedTrack, (BasicTrack.NeighborLevel)selectedLevel, BasicTrack.NeighborPosition.Zplus));
                                OnGUI();
                                return;
                            }
                        }
                        else // remove track
                        {
                            var verticalRemoveButtonStyle = new GUIStyle(removeButtonStyle)
                            {
                                fixedHeight = buttonWidthHeight,
                                fixedWidth = buttonWidthHeight / 2
                            };
                            GUI.enabled = false;
                            EditorGUILayout.ObjectField(SelectedTrack.NeighborTracks[selectedLevel, (int)BasicTrack.NeighborPosition.Zplus].gameObject, typeof(GameObject), true, GUILayout.Height(buttonWidthHeight), GUILayout.Width(buttonWidthHeight / 2));
                            GUI.enabled = true;
                            if (GUILayout.Button("Remove\nTrack\nZ+", verticalRemoveButtonStyle))
                            {
                                RemoveTrack(new BasicTrack.TrackConnectionInfo(SelectedTrack, (BasicTrack.NeighborLevel)selectedLevel, BasicTrack.NeighborPosition.Zplus));
                                OnGUI();
                                return;
                            }
                        }

                        #endregion

                        GUILayout.EndHorizontal();

                        #region -Add/Remove Track Xplus-

                        GUILayout.BeginHorizontal();

                        GUILayout.FlexibleSpace();
                        if (buttonType[(int)BasicTrack.NeighborPosition.Xplus]) // add track
                        {
                            if (GUILayout.Button("Add Track\nX+", addButtonStyle))
                            {
                                AddTrack(new BasicTrack.TrackConnectionInfo(SelectedTrack, (BasicTrack.NeighborLevel)selectedLevel, BasicTrack.NeighborPosition.Xplus));
                                OnGUI();
                                return;
                            }
                        }
                        else // remove track
                        {
                            GUILayout.BeginVertical();
                            GUI.enabled = false;
                            EditorGUILayout.ObjectField(SelectedTrack.NeighborTracks[selectedLevel, (int)BasicTrack.NeighborPosition.Xplus].gameObject, typeof(GameObject), true, GUILayout.Height(buttonWidthHeight / 2), GUILayout.Width(buttonWidthHeight));
                            GUI.enabled = true;
                            if (GUILayout.Button("Remove Track\nX+", removeButtonStyle))
                            {
                                RemoveTrack(new BasicTrack.TrackConnectionInfo(SelectedTrack, (BasicTrack.NeighborLevel)selectedLevel, BasicTrack.NeighborPosition.Xplus));
                                OnGUI();
                                GUILayout.EndVertical();
                                GUILayout.EndHorizontal();
                                return;
                            }
                            GUILayout.EndVertical();
                        }
                        GUILayout.FlexibleSpace();

                        GUILayout.EndHorizontal();

                        #endregion
                    }
                    break;
                case BasicTrack.NeighborPosition.Zplus: // 225..315 degrees
                    {
                        #region -Add/Remove Track Zplus-

                        GUILayout.BeginHorizontal();

                        GUILayout.FlexibleSpace();
                        if (buttonType[(int)BasicTrack.NeighborPosition.Zplus]) // add track
                        {
                            if (GUILayout.Button("Add Track\nZ+", addButtonStyle))
                            {
                                AddTrack(new BasicTrack.TrackConnectionInfo(SelectedTrack, (BasicTrack.NeighborLevel)selectedLevel, BasicTrack.NeighborPosition.Zplus));
                                OnGUI();
                                return;
                            }
                        }
                        else // remove track
                        {
                            GUILayout.BeginVertical();
                            if (GUILayout.Button("Remove Track\nZ+", removeButtonStyle))
                            {
                                RemoveTrack(new BasicTrack.TrackConnectionInfo(SelectedTrack, (BasicTrack.NeighborLevel)selectedLevel, BasicTrack.NeighborPosition.Zplus));
                                OnGUI();
                                GUILayout.EndVertical();
                                GUILayout.EndHorizontal();
                                return;
                            }
                            GUI.enabled = false;
                            EditorGUILayout.ObjectField(SelectedTrack.NeighborTracks[selectedLevel, (int)BasicTrack.NeighborPosition.Zplus].gameObject, typeof(GameObject), true, GUILayout.Height(buttonWidthHeight / 2), GUILayout.Width(buttonWidthHeight));
                            GUI.enabled = true;
                            GUILayout.EndVertical();
                        }
                        GUILayout.FlexibleSpace();

                        GUILayout.EndHorizontal();

                        #endregion

                        GUILayout.BeginHorizontal();

                        #region -Add/Remove Track Xminus- 

                        if (buttonType[(int)BasicTrack.NeighborPosition.Xminus]) // add track
                        {
                            if (GUILayout.Button("Add Track\nX-", addButtonStyle))
                            {
                                AddTrack(new BasicTrack.TrackConnectionInfo(SelectedTrack, (BasicTrack.NeighborLevel)selectedLevel, BasicTrack.NeighborPosition.Xminus));
                                OnGUI();
                                return;
                            }
                        }
                        else // remove track
                        {
                            var verticalRemoveButtonStyle = new GUIStyle(removeButtonStyle)
                            {
                                fixedHeight = buttonWidthHeight,
                                fixedWidth = buttonWidthHeight / 2
                            };
                            if (GUILayout.Button("Remove\nTrack\nX-", verticalRemoveButtonStyle))
                            {
                                RemoveTrack(new BasicTrack.TrackConnectionInfo(SelectedTrack, (BasicTrack.NeighborLevel)selectedLevel, BasicTrack.NeighborPosition.Xminus));
                                OnGUI();
                                return;
                            }
                            GUI.enabled = false;
                            EditorGUILayout.ObjectField(SelectedTrack.NeighborTracks[selectedLevel, (int)BasicTrack.NeighborPosition.Xminus].gameObject, typeof(GameObject), true, GUILayout.Height(buttonWidthHeight), GUILayout.Width(buttonWidthHeight / 2));
                            GUI.enabled = true;
                        }

                        #endregion

                        #region -Selected/Locked track GameObject-

                        EditorGUILayout.BeginVertical();

                        GUI.enabled = false;
                        EditorGUILayout.ObjectField(SelectedTrack.gameObject, typeof(GameObject), true, GUILayout.Height(buttonWidthHeight / 2), GUILayout.Width(buttonWidthHeight));
                        GUI.enabled = true;

                        if (GUILayout.Button("Remove Track", removeButtonStyle))
                        {
                            RemoveTrack(SelectedTrack);
                            OnGUI();
                            return;
                        }

                        EditorGUILayout.EndVertical();

                        #endregion

                        #region -Add/Remove Track Xplus- 

                        if (buttonType[(int)BasicTrack.NeighborPosition.Xplus]) // add track
                        {
                            if (GUILayout.Button("Add Track\nX+", addButtonStyle))
                            {
                                AddTrack(new BasicTrack.TrackConnectionInfo(SelectedTrack, (BasicTrack.NeighborLevel)selectedLevel, BasicTrack.NeighborPosition.Xplus));
                                OnGUI();
                                return;
                            }
                        }
                        else // remove track
                        {
                            var verticalRemoveButtonStyle = new GUIStyle(removeButtonStyle)
                            {
                                fixedHeight = buttonWidthHeight,
                                fixedWidth = buttonWidthHeight / 2
                            };
                            GUI.enabled = false;
                            EditorGUILayout.ObjectField(SelectedTrack.NeighborTracks[selectedLevel, (int)BasicTrack.NeighborPosition.Xplus].gameObject, typeof(GameObject), true, GUILayout.Height(buttonWidthHeight), GUILayout.Width(buttonWidthHeight / 2));
                            GUI.enabled = true;
                            if (GUILayout.Button("Remove\nTrack\nX+", verticalRemoveButtonStyle))
                            {
                                RemoveTrack(new BasicTrack.TrackConnectionInfo(SelectedTrack, (BasicTrack.NeighborLevel)selectedLevel, BasicTrack.NeighborPosition.Xplus));
                                OnGUI();
                                return;
                            }
                        }

                        #endregion

                        GUILayout.EndHorizontal();

                        #region -Add/Remove Track Zminus-

                        GUILayout.BeginHorizontal();

                        GUILayout.FlexibleSpace();
                        if (buttonType[(int)BasicTrack.NeighborPosition.Zminus]) // add track
                        {
                            if (GUILayout.Button("Add Track\nZ-", addButtonStyle))
                            {
                                AddTrack(new BasicTrack.TrackConnectionInfo(SelectedTrack, (BasicTrack.NeighborLevel)selectedLevel, BasicTrack.NeighborPosition.Zminus));
                                OnGUI();
                                return;
                            }
                        }
                        else // remove track
                        {
                            GUILayout.BeginVertical();
                            GUI.enabled = false;
                            EditorGUILayout.ObjectField(SelectedTrack.NeighborTracks[selectedLevel, (int)BasicTrack.NeighborPosition.Zminus].gameObject, typeof(GameObject), true, GUILayout.Height(buttonWidthHeight / 2), GUILayout.Width(buttonWidthHeight));
                            GUI.enabled = true;
                            if (GUILayout.Button("Remove Track\nZ-", removeButtonStyle))
                            {
                                RemoveTrack(new BasicTrack.TrackConnectionInfo(SelectedTrack, (BasicTrack.NeighborLevel)selectedLevel, BasicTrack.NeighborPosition.Zminus));
                                OnGUI();
                                GUILayout.EndVertical();
                                GUILayout.EndHorizontal();
                                return;
                            }
                            GUILayout.EndVertical();
                        }
                        GUILayout.FlexibleSpace();

                        GUILayout.EndHorizontal();

                        #endregion
                    }
                    break;
            }

            

            #endregion
        }
        else if (selectedTracks.Count != 0) // multi-track mode
        {
                
        }
        else // no tracks selected
        {
            var labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 20
            };
            GUILayout.Label("No tracks selected.", labelStyle);

            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
            {
                fixedHeight = 40,
                fontSize = 20
            };

            if (selectedTrackMapController && selectedTrackMapController.Count == 0)
            {
                if (GUILayout.Button("Create First Track", buttonStyle))
                {
                    if (!defaultPrefab)
                        defaultPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(ModelTrack.prefabPath);
                    BasicTrack addedTrack = ((GameObject)PrefabUtility.InstantiatePrefab(defaultPrefab, selectedTrackMapController.transform)).GetComponent<BasicTrack>();
                    addedTrack.transform.localPosition = Vector3.zero;
                    selectedTrackMapController.Add(addedTrack, (0, 0, 0));
                    Selection.activeObject = addedTrack;
                }
            }
            else if (selectedTrackMapController && selectedTrackMapController.Count != 0)
            {
                selection = EditorGUILayout.Vector3IntField("Track Position:", selection);
                var newSelection = selectedTrackMapController.Get((selection.x, selection.y, selection.z));
                if (!newSelection)
                    GUI.enabled = false;
                if (GUILayout.Button("Select Track", buttonStyle))
                {
                    Selection.activeGameObject = newSelection.gameObject;
                }
                if (!newSelection)
                    GUI.enabled = true;
            }
            else if (!selectedTrackMapController)
            {
                creationPoint = EditorGUILayout.Vector3Field("Creation Point:", creationPoint);
                if (GUILayout.Button("Create TrackGroup", buttonStyle))
                {
                    GameObject trackGroup = new GameObject("TrackGroup", typeof(TrackMapController));
                    trackGroup.transform.localScale = Vector3.one * ModelTrack.scale;
                    trackGroup.transform.position = creationPoint;
                    GameObject zeroPoint = new GameObject("ZeroPoint");
                    zeroPoint.transform.parent = trackGroup.transform;
                    zeroPoint.transform.localPosition = Vector3.zero;
                    trackGroup.GetComponent<TrackMapController>().zeroPoint = zeroPoint.transform;
                    Selection.activeGameObject = trackGroup;
                }
            }
        }
    }
    public void OnSelectionChange()
    {
        if (!lockedTrack)
        {
            selectedTracks = Selection.GetFiltered<BasicTrack>(SelectionMode.ExcludePrefab).ToList();
            if(selectedTracks.Count == 0)
            {
                var tempSelecion = Selection.GetFiltered<TrackMapController>(SelectionMode.ExcludePrefab);
                selectedTrackMapController = tempSelecion.Length != 0 ? tempSelecion.Last() : null;
            }
            Repaint();
        }
    }
    public void AddTrack(BasicTrack.TrackConnectionInfo tci)
    {
        if(!defaultPrefab)
            defaultPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(ModelTrack.prefabPath);
        BasicTrack addedTrack = ((GameObject)PrefabUtility.InstantiatePrefab(defaultPrefab, tci.track.transform.parent)).GetComponent<BasicTrack>();
        tci.track.trackMapController.Add(addedTrack, tci.track.position + tci.level.ToPosition() + tci.position.ToPosition());
        addedTrack.AlignTo(tci);
        if(!lockedTrack)
            Selection.activeObject = addedTrack;
    }
    public void RemoveTrack(BasicTrack.TrackConnectionInfo tci)
    {
        var objTemp = tci.track.NeighborTracks[(int)tci.level, (int)tci.position];
        tci.track.trackMapController.Remove(objTemp);
        DestroyImmediate(objTemp.gameObject);
    }

    public void RemoveTrack(BasicTrack caller)
    {
        caller.trackMapController.Remove(caller);
        DestroyImmediate(caller.gameObject);
    }
}

#endif