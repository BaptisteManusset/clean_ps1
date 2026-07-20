using System.Collections;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    [SerializeField] private float delay = 1;
    [SerializeField] private float minDistance = 6;

    [SerializeField] private VisibleEvent visible;

    private bool wait = false;


    private void Awake()
    {
        visible = GetComponent<VisibleEvent>();
    }

    private void OnEnable()
    {
        visible.beginVisible += OnBecameVisible;
        visible.endVisible += OnEndVisible;
        wait = false;
    }

    private void OnDisable()
    {
        visible.beginVisible -= OnBecameVisible;
        visible.endVisible -= OnEndVisible;
        wait = false;
    }

    private void OnBecameVisible()
    {
        if (gameObject.activeInHierarchy)
        {
            if (Vector3.Distance(GameManager.Instance.Cam.transform.position, transform.position) <= minDistance)
            {
                Hide();
                return;
            }

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
        Hide();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}