using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SimpleStateMachine),true)]
public class SimpleStateMachineEditor : Editor
{
    SimpleStateMachine sm;
    private void OnEnable()
    {
        sm = (SimpleStateMachine)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("New State"))
        {
            sm.NextState();
        }
    }
}