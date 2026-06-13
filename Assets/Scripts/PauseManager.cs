using System;

public static class PauseManager
{
    private static bool m_isPaused;

    //Getters
    public static bool IsPaused => m_isPaused;

    //Events
    private static event Action s_gamePaused;
    private static event Action s_gameResume;

    public static event Action GamePaused
    {
        add => s_gamePaused += value;
        remove => s_gamePaused -= value;
    }

    public static event Action GameResume
    {
        add => s_gameResume += value;
        remove => s_gameResume -= value;
    }

    public static void Pause()
    {
        m_isPaused = true;
    }

    public static void Resume()
    {
        m_isPaused = false;
    }
    
 
}