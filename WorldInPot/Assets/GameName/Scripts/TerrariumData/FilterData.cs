using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "FilterData", menuName = "Terrarium/FilterData")]
public class FilterData : TerrariumDataBase
{
    [field: SerializeField]
    public string filterName { get; private set; }

    public override string DisplayName => filterName;

    public Material filterMaterial;
    public Sprite filterIcon;

    [Button("Test Filter")]
    private void TestFilter()
    {
        if (TerrariumBuilder.Instance != null)
        {
            TerrariumBuilder.Instance.SelectFilter(this);
        }
        else
        {
            Debug.LogWarning("No TerrariumBuilder instance found in scene");
        }
    }
}
