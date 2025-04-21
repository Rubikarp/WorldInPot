using UnityEngine;
using UnityEngine.UI;

public class FilterButton : LayerButton
{
    [SerializeField] private FilterData filterData;
    protected override void Awake() => LoadButtonData();

    protected override void LoadButtonData()
    {
        if (iconImage != null && filterData != null)
        {
            iconImage.sprite = filterData.filterIcon;
            iconImage.enabled = true;
        }
        if (nameText != null && filterData != null)
        {
            nameText.text = filterData.DisplayName;
        }
    }

    protected override void OnButtonClicked()
    {
        TerrariumBuilder.Instance.SelectFilter(filterData);
    }
} 