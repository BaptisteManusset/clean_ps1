#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BookmarkScriptable : SingletonScriptableObject<BookmarkScriptable>
{
    [SerializeField] private int m_currentIndex = 0;

    [SerializeField] private List<BookmarkFolder> m_folders = new();

    public int FolderCount => m_folders.Count;

    private void OnValidate()
    {
        IncrementIndex(0);
    }

    public List<BookmarkElement> GetCurrentList()
    {
        if (m_folders[GetIndex()].List == null)
        {
            m_folders[GetIndex()].List = new List<BookmarkElement>();
        }

        return m_folders[GetIndex()].List;
    }

    public string[] GetFolderList()
    {
        return m_folders.Select(x => x.Name).ToArray();
    }

    public string GetCurrentName()
    {
        return m_folders.Count == 0 ? "Default" : m_folders[GetIndex()].Name;
    }

    public int GetIndex()
    {
        return m_folders.Count == 0 ? 0 : m_currentIndex;
    }

    public void IncrementIndex(int a_value)
    {
        SetIndex(m_currentIndex + a_value);
    }
    public void SetIndex(int a_value)
    {
        m_currentIndex = a_value;
        if (m_currentIndex < 0)
        {
            m_currentIndex = m_folders.Count - 1;
        }
        else if (m_currentIndex >= m_folders.Count)
        {
            m_currentIndex = 0;
        }
    }

    public static void CreateSettings(string a_path)
    {
        BookmarkScriptable asset = CreateInstance<BookmarkScriptable>();

        AssetDatabase.CreateAsset(asset, a_path);
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }

    public void Add(string a_bookmarkElement)
    {
        GetCurrentList().Add(new BookmarkElement(a_bookmarkElement));
    }

    [Serializable]
    public class BookmarkFolder
    {
        [SerializeField] private string m_name;
        [SerializeField] private List<BookmarkElement> m_list = new();

        public List<BookmarkElement> List
        {
            get => m_list;
            set => m_list = value;
        }

        public string Name
        {
            get => m_name;
            set => m_name = value;
        }
    }

    [Serializable]
    public class BookmarkElement
    {
        [SerializeField] private string m_name;
        [SerializeField] private string m_path;
        [SerializeField] private Texture m_icon;

        public BookmarkElement(string a_path, Texture a_icon = null)
        {
            m_path = a_path;
            m_name = Path.GetFileName(a_path);
            m_icon = AssetDatabase.GetCachedIcon(m_path);
        }

        public string Name => m_name;
        public string AssetPath => m_path;
        public Texture Icon => m_icon;
    }

    public void CreateNewFolder(string a_name, List<BookmarkElement> a_elements = null)
    {
        m_folders.Add(new BookmarkFolder() { Name = a_name, List = a_elements });
        m_currentIndex = m_folders.Count - 1;
    }
}
#endif