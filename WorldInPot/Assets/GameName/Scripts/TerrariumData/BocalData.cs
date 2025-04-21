using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "BocalData", menuName = "Terrarium/BocalData")]
public class BocalData : TerrariumDataBase
{
    [field: SerializeField]
    public string bocalName { get; private set; }

    public override string DisplayName => bocalName;

    public Mesh bocalMesh;
    public Sprite bocalSprite;

    [Button("Test Bocal")]
    private void TestBocal()
    {
        if (TerrariumBuilder.Instance != null)
        {
            TerrariumBuilder.Instance.SelectBocal(this);
        }
        else
        {
            Debug.LogWarning("No TerrariumBuilder instance found in scene");
        }
    }
}
