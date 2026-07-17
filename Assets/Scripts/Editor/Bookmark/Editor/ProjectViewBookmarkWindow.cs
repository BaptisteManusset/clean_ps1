#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class ProjectViewBookmarkWindow : EditorWindow
{
    private Vector2 m_scrollPos;
    private GUIStyle m_button;
    private GUIContent m_settingButton;
    private bool m_isLoaded = false;

    private static string BookmarkLocation => "Assets/Settings/Bookmark.asset";

    [MenuItem("Tools/Project Bookmark", priority = 1)]
    internal static void Init()
    {
        ProjectViewBookmarkWindow window =
            (ProjectViewBookmarkWindow)GetWindow(typeof(ProjectViewBookmarkWindow), false, "Bookmark");
        window.position = new Rect(window.position.xMin + 100f, window.position.yMin + 100f, 300f, 400f);
    }

    private void OnSelectionChange() => Repaint();

    private void OnGUI()
    {
        if (m_isLoaded == false)
        {
            if (!BookmarkScriptable.Instance)
            {
                BookmarkScriptable.CreateSettings(BookmarkLocation);
                BookmarkScriptable.Instance.CreateNewFolder("Default", new List<BookmarkScriptable.BookmarkElement>());
                Debug.Log("Auto generate bookmark settings file");
            }
            LoadUiStyle();
        }

        EditorGUILayout.BeginVertical();
        Header();
        BookmarkList();
        GUILayout.FlexibleSpace();
        Footer();
        GUI.enabled = true;
        EditorGUILayout.EndVertical();
    }


    #region UI elements

    private void Footer()
    {
        string currentPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        EditorGUILayout.HelpBox($"Current : {currentPath}", MessageType.None);
        using (new GUILayout.HorizontalScope())
        {
            if (GUILayout.Button($"Add current selection", m_button, GUILayout.ExpandHeight(true)))
            {
                AddElement(currentPath);
            }


            if (GUILayout.Button(new GUIContent("New folder", "Create a new bookmark folder"),
                    GUILayout.ExpandWidth(false)))
            {
                BookmarkScriptable.Instance.CreateNewFolder("New Folder");
            }
        }
    }

    private void BookmarkList()
    {
        m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos, "Box");

        if (BookmarkScriptable.Instance.GetCurrentList() == null ||
            BookmarkScriptable.Instance.GetCurrentList().Count == 0)
        {
            EditorGUILayout.HelpBox($"No bookmark save.", MessageType.None);
        }
        else
        {
            for (int i = 0; i < BookmarkScriptable.Instance.GetCurrentList().Count; i++)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button(
                            new GUIContent(BookmarkScriptable.Instance.GetCurrentList()[i].Name,
                                BookmarkScriptable.Instance.GetCurrentList()[i].Icon,
                                BookmarkScriptable.Instance.GetCurrentList()[i].AssetPath),
                            GUILayout.Width(80), GUILayout.ExpandWidth(true)))
                    {
                        SelectCurrent(i, Event.current.control);
                    }

                    if (GUILayout.Button(new GUIContent("X", "Remove element"), GUILayout.Width(20)))
                    {
                        BookmarkScriptable.Instance.GetCurrentList().RemoveAt(i);
                        EditorUtility.SetDirty(BookmarkScriptable.Instance);
                        i--;
                    }
                }
            }
        }

        EditorGUILayout.EndScrollView();
    }

    private void Header()
    {
        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button("◀", GUILayout.ExpandWidth(false)))
            {
                BookmarkScriptable.Instance.IncrementIndex(-1);
            }

            EditorGUI.BeginChangeCheck();


            GUILayout.Label("", EditorStyles.boldLabel);
            Rect guiRect = GUILayoutUtility.GetLastRect();

            int index = EditorGUI.Popup(
                guiRect,
                BookmarkScriptable.Instance.GetIndex(),
                BookmarkScriptable.Instance.GetFolderList());
            
            if (EditorGUI.EndChangeCheck())
            {
                BookmarkScriptable.Instance.SetIndex(index);
            }

            if (GUILayout.Button("▶", GUILayout.ExpandWidth(false)))
            {
                BookmarkScriptable.Instance.IncrementIndex(1);
            }

            if (GUILayout.Button(m_settingButton, GUILayout.ExpandWidth(false)))
            {
                Selection.activeObject = BookmarkScriptable.Instance;
            }
        }
    }

    #endregion

    private void LoadUiStyle()
    {
        if (!BookmarkScriptable.Instance)
        {
            BookmarkScriptable.CreateSettings(BookmarkLocation);
        }

        m_button = GUI.skin.GetStyle("Button");
        m_button.alignment = TextAnchor.MiddleLeft;
        m_button.stretchWidth = true;
        m_button.richText = true;
        m_button.fixedHeight = EditorGUIUtility.singleLineHeight;
        
        m_settingButton = EditorGUIUtility.IconContent(EditorGUIUtility.isProSkin ? "d__Menu" : "_Menu");
        m_settingButton.tooltip = "Settings";
    }

    private static void AddElement(string a_currentPath)
    {
        if (a_currentPath != "")
        {
            if (BookmarkScriptable.Instance.GetCurrentList().Any(x => x.AssetPath == a_currentPath) == false)
            {
                BookmarkScriptable.Instance.Add(a_currentPath);
                EditorUtility.SetDirty(BookmarkScriptable.Instance);
            }
        }
    }

    private static void SelectCurrent(int a_index, bool a_controlIsPressed)
    {
        BookmarkScriptable.BookmarkElement element = BookmarkScriptable.Instance.GetCurrentList()[a_index];

        if (element.AssetPath != "")
        {
            Selection.activeObject = null;
            string[] objs = AssetDatabase.FindAssets("", new[] { element.AssetPath });
            foreach (string c in objs)
            {
                Object cObj = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(c), typeof(Object));
                if (Path.GetDirectoryName(AssetDatabase.GetAssetPath(cObj)) == element.AssetPath)
                {
                    Selection.activeObject = cObj;
                    break;
                }
            }

            if (Selection.activeObject == null)
                Selection.activeObject = AssetDatabase.LoadAssetAtPath(element.AssetPath, typeof(Object));

            if (a_controlIsPressed)
            {
                AssetDatabase.OpenAsset(Selection.activeObject);
            }

            EditorUtility.SetDirty(BookmarkScriptable.Instance);
            EditorGUIUtility.PingObject(Selection.activeObject);
        }
        else
        {
            Debug.LogWarning("Oups, Bookmark path is empty. ಠ_ಠ", BookmarkScriptable.Instance);
            EditorUtility.SetDirty(BookmarkScriptable.Instance);
        }
    }
}

#endif