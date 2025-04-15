using UnityEngine;

//Terrarium plant types
public enum EVegetationType
{
    Succulent,
    Moss,
    Fern,
    Cactus,
}

[CreateAssetMenu(fileName = "VegetationData", menuName = "Terrarium/VegetationData")]
public class VegetationData : ScriptableObject
{
    [field: SerializeField]
    public string vegetationName { get; private set; }

    public GameObject vegetationPrefab;
    public EVegetationType vegetationType;
    public Sprite vegetationIcon;
}
