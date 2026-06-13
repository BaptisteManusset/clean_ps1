using System.Collections;
using TMPro;
using UnityEngine;

public class CenterMessage : SceneSingleton<CenterMessage>
{
    private TMP_Text label;


    protected override void Awake()
    {
        base.Awake();
        label = GetComponent<TMP_Text>();
        label.enabled = false;
    }

    public void PublishMessage(string message)
    {
        StartCoroutine(DisplayMessage(message));
    }

    private IEnumerator DisplayMessage(string message)
    {
        label.enabled = true;
        label.text = message;
        yield return new WaitForSeconds(1);
        label.text = "";
        label.enabled = false;
    }
}