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
            ListObjects();
        }
    }

    public void DisableObjects()
    {
        foreach (Light lght in Lights)
        {
            lght.gameObject.SetActive(false);
            lght.shadows = LightShadows.None;
        }

        foreach (InteractorItem item in Items)
        {
            item.gameObject.SetActive(false);
        }
    }

    public void EnableObjects()
    {
        foreach (Light lght in Lights)
        {
            lght.shadows = LightShadows.Hard;
            lght.gameObject.SetActive(true);
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

    [EditorButton]
    private void ListObjects()
    {
        Lights = GetComponentsInChildren<Light>(true).ToList();
        Items = GetComponentsInChildren<InteractorItem>(true).ToList();

#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }
}