using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
[SelectionBase]
public class Room : MonoBehaviour
{
    private Bounds m_bounds;
    private void OnDrawGizmosSelected()
    {
        Collider[] colliders = transform.GetComponentsInChildren<Collider>();
        
        m_bounds = colliders.First().bounds;

        foreach (Collider collider in colliders)
        {
            m_bounds.Encapsulate(collider.bounds);
        }
        
        Gizmos.color = new Color(0.67f, 0.68f, 1f);
        Gizmos.DrawWireCube(m_bounds.center, m_bounds.size);
    }
}