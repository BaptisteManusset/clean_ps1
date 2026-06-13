using System;
using System.Collections.Generic;
using System.Linq;

public static class ListUtils
{
    private static readonly Random Rng = new();

    //source: https://stackoverflow.com/questions/273313/randomize-a-listt
    public static List<T> Shuffle<T>(this List<T> a_list)
    {
        return a_list.OrderBy(_ => Rng.Next()).ToList();
    }

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> a_source)
    {
        return a_source.OrderBy(_ => Rng.Next());
    }

    public static object GetRandomElement<T>(this List<T> a_list)
    {
        if (a_list.Count == 0) return null;
        
        return a_list[UnityEngine.Random.Range(0, a_list.Count)];
    }
}