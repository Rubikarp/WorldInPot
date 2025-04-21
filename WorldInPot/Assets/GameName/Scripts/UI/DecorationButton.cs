using UnityEngine;
using UnityEngine.UI;

public class DecorationButton : LayerButton
{
    [SerializeField] private DecorationData decorationData;
    protected override void Awake() => LoadButtonData();

    protected override void LoadButtonData()
    {
        if (iconImage != null && decorationData != null)
        {
            iconImage.sprite = decorationData.structureIcon;
            iconImage.enabled = true;
        }
        if (nameText != null && decorationData != null)
        {
            nameText.text = decorationData.DisplayName;
        }
    }

    protected override void OnButtonClicked()
    {
        GameHandler.Instance.StartPlacing(decorationData);
    }
} 