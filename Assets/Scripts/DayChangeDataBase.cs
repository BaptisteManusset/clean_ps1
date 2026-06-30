using System;
using UnityEngine;

[Serializable]
public class DayComparationGroup
{
    public int Day;
    public DayFeedbackComparation Comparation;

    public bool IsValid(int currentDay)
    {
        return Comparation switch
        {
            DayFeedbackComparation.Equal => Day == currentDay,
            DayFeedbackComparation.NotEqual => Day != currentDay,
            DayFeedbackComparation.Less => Day < currentDay,
            DayFeedbackComparation.LessOrEqual => Day <= currentDay,
            DayFeedbackComparation.More => Day > currentDay,
            DayFeedbackComparation.MoreOrEqual => Day >= currentDay,
            DayFeedbackComparation.Always => true,
            DayFeedbackComparation.Never => false,
            _ => false
        };
    }
}

public enum DayFeedbackComparation
{
    Equal,
    NotEqual,
    Less,
    LessOrEqual,
    More,
    MoreOrEqual,
    Always,
    Never
}

[Serializable]
public abstract class DayChangeDataBase<T>
{
    public int Day;
    public DayFeedbackComparation Comparation;


    public T objToEnable;

    public abstract T Execute(int currentDay);

    protected bool CanIEnable(int currentDay)
    {
        return Comparation switch
        {
            DayFeedbackComparation.Equal => Day == currentDay,
            DayFeedbackComparation.NotEqual => Day != currentDay,
            DayFeedbackComparation.Less => Day < currentDay,
            DayFeedbackComparation.LessOrEqual => Day <= currentDay,
            DayFeedbackComparation.More => Day > currentDay,
            DayFeedbackComparation.MoreOrEqual => Day >= currentDay,
            DayFeedbackComparation.Always => true,
            DayFeedbackComparation.Never => false,
            _ => false
        };
    }
}

[Serializable]
public class GameObjectData : DayChangeDataBase<GameObject>
{
    public override GameObject Execute(int currentDay)
    {
        objToEnable.SetActive(CanIEnable(currentDay));
        return objToEnable;
    }
}