#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Debugger : MonoBehaviour
{
    public InputAction key;
    public DebuggerData debuggerData;

    private void OnEnable()
    {
        key.performed += OnPerformed;
        key.Enable();
    }


    private void OnDisable()
    {
        key.performed -= OnPerformed;
        key.Disable();
    }


    private void OnPerformed(InputAction.CallbackContext obj)
    {
        debuggerData.AddEntry(DebuggerData.Entry.Create(GameManager.Instance.player));
        Debug.Log("New entry");
    }

    private void OnDrawGizmos()
    {
        for (int i = debuggerData.Entries.Count - 1; i >= 0; i--)
        {
            if (debuggerData.Entries[i] == null) continue;
            Handles.Label(debuggerData.Entries[i].position, i.ToString());

            Handles.ArrowHandleCap(
                0,
                debuggerData.Entries[i].position,
                debuggerData.Entries[i].rotation,
                1,
                EventType.Repaint
            );
        }
    }
}
#endif