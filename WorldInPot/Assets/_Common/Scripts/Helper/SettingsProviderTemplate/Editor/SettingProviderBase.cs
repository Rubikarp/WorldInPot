using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

public class SettingProviderBase<T> : SettingsProvider where T : ScriptableObject
{
    protected static Editor editor;
    public SettingProviderBase(string path, SettingsScope scope) : base(path, scope) { }

    /* Implement this line in all children
    [SettingsProvider] public static SettingsProvider GetSettingsProvider() => CreateProviderForProjectSettings();
    */

    public static SettingsProvider CreateProviderForProjectSettings()
    {
        SettingProviderBase<T> setProvidBase = new SettingProviderBase<T>("_WorldGame/" + typeof(T).Name, SettingsScope.Project);
        setProvidBase.guiHandler = OnProviderGUI;

        return setProvidBase;
    }

    public static void OnProviderGUI(string context)
    {
        T setProvidData = Resources.Load(typeof(T).Name) as T;
        if (setProvidData is null)
        {
            setProvidData = CreateSettingsAsset();
        }
        if (!editor)
        {
            Editor.CreateCachedEditor(setProvidData, null, ref editor);
        }
        editor.OnInspectorGUI();
    }

    public static T CreateSettingsAsset()
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
        Debug.Log(path);
        var rms = ScriptableObject.CreateInstance<T>();
        AssetDatabase.CreateAsset(rms, path);
        //
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        return rms;
    }

    /// <summary> Name is without the extension (.cs) </summary>
    /// <param name="scripName"></param>
    /// <returns></returns>
    public static string FindScriptFolder(string scripName, bool relative = true)
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
}
#endif