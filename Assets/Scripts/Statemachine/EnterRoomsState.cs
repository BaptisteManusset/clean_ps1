using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public class EnterRoomsState : SimpleState
{
    [SerializeField] private Room[] rooms;

    [SerializeField] SerializedDictionary<Room, bool> m_states = new();


    private void Awake()
    {
        if (rooms.Length == 0)
        {
            Debug.LogWarning("no rooms found ", gameObject);
            return;
        }

        if (rooms.Any(x => x == null))
        {
            Debug.LogWarning("finding missing ref", gameObject);
            rooms = rooms.Where(x => x != null).ToArray();
            return;
        }

        foreach (Room room in rooms)
        {
            m_states.Add(room, false);
        }
    }

    public override void Enter()
    {
        foreach (Room room in rooms)
        {
            room.OnEntered += OnEntered;
            room.OnExisted += OnExisted;
        }
    }

    public override void Exit()
    {
        foreach (Room room in rooms)
        {
            room.OnEntered -= OnEntered;
            room.OnExisted -= OnExisted;
        }
    }

    private void OnEntered(Room room)
    {
        Debug.Log("Player Entered");
    }

    private void OnExisted(Room room)
    {
        m_states[room] = true;
        Debug.Log("Player Existed");

        OnUpdate();
    }

    private void OnUpdate()
    {
        string text = "";
        foreach ((Room room, bool state) in m_states)
        {
            text += $"{(state ? "<b>" : "")}{room.gameObject.name}{(state ? "</b>" : "")}\n";
        }
        
        TodoListUI.Instance.SetText(text);
        
        
        if (m_states.All(x => x.Value))
        {
            m_stateMachine.NextState();
        }
    }
}