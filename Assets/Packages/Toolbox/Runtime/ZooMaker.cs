using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


public class ZooMaker : MonoBehaviour
{
#if UNITY_EDITOR
    [NonSerialized]
    public string AbsoluteFolderPath;

    [SerializeField]
    private string m_folderPath;

    [SerializeField] private float m_caseSize = 2;

    public List<GameObject> m_items = new();


    private void OnDrawGizmosSelected()
    {
        foreach (Transform child in transform)
        {
            Gizmos.DrawWireCube(child.position, .75f * m_caseSize * Vector3.one);
        }
    }

    [EditorButton]
    public void SetPath()
    {
        if (string.IsNullOrEmpty(AbsoluteFolderPath)) AbsoluteFolderPath = Application.dataPath;

        AbsoluteFolderPath = EditorUtility.OpenFolderPanel("Load png Textures", AbsoluteFolderPath, "");
        m_folderPath = AbsoluteFolderPath.Replace(Application.dataPath, "Assets");
        m_items.Clear();


        if (m_folderPath == null) return;
        SpawnObjects();
    }

    [EditorButton]
    private void SpawnObjects()
    {
        if (m_items.Count == 0)
        {
            m_items = AssetDatabase.FindAssets("t=Prefab", new[] { m_folderPath })
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(x => (GameObject)AssetDatabase.LoadAssetAtPath(x, typeof(GameObject)))
                .ToList();
            return;
        }

        foreach (Transform transform in gameObject.transform)
        {
            DestroyImmediate(transform.gameObject);
        }

        int max = Mathf.CeilToInt(Mathf.Sqrt(m_items.Count));

        int total = 0;
        for (int x = 0; x < max; x++)
        {
            for (int z = 0; z < max; z++)
            {
                if (total >= m_items.Count) return;
                Vector3 position = new(x * m_caseSize, 0, z * m_caseSize);
                GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(m_items[total], transform);
                instance.transform.position = position;
                total++;
            }
        }
    }
#endif
}