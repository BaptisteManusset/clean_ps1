using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEditor;
using UnityEngine;

[Serializable]
public class GetterElement
{
    public SerializedDictionary<Door, float> WeigthedDestinations = new();

    public List<Door> Destinations
    {
        get { return WeigthedDestinations.OrderBy(x => x.Value).Select(x => x.Key).ToList(); }
    }


    public Door Get()
    {
        if (WeigthedDestinations.Count == 0)
        {
            Debug.LogError("No destination doors found");

            return null;
        }

        return WeigthedDestinations.RandomElementByWeight(e => e.Value).Key;
    }
}


public class DestinationGetter : MonoBehaviour
{
    [SerializeField]
    public Door Destination;


    public SerializedDictionary<DayCompareGroup, SerializedDictionary<Door, float>> RuledDestinations = new();

    public Door Get() => Get(GameManager.Instance.globalStatemachine.currentDay);

    public void AddDoorToRule(Door door, float probability = 1)
    {
        Undo.RecordObject(this, "Add door destination");
        SerializedDictionary<Door, float> pair = new() { { door, probability } };
        DayCompareGroup compare = new()
        {
            Comparation = DayCompareFlag.Equal,
            Day = 0
        };
        RuledDestinations.Add(compare, pair);
        EditorUtility.SetDirty(this);
        if (!Destination) AddDoor(door);
    }

    public void AddDoor(Door door)
    {
        Undo.RecordObject(this, "Add door destination");
        Destination = door;
        EditorUtility.SetDirty(this);
    }

    public Door Get(int day)
    {
        Door door = Destination;
        if (RuledDestinations.Count == 0) return door;

        foreach ((DayCompareGroup compare, SerializedDictionary<Door, float> value) in RuledDestinations)
        {
            if (!compare.IsValid(day)) continue;

            Door result = value.RandomElementByWeight(e => e.Value).Key;
            door = result != null ? result : Destination;
        }

        return door;
    }

    private void OnDrawGizmosSelected()
    {
        if (Destination == null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position+Vector3.up, new Vector3(1,2,1));
            return;
        }
        Gizmos.DrawLine(transform.position, Destination.anchor.position);

        Gizmos.color = Color.gray;

        foreach (var keyValuePair in RuledDestinations)
        {
            foreach (var valuePair in keyValuePair.Value)
            {
                Gizmos.DrawLine(transform.position, valuePair.Key.anchor.position);
            }
        }
    }
}