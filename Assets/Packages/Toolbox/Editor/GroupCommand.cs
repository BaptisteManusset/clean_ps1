using UnityEditor;
using UnityEngine;

public static class GroupCommand
{
    [MenuItem("GameObject/Group Selected %g")]
    private static void GroupSelected()
    {
        GameObject group = new GameObject("Group");
        Undo.RegisterCreatedObjectUndo(group, "Group Selected");

        foreach (GameObject gameObject in Selection.gameObjects)
        {
            gameObject.transform.SetParent(group.transform);
            Undo.SetTransformParent(gameObject.transform, group.transform, "Group Selected");
        }
        Selection.activeGameObject = group;
    }
}