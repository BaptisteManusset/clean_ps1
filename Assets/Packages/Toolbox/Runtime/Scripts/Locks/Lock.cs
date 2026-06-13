using System;
using System.Collections.Generic;

[Serializable]
public class Lock 
{
    public Lock() : this(false)
    {
    }

    public Lock(bool a_initialSelfLocked)
    {
        m_isSelfLocked = a_initialSelfLocked;
        if (!m_isSelfLocked)
            DoUnlock();
        else
            DoLock();
    }

    #region Properties
    private event System.Action<Lock> m_onLocked;
    public event System.Action<Lock> OnLocked
    {
        add => m_onLocked += value;
        remove => m_onLocked -= value;
    }

    private event System.Action<Lock> m_onUnlocked;
    public event System.Action<Lock> OnUnlocked
    {
        add => m_onUnlocked += value;
        remove => m_onUnlocked -= value;
    }

    private bool m_isSelfLocked;
    public bool IsSelfLocked => m_isSelfLocked;
    public HashSet<object> ExternalLocks => m_externalLocks;

    private bool m_isLocked;
    public bool IsLocked => m_isLocked;
    public bool IsUnlocked => !IsLocked;

    public virtual bool IsExternallyLocked => m_externalLocks.Count > 0;

    protected virtual bool ShouldUnlock => !m_isSelfLocked && !IsExternallyLocked && m_isLocked;

    protected virtual bool ShouldLock => (m_isSelfLocked || IsExternallyLocked) && !m_isLocked;

    protected HashSet<object> m_externalLocks = new();
    #endregion

    public void AddExternalLock(object a_lock)
    {
        m_externalLocks.Add(a_lock);
        if (ShouldLock)
        {
            DoLock();
        }
    }

    public void RemoveExternalLock(object a_lock)
    {
        m_externalLocks.Remove(a_lock);
        if (ShouldUnlock)
        {
            DoUnlock();
        }
    }

    public void ClearExternalLocks(bool a_updateState)
    {
        if (m_externalLocks.Count == 0)
            return;

        m_externalLocks.Clear();

        if (a_updateState && ShouldUnlock)
        {
            DoUnlock();
        }
    }

    public void LockSelf()
    {
        m_isSelfLocked = true;
        if (ShouldLock)
        {
            DoLock();
        }
    }

    public void UnlockSelf()
    {
        m_isSelfLocked = false;
        if (ShouldUnlock)
        {
            DoUnlock();
        }
    }

    protected virtual void DoUnlock()
    {
        m_isLocked = false;
        m_onUnlocked?.Invoke(this);
    }

    protected virtual void DoLock()
    {
        m_isLocked = true;
        m_onLocked?.Invoke(this);
    }
}
