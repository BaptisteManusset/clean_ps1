using UnityEngine;

public class Player : MonoBehaviour
{
    public Room CurrentRoom;
    public Light Light;
    
    private void Start()
    {
        SetCurrentRoom();
        Light.gameObject.SetActive(true);
    }

    [EditorButton]
    public void SetCurrentRoom()
    {
        Collider[] results = new Collider[3];
        int size = Physics.OverlapSphereNonAlloc(transform.position, 2, results);
        if (size == 0) return;

        foreach (Collider collider in results)
        {
            Room room = collider.GetComponentInParent<Room>();
            if (!room) continue;
            CurrentRoom = room;
            CurrentRoom.PlayerEnterNewRoom();
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IUsable item))
        {
            item.Use();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IUsable item))
        {
            item.ExitUse();
        }
    }

    public void Teleport(Transform a_destination)
    {
        transform.SetPositionAndRotation(a_destination.position, a_destination.rotation);
        Physics.SyncTransforms();
    }
    public void Teleport(Vector3 a_position)
    {
        transform.SetPositionAndRotation(a_position, transform.rotation);
        Physics.SyncTransforms();
    }
}