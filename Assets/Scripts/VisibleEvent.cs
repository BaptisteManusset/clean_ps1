using System;
using UnityEditor.Recorder.Input;
using UnityEngine;

public class VisibleEvent : MonoBehaviour
{
    private Renderer m_renderer;

    private bool visible = false;

    public event Action beginVisible;
    public event Action endVisible;

    private void Start()
    {
        m_renderer = GetComponentInChildren<Renderer>();
    }

    private void Update()
    {
        IsVisible();
    }

    public bool IsVisible()
    {
        Vector3 screenPos = GameManager.Instance.Cam.WorldToScreenPoint(transform.position);
        bool onScreen = screenPos.x > 0f && screenPos.x < Screen.width && screenPos.y > 0f &&
                        screenPos.y < Screen.height;

        if (onScreen && m_renderer.isVisible)
        {
            if (!visible)
            {
                beginVisible?.Invoke();
                visible = true;
            }

            return true;
        }

        if (visible)
        {
            endVisible?.Invoke();
            visible = false;
        }

        return false;
    }
}