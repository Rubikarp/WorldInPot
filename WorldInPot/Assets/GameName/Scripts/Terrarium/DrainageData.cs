using UnityEngine;

[CreateAssetMenu(fileName = "DrainageData", menuName = "Terrarium/DrainageData")]
public class DrainageData : ScriptableObject
{
    [field: SerializeField]
    public string drainageName { get; private set; }

    public Material drainageMaterial;
    public float layerHeight;


    public Sprite drainageIcon;
}
