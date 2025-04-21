using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public abstract class LayerButton : MonoBehaviour
{
    [SerializeField] protected Button button;
    [SerializeField] protected Image buttonImage;
    [SerializeField] protected Image iconImage;
    [SerializeField] protected TextMeshProUGUI nameText;

    protected virtual void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();
        
        if(buttonImage == null)
            buttonImage = GetComponent<Image>();

        if (iconImage == null)
            iconImage = GetComponentInChildren<Image>();

        if (nameText == null)
            nameText = GetComponentInChildren<TextMeshProUGUI>();

        LoadButtonData();
    }

    [Button]
    protected virtual void LoadButtonData()
    {
        // Base implementation does nothing
        // Derived classes should override this to load their specific data
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