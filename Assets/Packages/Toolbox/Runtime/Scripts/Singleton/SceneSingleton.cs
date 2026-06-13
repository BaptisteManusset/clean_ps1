using UnityEngine;

public abstract class SceneSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T S_INSTANCE = null;
    public static T Instance => S_INSTANCE;

    #region Domain Reload

// As we are in a package, using RuntimeInitializeOnLoadMethod causes an error on PC at runtime, so keep it only in editor
#if UNITY_EDITOR
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void ClearStaticDomain()
    {
        S_INSTANCE = null;
    }
#endif

    #endregion

    protected virtual void Awake()
    {
        if (S_INSTANCE != null)
            Debug.LogErrorFormat("Multiple {0}/{1} Singleton - OnAwake", gameObject.name, GetType());

        S_INSTANCE = gameObject.GetComponent<T>();
    }

    protected virtual void OnDestroy()
    {
        if (S_INSTANCE == this)
            S_INSTANCE = null;
        else
            Debug.LogErrorFormat("Multiple {0}/{1} Singleton - OnDestroy", gameObject.name, GetType());
    }
}