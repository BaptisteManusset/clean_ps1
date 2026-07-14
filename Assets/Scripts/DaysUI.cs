using TMPro;
using UnityEngine;

public class DaysUI : MonoBehaviour
{
    public TMP_Text text;

    private void Start()
    {
        GameManager.Instance.globalStatemachine.DayChanged += OnGlobalChanged;
        OnGlobalChanged();
    }

    private void OnDestroy()
    {
        if (GameManager.Instance) GameManager.Instance.globalStatemachine.DayChanged -= OnGlobalChanged;
    }

    private void OnGlobalChanged()
    {
        text.text = $"jour:{GameManager.Instance.globalStatemachine.currentDay}";
    }
}