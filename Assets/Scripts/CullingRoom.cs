using UnityEngine;

public class CullingRoom : MonoBehaviour
{
    private Room room;

    [SerializeField]
    Renderer[] renderers;

    private void Awake()
    {
        room = GetComponent<Room>();
        renderers = GetComponentsInChildren<Renderer>();

        room.OnEntered += OnEntered;
        room.OnExisted += OnExisted;
        
        OnExisted(room);
    }

    private void OnExisted(Room obj)
    {
        foreach (Renderer rend in renderers)
        {
            rend.enabled = false;
        }
    }

    private void OnEntered(Room obj)
    {
        foreach (Renderer rend in renderers)
        {
            rend.enabled = true;
        }
    }
}