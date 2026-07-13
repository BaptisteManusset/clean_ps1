using System.Collections.Generic;
using UnityEngine;

public class DayEnvironmentChanger : MonoBehaviour
{
    public List<GameObjectData> DayFeedbacks = new();

    private void Start()
    {
        GameManager.Instance.globalStatemachine.DayChanged += GlobalChanged;
        GlobalChanged();
    }

    private void GlobalChanged()
    {
        int currentDay = GameManager.Instance.globalStatemachine.currentDay;
        foreach (GameObjectData day in DayFeedbacks)
        {
            day.Execute(currentDay);
        }
    }
}