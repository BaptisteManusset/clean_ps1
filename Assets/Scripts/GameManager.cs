using UnityEngine;
using UnityEngine.Serialization;


[DefaultExecutionOrder(-100)]
public class GameManager : SceneSingleton<GameManager>
{
    public Camera Cam;
    public Player player;
    [FormerlySerializedAs("DayStatemachine")] public GlobalStatemachine globalStatemachine;
}