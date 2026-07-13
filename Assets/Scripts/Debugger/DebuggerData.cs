#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "DebuggerData", menuName = ">>>>/Debugger data", order = 0)]
public class DebuggerData : ScriptableObject
{
    public List<Entry> Entries = new();

    public void AddEntry(Entry entry)
    {
        Entries.Add(entry);
        EditorUtility.SetDirty(this);
    }

    [Serializable]
    public class Entry
    {
        public Quaternion rotation;
        public Vector3 position;

        public static Entry Create(Player player)
        {
            Entry instance = new()
            {
                position = GameManager.Instance.player.transform.position,
                rotation = GameManager.Instance.Cam.transform.rotation
            };

            return instance;
        }
    }
}
#endif