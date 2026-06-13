using UnityEngine;

public class Player : SceneSingleton<Player>
{
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
}