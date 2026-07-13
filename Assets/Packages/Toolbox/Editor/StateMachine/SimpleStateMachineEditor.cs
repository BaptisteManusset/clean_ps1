using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SimpleStateMachine), true)]
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

        GUILayout.Label("States");
        foreach (ISimpleState simpleState in sm.States)
        {
            SimpleState state = (SimpleState)simpleState;
            if (GUILayout.Button($"{(state.IsPlaying ? "> " : "")}{state.gameObject.name}"))
            {
                Selection.activeGameObject = state.gameObject;
            }
        }
    }
}