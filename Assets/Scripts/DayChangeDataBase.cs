using System;
using UnityEngine;

[Serializable]
public class DayCompareGroup
{
    public int Day;
    public DayCompareFlag Comparation;

    public bool IsValid(int currentDay)
    {
        if (Comparation.HasFlag(DayCompareFlag.Equal) && currentDay == Day) return true;
        if (Comparation.HasFlag(DayCompareFlag.Less) && currentDay < Day) return true;
        if (Comparation.HasFlag(DayCompareFlag.More) && currentDay > Day) return true;
        return false;
    }

    public override string ToString()
    {
        if (Comparation.HasFlag(DayCompareFlag.Equal | DayCompareFlag.Less | DayCompareFlag.More)) return "Always ⊤";
        if (Comparation.HasFlag(DayCompareFlag.Equal | DayCompareFlag.Less)) return $"<= {Day}";
        if (Comparation.HasFlag(DayCompareFlag.Equal | DayCompareFlag.More)) return $">= {Day}";
        if (Comparation.HasFlag(DayCompareFlag.Less | DayCompareFlag.More)) return $"=/= {Day}";
        if (Comparation.HasFlag(DayCompareFlag.Equal)) return $"= {Day}";
        if (Comparation.HasFlag(DayCompareFlag.Less)) return $"< {Day}";
        if (Comparation.HasFlag(DayCompareFlag.More)) return $"> {Day}";
        if (Comparation == 0) return "never ⊥";
        return base.ToString();
    }
}

[Flags]
public enum DayCompareFlag
{
    Equal = 1,
    Less = 4,
    More = 8
}

[Serializable]
public abstract class DayChangeDataBase<T>
{
    public DayCompareGroup Comparation;

    public T objToEnable;

    public abstract T Execute(int currentDay);

    protected bool CanIEnable(int currentDay) => Comparation.IsValid(currentDay);
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