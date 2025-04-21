using UnityEngine;
using UnityEngine.UI;

public class TerrariumValidator : MonoBehaviour
{
    public Button validateButton;
    public TerrariumBuilder terrariumBuilder;

    private void Update()
    {
        validateButton.interactable = terrariumBuilder.creationProgress >= ETerrariumStep.VegetationAndDecoration;
    }
}
