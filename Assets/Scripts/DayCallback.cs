using UnityEngine;

public class DayCallback : MonoBehaviour
{
    public DayCompareGroup compareSettings = new();
    private void Start()
    {
        GameManager.Instance.globalStatemachine.DayChanged += OnDayChanged;
        OnDayChanged();
    }

    private void OnDayChanged()
    {
        bool isValid = compareSettings.IsValid(GameManager.Instance.globalStatemachine.currentDay);
        
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(isValid);
        }
    }

    private void OnValidate()
    {
        gameObject.name = $"Day {compareSettings} {compareSettings.Day} ";
    }
}