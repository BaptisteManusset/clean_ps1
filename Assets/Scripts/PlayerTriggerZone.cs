using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerZone : MonoBehaviour
{
    #region Properties
    [SerializeField]
    private List<Collider> m_colliders;

    [SerializeField]
    private bool m_ignoreOnPause = true;

    private event System.Action<PlayerTriggerZone> m_entered;
    public event System.Action<PlayerTriggerZone> Entered
    {
        add => m_entered += value;
        remove => m_entered -= value;
    }

    private event System.Action<PlayerTriggerZone> m_exited;
    public event System.Action<PlayerTriggerZone> Exited
    {
        add => m_exited += value;
        remove => m_exited -= value;
    }

    protected bool m_isInside = false;
    public bool IsInside => m_isInside;

    private bool m_wasEnabledBeforePause;
    #endregion

    #region Unity Methods
    protected virtual void OnEnable()
    {
        foreach (Collider collider in m_colliders)
        {
            collider.enabled = true;
        }
    }

    protected virtual void OnDisable()
    {
        foreach (Collider collider in m_colliders)
        {
            collider.enabled = false;
        }
    }

    private void Awake()
    {
        PauseManager.GamePaused += OnGamePaused;
        PauseManager.GameResume += OnGameResumed;
    }

    private void OnDestroy()
    {
        PauseManager.GamePaused -= OnGamePaused;
        PauseManager.GameResume -= OnGameResumed;
    }

    protected virtual void Update()
    {
        if (Player.Instance == null)
        {
            if (m_isInside)
            {
                SetIsInside(false);
            }
            return;
        }

        bool isInside = GetIsInside(Player.Instance.transform.position);

        if (m_isInside != isInside)
        {
            SetIsInside(isInside);
        }
    }
    #endregion

    private void OnGamePaused()
    {
        if (!m_ignoreOnPause)
            return;

        m_wasEnabledBeforePause = enabled;
        enabled = false;
    }

    private void OnGameResumed()
    {
        if (!m_ignoreOnPause)
            return;

        enabled = m_wasEnabledBeforePause;
    }

    private bool GetIsInside(Vector3 a_position)
    {
        foreach (Collider collider in m_colliders)
        {
            var pos = collider.ClosestPoint(a_position);

            // if (collider.Contains(a_position))
            // {
            //     return true;
            // }
        }

        return false;
    }

    protected virtual void SetIsInside(bool a_isInside)
    {
        m_isInside = a_isInside;

        if (a_isInside)
        {
            m_entered?.Invoke(this);
        }
        else
        {
            m_exited?.Invoke(this);
        }
    }
}
