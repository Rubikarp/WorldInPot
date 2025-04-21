using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "DrainageData", menuName = "Terrarium/DrainageData")]
public class DrainageData : TerrariumDataBase
{
    [field: SerializeField]
    public string drainageName { get; private set; }

    public override string DisplayName => drainageName;

    public Material drainageMaterial;
    public Sprite drainageIcon;

    [Button("Test Drainage")]
    private void TestDrainage()
    {
        if (TerrariumBuilder.Instance != null)
        {
            TerrariumBuilder.Instance.SelectDrainage(this);
        }
        else
        {
            Debug.LogWarning("No TerrariumBuilder instance found in scene");
        }
    }
}
