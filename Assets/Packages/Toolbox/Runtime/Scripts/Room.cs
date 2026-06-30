using System.Linq;
using UnityEditor;
using UnityEngine;

[DisallowMultipleComponent]
[SelectionBase]
public class Room : MonoBehaviour
{
    [SerializeField] private Bounds m_bounds = new(Vector3.zero, Vector3.zero);

    void Reset()
    {
        CalculateBounds();
    }

    private void CalculateBounds()
    {
        Collider[] colliders = transform.GetComponentsInChildren<Collider>();

        m_bounds = colliders.First().bounds;

        foreach (Collider collider in colliders)
        {
            m_bounds.Encapsulate(collider.bounds);
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (m_bounds.size == Vector3.zero || m_bounds.center == Vector3.zero )
        {
            CalculateBounds();
            EditorUtility.SetDirty(this);
        }

        Gizmos.color = new Color(0.67f, 0.68f, 1f);
        Gizmos.DrawWireCube(m_bounds.center, m_bounds.size);
        Handles.Label(m_bounds.center, gameObject.name, EditorStyles.boldLabel);
    }
}