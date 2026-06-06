using System;
using UnityEngine;

[SelectionBase]
public class AutoSelect : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, Vector3.one);
    }
}