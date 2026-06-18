using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FixWall : MonoBehaviour
{
    public List<GameObject> selectes  = new();

    [ContextMenu("Fixe wall")]
    public void Fixe()
    {
        for (int i = selectes.Count - 1; i >= 0; i--)
        {
            if(selectes[i].name.Contains("corner")) continue;
            selectes[i].transform.localEulerAngles += new Vector3(0, -90, 0);
            EditorUtility.SetDirty(selectes[i].transform);
        }
    }
}