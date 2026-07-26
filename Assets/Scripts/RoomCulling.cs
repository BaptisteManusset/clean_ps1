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

    private void Awake()
    {
        m_room = GetComponent<Room>();
        DisableLights();
    }

    public void DisableLights()
    {
        foreach (Light l in Lights)
        {
            l.gameObject.SetActive(false);
        }
    }

    public void EnableLights()
    {
        foreach (Light l in Lights)
        {
            l.gameObject.SetActive(true);
        }
    }

    private void Reset()
    {
        ListLights();
    }

#if UNITY_EDITOR
    [EditorButton]
    private void ListLights()
    {
        Lights = GetComponentsInChildren<Light>(true).ToList();
        EditorUtility.SetDirty(this);
    }
#endif
}