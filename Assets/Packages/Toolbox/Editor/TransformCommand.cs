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
}
