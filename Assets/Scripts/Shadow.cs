using System.Collections;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    [SerializeField] private float delay = 1;

    [SerializeField] private VisibleEvent visible;

    private bool wait = false;

    private void Awake()
    {
        visible = GetComponent<VisibleEvent>();
    }

    private void OnEnable()
    {
        visible.beginVisible += OnBecameInvisible;
        visible.endVisible += OnEndVisible;
        wait = false;
    }

    private void OnDisable()
    {
        visible.beginVisible -= OnBecameInvisible;
        visible.endVisible -= OnEndVisible;
        wait = false;
    }

    private void OnBecameInvisible()
    {
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(WaitForHide());
        }
    }

    private void OnEndVisible()
    {
    }

    private IEnumerator WaitForHide()
    {
        if (wait) yield break;
        wait = true;
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}