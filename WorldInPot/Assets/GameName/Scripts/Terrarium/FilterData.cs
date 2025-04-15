using UnityEngine;

[CreateAssetMenu(fileName = "FilterData", menuName = "Terrarium/FilterData")]
public class FilterData : ScriptableObject
{
    [field: SerializeField]
    public string filterName { get; private set; }

    public Material filterMaterial;
    public float layerHeight;

    public Sprite filterIcon;
}
