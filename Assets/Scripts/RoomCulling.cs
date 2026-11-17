using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[DisallowMultipleComponent]
public class RoomCulling : MonoBehaviour
{
    private Room m_room;

    [SerializeField] public List<Light> Lights = new();
    [SerializeField] public List<InteractorItem> Items = new();

    private void Awake()
    {
        m_room = GetComponent<Room>();

        if (Lights.Count + Items.Count == 0)
        {
            DisableObjects();
        }
    }

    public void DisableObjects()
    {
        foreach (Light l in Lights)
        {
            l.gameObject.SetActive(false);
        }

        foreach (InteractorItem o in Items)
        {
            o.gameObject.SetActive(false);
        }
    }

    public void EnableObjects()
    {
        foreach (Light light in Lights)
        {
            light.gameObject.SetActive(true);
        }

        foreach (InteractorItem item in Items)
        {
            item.gameObject.SetActive(true);
        }
    }

    private void Reset()
    {
        ListObjects();
    }

#if UNITY_EDITOR
    [EditorButton]
    private void ListObjects()
    {
        Lights = GetComponentsInChildren<Light>(true).ToList();
        Items = GetComponentsInChildren<InteractorItem>(true).ToList();

        EditorUtility.SetDirty(this);
    }
#endif
}