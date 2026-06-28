using System;
using TMPro;
using UnityEngine;

public class DaysUI : MonoBehaviour
{
    public TMP_Text text;

    private void Start()
    {
        GameManager.Instance.DayStatemachine.DayChanged += OnDayChanged;
        OnDayChanged();
    }

    private void OnDestroy()
    {
        if (GameManager.Instance) GameManager.Instance.DayStatemachine.DayChanged -= OnDayChanged;
    }

    private void OnDayChanged()
    {
        text.text = $"jour:{GameManager.Instance.DayStatemachine.currentDay}";
    }
}