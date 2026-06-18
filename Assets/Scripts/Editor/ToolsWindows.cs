using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Editor
{
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
            if (GUILayout.Button("Link Doors", GUILayout.Height(EditorGUIUtility.singleLineHeight*2)))
            {
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
                if (GUILayout.Button("Round L Pos"))
                {
                    foreach (GameObject gameObject in Selection.gameObjects)
                    {
                        gameObject.transform.localPosition = gameObject.transform.localPosition.Round();
                        EditorUtility.SetDirty(gameObject.transform);
                    }
                }

                if (GUILayout.Button("Round Pos"))
                {
                    foreach (GameObject gameObject in Selection.gameObjects)
                    {
                        gameObject.transform.position =  gameObject.transform.position.Round();
                        EditorUtility.SetDirty(gameObject.transform);
                    }
                }
            }
        }
    }
}