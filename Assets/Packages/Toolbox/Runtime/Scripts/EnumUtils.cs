using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

public static class EnumUtils
{
    public static IEnumerable<T> GetValues<T>()
    {
        return Enum.GetValues(typeof(T)).Cast<T>();
    }

    public static string[] GetNames<T>()
    {
        return Enum.GetNames(typeof(T));
    }

    public static int GetCurrentIndex<T>(T a_value)
    {
        return Array.IndexOf(Enum.GetValues(a_value.GetType()), a_value);
    }

    public static int Count<T>() where T : Enum
    {
        return Enum.GetNames(typeof(T)).Length;
    }

    // This extension method is broken out so you can use a similar pattern with 
    // other MetaData elements in the future. This is your base method for each.
    public static T GetAttribute<T>(this Enum a_value) where T : Attribute {
        Type type = a_value.GetType();
        MemberInfo[] memberInfo = type.GetMember(a_value.ToString());
        object[] attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
        return attributes.Length > 0 
            ? (T)attributes[0]
            : null;
    }
    
    public static bool IsObsolete(this Enum a_value)
    {
        FieldInfo fieldInfo = a_value.GetType().GetField(a_value.ToString());
        ObsoleteAttribute[] attributes = (ObsoleteAttribute[])fieldInfo.GetCustomAttributes(typeof(ObsoleteAttribute), false);
        return attributes is { Length: > 0 };
    }

    public static T GetRandomEnumValue<T>() where T : Enum
    {
        Array values = Enum.GetValues(typeof(T));
        int index = UnityEngine.Random.Range(0, values.Length);
        return (T)values.GetValue(index);
    }
}
