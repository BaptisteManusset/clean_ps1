using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class ToolsWindows : EditorWindow
{
    [MenuItem("Tools/Tools")]
    private static void ShowWindow()
    {
        var window = GetWindow<ToolsWindows>();
        window.titleContent = new GUIContent("TITLE");
        window.Show();
    }

    private void OnGUI()
    {
        using (new GUILayout.VerticalScope())
        {
            if (GUILayout.Button("Link Doors", GUILayout.Height(EditorGUIUtility.singleLineHeight * 2)))
            {
                if (Selection.gameObjects.Length <= 1) return;
                List<Door> doors = new();

                foreach (GameObject gameObject in Selection.gameObjects)
                {
                    Door door = gameObject.GetComponentInChildren<Door>(true);
                    if (door)
                    {
                        doors.Add(door);
                    }
                }

                for (int i = 0; i < doors.Count; i++)
                {
                    doors[i].SetOtherDoor(doors[(i + 1) % doors.Count]);
                    EditorUtility.SetDirty(doors[i]);
                }
            }

            using (new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Round scale"))
                {
                    foreach (GameObject gameObject in Selection.gameObjects)
                    {
                        gameObject.transform.localScale = gameObject.transform.localScale.Round();
                        EditorUtility.SetDirty(gameObject.transform);
                    }
                }

                if (GUILayout.Button("Wiggle scale"))
                {
                    foreach (GameObject gameObject in Selection.gameObjects)
                    {
                        gameObject.transform.localScale = Vector3.one * Random.Range(0.8f, 1.2f);
                        EditorUtility.SetDirty(gameObject.transform);
                    }
                }

                if (GUILayout.Button("Round Pos"))
                {
                    foreach (GameObject gameObject in Selection.gameObjects)
                    {
                        gameObject.transform.position = gameObject.transform.position.Round();
                        EditorUtility.SetDirty(gameObject.transform);
                    }
                }

                if (GUILayout.Button("Wiggle Rot"))
                {
                    foreach (GameObject gameObject in Selection.gameObjects)
                    {
                        gameObject.transform.localEulerAngles =
                            new Vector3(0, Random.Range(-10, 10), Random.Range(-10, 10));
                        EditorUtility.SetDirty(gameObject.transform);
                    }
                }

                if (GUILayout.Button("Reset Rot"))
                {
                    foreach (GameObject gameObject in Selection.gameObjects)
                    {
                        gameObject.transform.localEulerAngles = Vector3.zero;
                        EditorUtility.SetDirty(gameObject.transform);
                    }
                }
            }

            GUILayout.Label("Rotation");
            using (new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button("-180"))
                {
                    foreach (GameObject gameObject in Selection.gameObjects)
                    {
                        gameObject.transform.localEulerAngles += new Vector3(0, -180, 0);
                        EditorUtility.SetDirty(gameObject.transform);
                    }
                }

                if (GUILayout.Button("-90"))
                {
                    foreach (GameObject gameObject in Selection.gameObjects)
                    {
                        gameObject.transform.localEulerAngles += new Vector3(0, -90, 0);
                        EditorUtility.SetDirty(gameObject.transform);
                    }
                }


                if (GUILayout.Button("-45"))
                {
                    foreach (GameObject gameObject in Selection.gameObjects)
                    {
                        gameObject.transform.localEulerAngles += new Vector3(0, -45, 0);
                        EditorUtility.SetDirty(gameObject.transform);
                    }
                }

                if (GUILayout.Button("0"))
                {
                    foreach (GameObject gameObject in Selection.gameObjects)
                    {
                        gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
                        EditorUtility.SetDirty(gameObject.transform);
                    }
                }


                if (GUILayout.Button("+45"))
                {
                    foreach (GameObject gameObject in Selection.gameObjects)
                    {
                        gameObject.transform.localEulerAngles += new Vector3(0, 45, 0);
                        EditorUtility.SetDirty(gameObject.transform);
                    }
                }

                if (GUILayout.Button("+180"))
                {
                    foreach (GameObject gameObject in Selection.gameObjects)
                    {
                        gameObject.transform.localEulerAngles += new Vector3(0, 180, 0);
                        EditorUtility.SetDirty(gameObject.transform);
                    }
                }
            }

            GUILayout.FlexibleSpace();
            if (GUILayout.Button("TP player") && Application.isPlaying)
            {
                Player.Instance.Teleport(SceneView.lastActiveSceneView.camera.transform);
                Player.Instance.transform.eulerAngles = Vector3.forward;
            }

            GUILayout.FlexibleSpace();
            using (new GUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("state",
                    GameManager.Instance ? GameManager.Instance.DayStatemachine.CurrentState.ToString() : "Undefined");
                
                if (GUILayout.Button("Next") && Application.isPlaying)
                {
                    GameManager.Instance.DayStatemachine.NextState();
                }
            }

            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("Items", EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Scan"))
                {
                    ScanItems();
                }
            }

            if (counts.Count == 0)
            {
                ScanItems();
            }
            else
            {
                foreach (KeyValuePair<ItemData, int> keyValuePair in counts)
                {
                    using (new GUILayout.HorizontalScope())
                    {
                        EditorGUILayout.LabelField(keyValuePair.Key.name, $"{keyValuePair.Value}");
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("Select"))
                        {
                            Selection.objects = (from interactorItem in items
                                where interactorItem.itemType == keyValuePair.Key
                                select interactorItem.gameObject).ToArray();
                        }
                    }
                }
            }
        }
    }

    private void ScanItems()
    {
        items = new List<InteractorItem>();
        counts = new Dictionary<ItemData, int>();

        items = FindObjectsByType<InteractorItem>(FindObjectsSortMode.None).ToList();

        foreach (InteractorItem item in items)
        {
            counts.TryAdd(item.itemType, 0);

            counts[item.itemType] += item.count;
        }
    }

    List<InteractorItem> items = new();

    Dictionary<ItemData, int> counts = new();
}