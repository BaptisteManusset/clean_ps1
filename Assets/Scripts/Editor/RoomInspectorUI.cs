using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEditor;
using UnityEngine;


public class RoomInspectorUI : EditorWindow
{
    private List<Room> rooms = new();

    private int index = 0;
    public Texture doorIcon;
    public Texture roomIcon;
    private Vector2 pos;
    GUIStyle style;


    [MenuItem("Tools/Room inspector")]
    private static void ShowWindow()
    {
        RoomInspectorUI window = GetWindow<RoomInspectorUI>();
        window.titleContent = new GUIContent("TITLE");
        window.Show();
        window.autoRepaintOnSceneChange = true;
    }


    private void OnEnable()
    {
        rooms = FindObjectsByType<Room>(FindObjectsSortMode.InstanceID).ToList();
        Selection.selectionChanged += OnSelectionChanged;
        SceneView.duringSceneGui += OnSceneGUI;

        style = new GUIStyle(EditorStyles.largeLabel)
        {
            fontSize = 20,
            alignment = TextAnchor.MiddleCenter
        };
    }

    private void OnDisable()
    {
        Selection.selectionChanged -= OnSelectionChanged;
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnSelectionChanged()
    {
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        Handles.color = Color.red;
        // Handles.DrawWireDisc(area.center, new Vector3(0, 1, 0), area.radius);

        foreach (Room room in rooms)
        {
            if (SceneVisibilityManager.instance.IsHidden(room.gameObject)) continue;
            for (int i = 0; i < room.Doors.Count; i++)
            {
                if (room.Doors[i] == null) continue;
                Handles.Label(room.Doors[i].gameObject.transform.position + Vector3.up, i.ToString(), style);
            }
        }

        SceneView.RepaintAll();
    }

    private void OnGUI()
    {
        if (GUILayout.Button("exit focus"))
        {
            SceneVisibilityManager.instance.ExitIsolation();
        }

        using (EditorGUILayout.ScrollViewScope scope = new(pos))
        {
            pos = scope.scrollPosition;

            foreach (Room room in rooms)
            {
                if (SceneVisibilityManager.instance.IsHidden(room.gameObject)) continue;
                DrawRoom(room);
            }
        }
    }

    private void DrawRoom(Room room)
    {
        using (new EditorGUILayout.VerticalScope(GUI.skin.box))
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                if (Selection.activeGameObject == room.gameObject) GUI.color = Color.green;

                if (GUILayout.Button(new GUIContent($"{room.gameObject.name}", roomIcon),
                        EditorStyles.largeLabel, GUILayout.Height(EditorGUIUtility.singleLineHeight * 2)))
                {
                    Selection.activeGameObject = room.gameObject;
                }

                GUI.color = Color.white;

                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Update doors names"))
                {
                    room.SetDoorsNames();
                }

                if (GUILayout.Button("Focus"))
                {
                    SceneVisibilityManager.instance.Isolate(room.gameObject, true);
                    Selection.activeGameObject = room.gameObject;
                    SceneView.lastActiveSceneView.FrameSelected(room.gameObject);
                }
            }

            for (int d = 0; d < room.Doors.Count; d++)
            {
                Door door = room.Doors[d];
                using (new EditorGUILayout.HorizontalScope(GUI.skin.textArea))
                {
                    using (new EditorGUILayout.HorizontalScope(GUILayout.Width(200)))
                    {
                        GUILayout.Label(d.ToString(), EditorStyles.boldLabel, GUILayout.Width(10));
                        DrawDoor(door);
                    }

                    using (new EditorGUILayout.VerticalScope(GUI.skin.textArea))
                    {
                        if (door.Getter.Destination)
                        {
                            DrawDoor(door.Getter.Destination);
                        }
                        else
                        {
                            EditorGUILayout.HelpBox("No Destination", MessageType.Warning);
                        }

                        if (door.Getter.RuledDestinations.Count == 0) continue;


                        foreach ((DayCompareGroup dayCompareGroup,
                                     SerializedDictionary<Door, float> destinationDoors)
                                 in door.Getter.RuledDestinations)
                        {
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                GUILayout.Label(dayCompareGroup.ToString(), EditorStyles.boldLabel,
                                    GUILayout.Width(50));
                                using (new EditorGUILayout.VerticalScope())
                                {
                                    for (int i = 0; i < destinationDoors.Count; i++)
                                    {
                                        using (new EditorGUILayout.HorizontalScope())
                                        {
                                            Door selectedDoor = destinationDoors.ElementAt(i).Key;
                                            destinationDoors[selectedDoor] =
                                                EditorGUILayout.FloatField(destinationDoors[selectedDoor],
                                                    GUILayout.Width(40));
                                            DrawDoor(selectedDoor);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void DrawDoor(Door door)
    {
        if (door == null) return;
        if (Selection.activeGameObject == door.gameObject) GUI.color = Color.green;

        if (GUILayout.Button(new GUIContent(door.gameObject.name, door.gameObject.name),
                GUILayout.Height(EditorGUIUtility.singleLineHeight)))
        {
            Selection.activeGameObject = door.gameObject;
        }

        GUI.color = Color.white;
    }
}