#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Attribute to mark a class as a GameSettings.
/// Please note: Any class marked with this attribute must inherit from the SingletonSCO base class.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class GameSettingsAttribute : CallbackOrderAttribute
{
    public GameSettingsAttribute(Type target)
    {
        if (!typeof(SingletonSCO<>).IsAssignableFrom(target))
        {
            throw new ArgumentException("The class marked with GameSettingsAttribute must inherit from SingletonSCO.");
        }
    }
}
#endif