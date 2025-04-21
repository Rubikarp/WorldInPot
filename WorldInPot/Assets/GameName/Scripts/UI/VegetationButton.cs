using UnityEngine;
using UnityEngine.UI;

public class VegetationButton : LayerButton
{
    [SerializeField] private VegetationData vegetationData;

    protected override void Awake()
    {
        base.Awake();
        if (iconImage != null && vegetationData != null)
        {
            iconImage.sprite = vegetationData.vegetationIcon;
        }
        if (nameText != null && vegetationData != null)
        {
            nameText.text = vegetationData.DisplayName;
        }
    }

    protected override void OnButtonClicked()
    {
        GameHandler.Instance.StartPlacing(vegetationData);
    }
} 