using UnityEngine;
using UnityEngine.UI;

public class SelectedObjectUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform uiElement;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Camera mainCamera;

    [Header("Settings")]
    [SerializeField] private Vector2 screenOffset = new Vector2(0, 50f);
    [SerializeField] private float smoothSpeed = 10f;

    private Vector3 targetPosition;

    private void Start()
    {
        mainCamera = Camera.main;

        GameHandler.Instance.onSelectionChange += OnSelectionChanged;
        uiElement.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (GameHandler.Instance != null)
        {
            GameHandler.Instance.onSelectionChange -= OnSelectionChanged;
        }
    }

    private void OnSelectionChanged(GameObject selectedObject)
    {
        uiElement.gameObject.SetActive(selectedObject != null);
    }

    private void LateUpdate()
    {
        if (GameHandler.Instance.SelectedObject == null) return;

        Vector3 screenPos = mainCamera.WorldToScreenPoint(GameHandler.Instance.SelectedObject.transform.position);
        
        Vector2 canvasPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPos,
            canvas.worldCamera,
            out canvasPos
        );

        uiElement.anchoredPosition = canvasPos + screenOffset;
    }
} 