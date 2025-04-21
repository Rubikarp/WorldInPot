using UnityEngine;
using UnityEngine.UI;

public class SubstratButton : LayerButton
{
    [SerializeField] private SubstratData substratData;
    protected override void Awake() => LoadButtonData();

    protected override void LoadButtonData()
    {
        if (iconImage != null && substratData != null)
        {
            iconImage.sprite = substratData.substratIcon;
            iconImage.enabled = true;
        }
        if (nameText != null && substratData != null)
        {
            nameText.text = substratData.DisplayName;
        }
    }

    protected override void OnButtonClicked()
    {
        TerrariumBuilder.Instance.SelectSubstrat(substratData);
    }
} 