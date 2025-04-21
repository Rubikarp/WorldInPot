using System;
using UnityEngine;
using UnityEngine.UI;

public class SubstratShapeButton : LayerButton
{
    [SerializeField] private ESubstratShape substratShape;
    [SerializeField] private Sprite shapeIcon;
    protected override void Awake() => LoadButtonData();

    protected override void LoadButtonData()
    {
        if (iconImage != null && shapeIcon != null)
        {
            iconImage.sprite = shapeIcon;
            iconImage.enabled = true;
        }
        if (nameText != null)
        {
            nameText.text = Enum.GetName(typeof(ESubstratShape), substratShape);
        }
    }

    protected override void OnButtonClicked()
    {
        TerrariumBuilder.Instance.SelectSubstratShape(substratShape);
    }
} 