using UnityEngine;
using UnityEngine.UI;

public class BocalButton : LayerButton
{
    [SerializeField] private BocalData bocalData;

    protected override void Awake()
    {
        base.Awake();
        if (iconImage != null && bocalData != null)
        {
            iconImage.sprite = bocalData.bocalSprite;
        }
        if (nameText != null && bocalData != null)
        {
            nameText.text = bocalData.DisplayName;
        }
    }

    protected override void OnButtonClicked()
    {
        TerrariumBuilder.Instance.SelectBocal(bocalData);
    }
} 