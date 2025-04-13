using System;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Abstract class for making reload-proof singletons out of ScriptableObjects
/// Returns the asset created on the editor, or null if there is none
/// Based on https://www.youtube.com/watch?v=VBA1QCoEAX4
/// </summary>
/// <typeparam name="T">Singleton type</typeparam>

public abstract class SingletonSCO<T> : ScriptableObject where T : ScriptableObject
{
    static T instance = null;
    public static T Instance
    {
        get
        {
            if (!instance)
            {
                instance = Resources.Load<T>(typeof(T).Name);
                if (!instance)
                {
#if UNITY_EDITOR
                    CreateAsset();
                    instance = Resources.Load<T>(typeof(T).Name);
#endif
                }

                if (!instance)
                    Debug.LogError($"SingletonSCO: {typeof(T).Name} not found in Resources. " +
                        $"Make sure it is properly placed in a Resources folder, and its name is the same as its script.");
            }

            return instance;
        }
    }
#if UNITY_EDITOR

    private static T CreateAsset()
    {
        var className = typeof(T).Name;
        var path = FindScriptFolder(className, true);
        if (!AssetDatabase.IsValidFolder(path + "Resources/"))
        {
            var folder = path.Remove(path.Length - 1);
            Debug.Log(folder);
            AssetDatabase.CreateFolder(folder, "Resources");
        }
        path += "Resources/" + className + ".asset";
        Debug.Log("Create asset at : " + path);
        var rms = ScriptableObject.CreateInstance<T>();
        AssetDatabase.CreateAsset(rms, path);
        //
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        return rms;
    }
    private static string FindScriptFolder(string scripName, bool relative = true)
    {
        string[] fullFilePath = System.IO.Directory.GetFiles(Application.dataPath, scripName + ".cs", System.IO.SearchOption.AllDirectories);
        if (fullFilePath.Length == 0)
        {
            //Can't find script (verify scripName)
            return null;
        }
        string path = fullFilePath[0].Replace(scripName + ".cs", "").Replace("\\", "/");
        if (relative)
        {
            //https://docs.microsoft.com/fr-fr/dotnet/api/system.string.split?view=net-6.0#system-string-split(system-string-system-stringsplitoptions)
            return "Assets" + path.Split(new string[] { "Assets" }, StringSplitOptions.None)[1];
        }
        else
        {
            return path;
        }
    }
#endif
}