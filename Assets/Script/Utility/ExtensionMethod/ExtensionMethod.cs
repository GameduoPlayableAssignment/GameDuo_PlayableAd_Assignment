using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

// public static class Debug
// {
//     public static void Log(object message, StackTraceLogType stackTraceLogType = StackTraceLogType.ScriptOnly)
//     {
//         Application.SetStackTraceLogType(LogType.Log, stackTraceLogType);
//         DebugX.Log(message);
//     }
// }

public static partial class ExtensionMethod
{
    public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
    {
        foreach (T item in enumeration)
        {
            action(item);
        }
    }

    public static Dictionary<TKey, TValue> Shuffle<TKey, TValue>(this Dictionary<TKey, TValue> source)
    {
        Random r = new Random();
        return source.OrderBy(x => r.Next()).ToDictionary(item => item.Key, item => item.Value);
    }
    
    public static Transform Search(this Transform target, string name)
    {
        if (target.name == name)
            return target;
        
        for (int i = 0; i < target.childCount; ++i)
        {
            var result = Search(target.GetChild(i), name);
            if (result != null) return result;
        }
        return null;
    }

    public static bool CheckLayer(this int layer, int _layer)
    {
        return (layer & _layer) != 0;
    }
    public static bool CheckLayer(this int layer, string name)
    {
        return layer == (layer | 1 << LayerMask.NameToLayer(name));
    }
    public static void SetSprite(this Image image, Sprite sprite)
    {
        image.sprite = sprite;
        image.SetNativeSize();
    }
}