using System.Collections.Generic;
using UnityEngine;

public class DayEnvironmentChanger : MonoBehaviour
{
    public List<GameObjectData> DayFeedbacks = new();

    private void Start()
    {
        GameManager.Instance.DayStatemachine.DayChanged += DayChanged;
        DayChanged();
    }

    private void DayChanged()
    {
        int currentDay = GameManager.Instance.DayStatemachine.currentDay;
        foreach (GameObjectData day in DayFeedbacks)
        {
            day.Execute(currentDay);
        }
    }
}