using UnityEngine;
using UnityEngine.UI;

public class FilterButton : LayerButton
{
    [SerializeField] private FilterData filterData;

    protected override void Awake()
    {
        base.Awake();
        if (iconImage != null && filterData != null)
        {
            iconImage.sprite = filterData.filterIcon;
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