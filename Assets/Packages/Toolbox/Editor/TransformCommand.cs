using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class TransformCommand
{
    [MenuItem("CONTEXT/Transform/Round Position")]
    private static void GroupSelected()
    {
        foreach (GameObject gameObject in Selection.gameObjects)
        {
            gameObject.transform.position = Vector3Int.RoundToInt(gameObject.transform.position);
            EditorUtility.SetDirty(gameObject);
        }
    }

    [MenuItem("CONTEXT/Transform/Normalize positions")]
    private static void NormalizeSelected()
    {
        GameObject[] objs = Selection.gameObjects;
        objs = objs.OrderBy(x => x.transform.position.y).ToArray();
        Vector3 first = objs.First().transform.position;
        Vector3 last = objs.Last().transform.position;
        Vector3 diff = (last - first) / objs.Length;

        for (int i = 0; i < objs.Length; i++)
        {
            objs[i].transform.position = first + diff * i;
            EditorUtility.SetDirty(objs[i]);
        }
    }
}