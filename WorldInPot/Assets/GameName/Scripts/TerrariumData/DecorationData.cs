using UnityEngine;
using NaughtyAttributes;
#if UNITY_EDITOR
using UnityEditor;
#endif

//Terrarium structureType
public enum EDecorationType
{
    Rock,
    Root,
    Toys,
    Bark,
}

[CreateAssetMenu(fileName = "DecorationData", menuName = "Terrarium/DecorationData")]
public class DecorationData : TerrariumDataBase, IPlacableObject
{
    [field: SerializeField]
    public string decorationName { get; private set; }

    public override string DisplayName => decorationName;

    public GameObject decorationPrefab;
    public EDecorationType decorationType;
    public Sprite structureIcon;
    public Mesh decorationMesh;

    public GameObject Prefab => decorationPrefab;
    public Mesh PreviewMesh => decorationMesh;
    public string Name => decorationName;

    [Button("Test Decoration")]
    private void TestDecoration()
    {
        if (TerrariumBuilder.Instance != null)
        {
            Vector3 spawnPosition = TerrariumBuilder.Instance.transform.position + Vector3.up * 3f;
            TerrariumBuilder.Instance.AddDecoration(this, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("No TerrariumBuilder instance found in scene");
        }
    }
}
