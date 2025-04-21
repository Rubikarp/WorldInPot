using UnityEngine;
using UnityEngine.UI;

public class SubstratShapeButton : LayerButton
{
    [SerializeField] private ESubstratShape substratShape;
    [SerializeField] private Sprite shapeIcon;

    protected override void Awake()
    {
        base.Awake();
        if (iconImage != null && shapeIcon != null)
        {
            iconImage.sprite = shapeIcon;
        }
    }

    protected override void OnButtonClicked()
    {
        TerrariumBuilder.Instance.SelectSubstratShape(substratShape);
    }
} 