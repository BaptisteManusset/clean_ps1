using UnityEngine;

public class Player : MonoBehaviour
{

    public RoomAgent RoomAgent;
    public Light Light;

    private void Awake()
    {
        RoomAgent = GetComponent<RoomAgent>();
        Light.gameObject.SetActive(true);
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