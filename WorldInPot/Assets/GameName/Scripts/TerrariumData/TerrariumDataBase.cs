using UnityEngine;
using NaughtyAttributes;

#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class TerrariumDataBase : ScriptableObject
{
    public abstract string DisplayName { get; }

    [Button("Rename Asset")] private void RenameAsset()
    {
#if UNITY_EDITOR
        if (!string.IsNullOrEmpty(DisplayName))
        {
            name = DisplayName;
            string assetPath = AssetDatabase.GetAssetPath(this);
            if (!string.IsNullOrEmpty(assetPath))
            {
                // Get the current name of the script type
                var name = this.GetType().Name + "_" + DisplayName;
                AssetDatabase.RenameAsset(assetPath, name);
                AssetDatabase.SaveAssets();
            }
        }
#endif
    }
} 