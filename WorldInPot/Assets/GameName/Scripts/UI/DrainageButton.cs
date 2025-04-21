using UnityEngine;
using UnityEngine.UI;

public class DrainageButton : LayerButton
{
    [SerializeField] private DrainageData drainageData;

    protected override void Awake()
    {
        base.Awake();
        if (iconImage != null && drainageData != null)
        {
            iconImage.sprite = drainageData.drainageIcon;
        }
        if (nameText != null && drainageData != null)
        {
            nameText.text = drainageData.DisplayName;
        }
    }

    protected override void OnButtonClicked()
    {
        TerrariumBuilder.Instance.SelectDrainage(drainageData);
    }
} 