using UnityEngine;
using UnityEngine.UI;

public class SubstratButton : LayerButton
{
    [SerializeField] private SubstratData substratData;

    protected override void Awake()
    {
        base.Awake();
        if (iconImage != null && substratData != null)
        {
            iconImage.sprite = substratData.substratIcon;
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