using System;

public class DayStatemachine : SimpleStateMachine
{
    public int currentDay = 0;
    public event Action DayChanged;

    public SimpleState DefaultState;

    private void OnEnable()
    {
        ChangeState(DefaultState);
    }

    public void IncreaseDay()
    {
        currentDay++;
        DayChanged?.Invoke();
    }
}