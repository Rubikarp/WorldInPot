using UnityEngine;

//Terrarium structureType
public enum EStructureType
{
    Rock,
    Root,
    Toys,
    Bark,
}

[CreateAssetMenu(fileName = "StructureData", menuName = "Terrarium/StructureData")]
public class StructureData : ScriptableObject
{
    [field: SerializeField]
    public string structureName { get; private set; }

    public GameObject structurePrefab;
    public EStructureType structureType;

    public Sprite structureIcon;
}
