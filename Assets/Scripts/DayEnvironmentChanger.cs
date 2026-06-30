using System;
using System.Collections.Generic;
using UnityEngine;

public class DayEnvironmentChanger : MonoBehaviour
{
    public List<DayFeedback> DayFeedbacks = new();

    private void Start()
    {
        GameManager.Instance.DayStatemachine.DayChanged += DayChanged;
        DayChanged();
    }

    private void DayChanged()
    {
        int currentDay = GameManager.Instance.DayStatemachine.currentDay;

        foreach (DayFeedback day in DayFeedbacks)
        {
            day.Update(currentDay);
        }
    }


    [Serializable]
    public class DayFeedback
    {
        public int Day;
        public DayFeedbackComparation Comparation;

        public enum DayFeedbackComparation
        {
            Equal,
            NotEqual,
            Less,
            LessOrEqual,
            More,
            MoreOrEqual
        }

        public GameObject objToEnable;

        public void Update(int currentDay)
        {
            objToEnable.SetActive(CanIEnable(currentDay));
        }

        private bool CanIEnable(int currentDay)
        {
            return Comparation switch
            {
                DayFeedbackComparation.Equal => Day == currentDay,
                DayFeedbackComparation.NotEqual => Day != currentDay,
                DayFeedbackComparation.Less => Day < currentDay,
                DayFeedbackComparation.LessOrEqual => Day <= currentDay,
                DayFeedbackComparation.More => Day > currentDay,
                DayFeedbackComparation.MoreOrEqual => Day >= currentDay,
                _ => false
            };
        }
    }
}