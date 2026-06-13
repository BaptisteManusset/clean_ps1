#if UNITY_EDITOR
using System.Linq;
using LDLC.Addons.Common.Editor;
using UnityEditor;
using UnityEngine;

public static class ProjectEditorUtils
{
    [MenuItem("Assets/Utils/Get GUID")]
    public static void GetGuid()
    {
        string[] guids = Selection.assetGUIDs;

        if (guids.Length == 1)
        {
            guids.First().CopyToClipboard();
        }

        foreach (string guid in guids)
        {
            Debug.Log($"{AssetDatabase.GUIDToAssetPath(guid)} have the GUID {guid}");
        }
    }
}
#endif