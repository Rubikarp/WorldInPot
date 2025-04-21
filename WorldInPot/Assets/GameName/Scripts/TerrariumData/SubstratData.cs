using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "SubstratData", menuName = "Terrarium/SubstratData")]
public class SubstratData : TerrariumDataBase
{
    [field:SerializeField]
    public string substratName { get; private set; }

    public override string DisplayName => substratName;

    public Material substratMaterial;
    public Sprite substratIcon;

    [Button("Test Substrat")]
    private void TestSubstrat()
    {
        if (TerrariumBuilder.Instance != null)
        {
            TerrariumBuilder.Instance.SelectSubstrat(this);
        }
        else
        {
            Debug.LogWarning("No TerrariumBuilder instance found in scene");
        }
    }
}
