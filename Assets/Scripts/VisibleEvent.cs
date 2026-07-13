using System;
using UnityEngine;

public class VisibleEvent : MonoBehaviour
{
    private Renderer m_renderer;

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
            beginVisible?.Invoke();
            return true;
        }

        endVisible?.Invoke();
        return false;
    }
}