using System;

public class GlobalStatemachine : SimpleStateMachine
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