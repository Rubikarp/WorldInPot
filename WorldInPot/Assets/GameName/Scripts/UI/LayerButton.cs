using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class LayerButton : MonoBehaviour
{
    [SerializeField] protected Button button;
    [SerializeField] protected Image iconImage;
    [SerializeField] protected TextMeshProUGUI nameText;

    protected virtual void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();
        
        if (iconImage == null)
            iconImage = GetComponent<Image>();

        if (nameText == null)
            nameText = GetComponentInChildren<TextMeshProUGUI>();
    }

    protected virtual void OnEnable()
    {
        button.onClick.AddListener(OnButtonClicked);
    }

    protected virtual void OnDisable()
    {
        button.onClick.RemoveListener(OnButtonClicked);
    }

    protected abstract void OnButtonClicked();
} 