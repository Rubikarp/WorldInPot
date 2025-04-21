using UnityEngine;
using NaughtyAttributes;

//Terrarium plant types
public enum EVegetationType
{
    Succulent,
    Moss,
    Fern,
    Cactus,
}

[CreateAssetMenu(fileName = "VegetationData", menuName = "Terrarium/VegetationData")]
public class VegetationData : TerrariumDataBase, IPlacableObject
{
    [field: SerializeField]
    public string vegetationName { get; private set; }

    public override string DisplayName => vegetationName;

    public GameObject vegetationPrefab;
    public EVegetationType vegetationType;
    public Sprite vegetationIcon;
    public Mesh vegetationMesh;

    public GameObject Prefab => vegetationPrefab;
    public Mesh PreviewMesh => vegetationMesh;
    public string Name => vegetationName;

    [Button("Test Vegetation")]
    private void TestVegetation()
    {
        if (TerrariumBuilder.Instance != null)
        {
            Vector3 spawnPosition = TerrariumBuilder.Instance.transform.position + Vector3.up * 3f;
            TerrariumBuilder.Instance.AddVegetation(this, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("No TerrariumBuilder instance found in scene");
        }
    }
}
