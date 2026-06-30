using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Serialization;

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


    public SerializedDictionary<DayComparationGroup, SerializedDictionary<Door, float>> RuledDestinations = new();

    public Door Get() => Get(GameManager.Instance.DayStatemachine.currentDay);

    public Door Get(int day)
    {
        Door door = Destination;
        if (RuledDestinations.Count == 0) return door;

        foreach ((DayComparationGroup compare, SerializedDictionary<Door, float> value) in RuledDestinations)
        {
            if (!compare.IsValid(day)) continue;

            Door result = value.RandomElementByWeight(e => e.Value).Key;
            door = result != null ? result : Destination;
        }

        return door;
    }

    private void OnDrawGizmos()
    {
        if(Destination == null) return;
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