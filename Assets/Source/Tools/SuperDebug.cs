using System;
using System.Collections.Generic;
using UnityEngine;

public class SuperDebug : Idebug
{
    private static Color _defaultColor = Color.white;
    private static Dictionary<Type, Color> _colors = new Dictionary<Type, Color>();

    public static void RegisterColor(Type type, Color color)
    {
        _colors[type] = color;
    }
    
    /// <summary>
    /// don't forget to turn this method back into static , testing the injection for now 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="type"></param>
    public void Log(object message, Type type)
    {
        Color color;
        if (_colors.TryGetValue(type, out color))
        {
            Debug.Log("<color=" + ColorUtility.ToHtmlStringRGBA(color) + ">" + message + "</color>");
        }
        else
        {
            Debug.Log("<color=" + ColorUtility.ToHtmlStringRGBA(_defaultColor) + ">" + message + "</color>");
        }
    }
}
