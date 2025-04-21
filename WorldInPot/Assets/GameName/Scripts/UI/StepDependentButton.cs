using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class StepDependentButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private ETerrariumStep requiredStep;

    private void Awake()
    {
        TerrariumBuilder.Instance.onProgressChanged += UpdateInteractability;
        UpdateInteractability(TerrariumBuilder.Instance.creationProgress);
    }
    private void OnEnable()
    {
        UpdateInteractability(TerrariumBuilder.Instance.creationProgress);
    }

    private void OnDestroy()
    {
        if (TerrariumBuilder.Instance != null)
            TerrariumBuilder.Instance.onProgressChanged -= UpdateInteractability;
    }

    private void UpdateInteractability(ETerrariumStep currentStep)
    {
        if (button == null) return;
        button.interactable = (int)currentStep >= (int)requiredStep;
    }
} 