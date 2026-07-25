using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class ShortcutTools : EditorWindow
{
    private static ShortcutTools window;

    [MenuItem("Tools/Tools")]
    private static void ShowWindow()
    {
        window = GetWindow<ShortcutTools>();
        window.titleContent = new GUIContent(nameof(ShortcutTools));
        window.Show();
    }

    private void OnGUI()
    {
        using (new GUILayout.VerticalScope())
        {
            using (new GUILayout.HorizontalScope(GUILayout.Height(EditorGUIUtility.singleLineHeight * 2)))
            {
                if (GUILayout.Button("Link Doors", GUILayout.ExpandHeight(true)))
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

                if (GUILayout.Button("🎲", GUILayout.ExpandHeight(true), GUILayout.MaxWidth(60)))
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
                        doors[i].SetOtherDoor(doors[(i + 1) % doors.Count], true);
                        EditorUtility.SetDirty(doors[i]);
                    }
                }
            }


            if (GUILayout.Button("Set door names"))
            {
                Room[] rooms = FindObjectsByType<Room>(FindObjectsInactive.Include, FindObjectsSortMode.None);

                foreach (Room room in rooms)
                {
                    room.SetDoorsNames();
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
                    WiggleScale();
                }

                if (GUILayout.Button("Round Pos"))
                {
                    RoundPosition();
                }

                if (GUILayout.Button("Wiggle Rot YZ"))
                {
                    WiggleRotation(new Vector3(0, Random.Range(-10, 10), Random.Range(-10, 10)));
                }

                if (GUILayout.Button("Wiggle Rot Y"))
                {
                    WiggleRotation(new Vector3(0, Random.Range(-10, 10), 0));
                }

                if (GUILayout.Button("Reset Rot"))
                {
                    ResetRotation();
                }
            }

            GUILayout.Label("Rotation");
            using (new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button("-180"))
                {
                    IncreaseRotation(new Vector3(0, -180, 0));
                }

                if (GUILayout.Button("-90"))
                {
                    IncreaseRotation(new Vector3(0, -90, 0));
                }


                if (GUILayout.Button("-45"))
                {
                    IncreaseRotation(new Vector3(0, -45, 0));
                }


                if (GUILayout.Button("45"))
                {
                    IncreaseRotation(new Vector3(0, 45, 0));
                }

                if (GUILayout.Button("90"))
                {
                    IncreaseRotation(new Vector3(0, 90, 0));
                }

                if (GUILayout.Button("180"))
                {
                    IncreaseRotation(new Vector3(0, 180, 0));
                }
            }

            using (new EditorGUI.DisabledGroupScope(!Application.isPlaying))
            {
                using (new GUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("TP player"))
                    {
                        GameManager.Instance.player.Teleport(SceneView.lastActiveSceneView.camera.transform);
                        GameManager.Instance.player.transform.eulerAngles = Vector3.forward;
                        GameManager.Instance.player.SetCurrentRoom();
                    }

                    if (GUILayout.Button("DvRoom", GUILayout.Width(EditorGUIUtility.currentViewWidth / 4)))
                    {
                        GameManager.Instance.player.Teleport(Vector3.up * 2);
                        GameManager.Instance.player.transform.eulerAngles = Vector3.forward;
                        GameManager.Instance.player.SetCurrentRoom();
                    }
                }

                // GUILayout.FlexibleSpace();
                // using (new GUILayout.HorizontalScope())
                // {
                //     EditorGUILayout.LabelField("state",
                //         GameManager.Instance
                //             ? GameManager.Instance.globalStatemachine.CurrentState.ToString()
                //             : "Undefined");
                //
                //     if (GUILayout.Button("Next") && Application.isPlaying)
                //     {
                //         GameManager.Instance.globalStatemachine.NextState();
                //     }
                // }


                using (new GUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Lights Off"))
                    {
                        foreach (Light l in FindObjectsByType<Light>(FindObjectsInactive.Include,
                                     FindObjectsSortMode.InstanceID))
                        {
                            l.gameObject.SetActive(false);
                        }
                    }

                    if (GUILayout.Button("Lights On"))
                    {
                        foreach (Light l in FindObjectsByType<Light>(FindObjectsInactive.Include,
                                     FindObjectsSortMode.InstanceID))
                        {
                            l.gameObject.SetActive(true);
                        }
                    }
                }


                // using (new GUILayout.HorizontalScope())
                // {
                //     GUILayout.Label("Day");
                //     for (int i = 1; i < 5; i++)
                //     {
                //         if (GUILayout.Button($"{i}",
                //                 GameManager.Instance.globalStatemachine.IsCurrentDay(i)
                //                     ? EditorStyles.boldLabel
                //                     : EditorStyles.label))
                //         {
                //             GameManager.Instance.globalStatemachine.SetDay(i);
                //         }
                //     }
                //
                //     if (GUILayout.Button($"++"))
                //     {
                //         GameManager.Instance.globalStatemachine.IncreaseDay();
                //     }
                // }
            }

            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("Items", EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Scan"))
                {
                    ScanItems();
                }

                using (new EditorGUI.DisabledScope(Application.isPlaying == false))
                {
                    if (GUILayout.Button("GiveAll"))
                    {
                        if (counts.Count == 0)
                        {
                            ScanItems();
                        }

                        GiveAllItems();
                    }
                }
            }

            if (counts.Count != 0)
            {
                foreach (KeyValuePair<ItemData, int> keyValuePair in counts)
                {
                    using (new GUILayout.HorizontalScope())
                    {
                        EditorGUILayout.LabelField(keyValuePair.Key.name, $"{keyValuePair.Value}");
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("Select"))
                        {
                            Selection.objects = items
                                .Where(interactorItem => interactorItem.itemType == keyValuePair.Key)
                                .Select(interactorItem => interactorItem.gameObject).ToArray();
                        }
                    }
                }
            }
        }
    }

    private void GiveAllItems()
    {
        foreach (InteractorItem item in items)
        {
            Inventory.Instance.AddItem(item.itemType);
        }
  
    }

    private static void IncreaseRotation(Vector3 angleAdded)
    {
        foreach (GameObject gameObject in Selection.gameObjects)
        {
            Undo.RecordObject(gameObject.transform, "Increase rotation");
            gameObject.transform.localEulerAngles += angleAdded;
        }
    }

    private static void WiggleScale()
    {
        foreach (GameObject gameObject in Selection.gameObjects)
        {
            Undo.RecordObject(gameObject.transform, "Wiggle rotation");
            gameObject.transform.localScale = Vector3.one * Random.Range(0.8f, 1.2f);
        }
    }

    private static void RoundPosition()
    {
        foreach (GameObject gameObject in Selection.gameObjects)
        {
            Undo.RecordObject(gameObject.transform, "Round Position");
            gameObject.transform.position = gameObject.transform.position.Round();
        }
    }

    private static void ResetRotation()
    {
        foreach (GameObject gameObject in Selection.gameObjects)
        {
            Undo.RecordObject(gameObject.transform, "Reset rotation");
            gameObject.transform.localEulerAngles = Vector3.zero;
        }
    }

    private static void WiggleRotation(Vector3 newRotation)
    {
        foreach (GameObject gameObject in Selection.gameObjects)
        {
            Undo.RecordObject(gameObject.transform, "Wiggle rotation");
            gameObject.transform.localEulerAngles = newRotation;
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

    private List<InteractorItem> items = new();

    private Dictionary<ItemData, int> counts = new();
}