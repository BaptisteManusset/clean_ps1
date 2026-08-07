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
    public Door Destination;

    public Door Door;


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
            Gizmos.DrawCube(transform.position + Vector3.up, new Vector3(1, 2, 1));
            return;
        }

        Color currentColor = Door.IsLocked ? Color.green : Color.red;

        DrawPath(transform, Destination.transform, currentColor);

        foreach (KeyValuePair<DayCompareGroup, SerializedDictionary<Door, float>> keyValuePair in RuledDestinations)
        {
            foreach (KeyValuePair<Door, float> valuePair in keyValuePair.Value)
            {
                DrawPath(transform, valuePair.Key.anchor, Color.gray);
            }
        }
    }

    private static void DrawPath(Transform a_origin, Transform a_destination, Color currentColor)
    {
        Handles.DrawBezier(
            a_origin.position, a_destination.position,
            a_origin.position + Vector3.up * 5,
            a_destination.position + Vector3.up * 5,
            currentColor, null, 5f);
    }
}