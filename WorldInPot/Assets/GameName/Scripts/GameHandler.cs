using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public enum EEditMode
{
    None = 0,
    Placing = 1,
    Moving = 2,
    Rotating = 3
}

public class GameHandler : SingletonMono<GameHandler>
{
    [Header("References")]
    [SerializeField] private TerrariumBuilder terrariumBuilder;
    [SerializeField] private CameraManager cameraManager;

    [Header("Parameters")]
    [SerializeField, Range(0.1f, 1f)] private float rotationSensitivity = 0.5f;
    [SerializeField] private LayerMask placementLayerMask;
    [SerializeField] private LayerMask selectionLayer;
    [SerializeField] private GameObject previewObject;
    [SerializeField] private MeshFilter previewMesh;

    [Header("State")]
    [field: SerializeField] public bool InTerrariumMode { get; private set; } = true;
    [field: SerializeField] public EEditMode CurrentEditMode { get; private set; } = EEditMode.None;
    [field: SerializeField] public GameObject SelectedObject { get; private set; }

    private Vector3 moveStartPosition;
    private Vector3 rotationStartPosition;
    private Quaternion rotationStartRotation;
    private bool isRotating = false;
    private IPlacableObject currentPlacable;

    public UnityAction<EEditMode> onModeChange;
    public UnityAction<GameObject> onSelectionChange;

    private void OnValidate()
    {
        ValidateReferences();
    }

    private void ValidateReferences()
    {
        if (terrariumBuilder == null)
        {
            Debug.LogError("TerrariumBuilder is not assigned in GameHandler!");
        }
        if (cameraManager == null)
        {
            Debug.LogError("CameraManager is not assigned in GameHandler!");
        }
        if (previewObject == null)
        {
            Debug.LogError("PreviewObject is not assigned in GameHandler!");
        }
        if (previewMesh == null)
        {
            Debug.LogError("PreviewMesh is not assigned in GameHandler!");
        }
    }

    [Button("Toggle Game Phase")]
    public void ToogleGamePhase() => ChangeGamePhase(!InTerrariumMode);

    public void ChangeGamePhase(bool inTerrarium)
    {
        if (terrariumBuilder == null || cameraManager == null) return;

        InTerrariumMode = inTerrarium;
        if (InTerrariumMode)
        {
            terrariumBuilder.ClearAllElements();
            cameraManager.FocusOnTerrarium();
        }
        else
        {
            cameraManager.FocusOnClient();
        }
    }

    [Button] public void MoveToNoneMode() => SetEditMode(EEditMode.None);
    [Button] public void MoveToPlacingMode() => SetEditMode(EEditMode.Placing);
    [Button] public void MoveToMovingMode() => SetEditMode(EEditMode.Moving);
    [Button] public void MoveToRotatingMode() => SetEditMode(EEditMode.Rotating);

    public void SetEditMode(EEditMode newMode)
    {
        if (CurrentEditMode == newMode) return;

        // Reset position if leaving moving mode
        if (CurrentEditMode == EEditMode.Moving && SelectedObject != null)
        {
            SelectedObject.transform.position = moveStartPosition;
        }

        // Reset rotation if leaving rotating mode
        if (CurrentEditMode == EEditMode.Rotating && SelectedObject != null)
        {
            SelectedObject.transform.rotation = rotationStartRotation;
        }

        // Clean up preview if leaving placing mode
        if (CurrentEditMode == EEditMode.Placing)
        {
            if (previewObject != null)
            {
                previewObject.SetActive(false);
            }
            currentPlacable = null;
        }

        // Store start position when entering moving mode
        if (newMode == EEditMode.Moving && SelectedObject != null)
        {
            moveStartPosition = SelectedObject.transform.position;
        }

        // Store start rotation when entering rotating mode
        if (newMode == EEditMode.Rotating && SelectedObject != null)
        {
            rotationStartRotation = SelectedObject.transform.rotation;
        }

        CurrentEditMode = newMode;
        onModeChange?.Invoke(CurrentEditMode);
    }

    private void Start()
    {
        ValidateReferences();
        ChangeGamePhase(InTerrariumMode);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ChangeGamePhase(!InTerrariumMode);
        }

        if (InTerrariumMode)
        {
            HandleEditModeInput();
        }
        else
        {
            HandleClientModeInput();
        }
    }

    private bool IsPointerOverUI()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }

    private void HandleEditModeInput()
    {
        // Handle mode switching with number keys
        if (Input.GetKeyDown(KeyCode.Escape)) SetEditMode(EEditMode.None);
        if (Input.GetKeyDown(KeyCode.Alpha1)) SetEditMode(EEditMode.Placing);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SetEditMode(EEditMode.Moving);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SetEditMode(EEditMode.Rotating);

        switch (CurrentEditMode)
        {
            case EEditMode.Placing:
                HandlePlacingMode();
                break;
            case EEditMode.Moving:
                HandleMovingMode();
                break;
            case EEditMode.Rotating:
                HandleRotatingMode();
                break;
            case EEditMode.None:
                HandleNoneMode();
                break;
        }
    }

    private void HandlePlacingMode()
    {
        if (previewObject == null || IsPointerOverUI()) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, placementLayerMask) && 
            Vector3.Dot(Vector3.up, hit.normal) > Mathf.Cos(80 * Mathf.Deg2Rad))
        {
            previewObject.transform.position = hit.point;
            previewObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

            if (Input.GetMouseButtonDown(0))
            {
                ValidatePlacement(hit);
            }
        }
    }

    private void ValidatePlacement(RaycastHit hit)
    {
        if (currentPlacable == null || terrariumBuilder == null) return;

        if (currentPlacable is VegetationData vegetationData)
        {
            terrariumBuilder.AddVegetation(vegetationData, hit.point, previewObject.transform.rotation);
        }
        else if (currentPlacable is DecorationData decorationData)
        {
            terrariumBuilder.AddDecoration(decorationData, hit.point, previewObject.transform.rotation);
        }

        previewObject.SetActive(false);
        SetEditMode(EEditMode.None);
    }

    private void HandleMovingMode()
    {
        if (SelectedObject == null || IsPointerOverUI()) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, placementLayerMask) && 
            Vector3.Dot(Vector3.up, hit.normal) > Mathf.Cos(80 * Mathf.Deg2Rad))
        {
            SelectedObject.transform.position = hit.point;
            SelectedObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

            if (Input.GetMouseButtonDown(0))
            {
                moveStartPosition = SelectedObject.transform.position;
                SetEditMode(EEditMode.None);
            }
        }
    }

    private void HandleRotatingMode()
    {
        if (SelectedObject == null || IsPointerOverUI()) return;

        if (Input.GetMouseButtonDown(0))
        {
            isRotating = true;
            rotationStartPosition = Input.mousePosition;
            rotationStartRotation = SelectedObject.transform.rotation;
        }
        else if (Input.GetMouseButton(0) && isRotating)
        {
            Vector3 currentMousePosition = Input.mousePosition;
            Vector3 mouseDelta = currentMousePosition - rotationStartPosition;

            Vector3 cameraRight = Camera.main.transform.right;
            Vector3 cameraUp = Camera.main.transform.up;

            Quaternion rotationX = Quaternion.AngleAxis(-mouseDelta.y * rotationSensitivity, cameraRight);
            Quaternion rotationY = Quaternion.AngleAxis(mouseDelta.x * rotationSensitivity, cameraUp);

            SelectedObject.transform.rotation = rotationStartRotation * rotationY * rotationX;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            rotationStartRotation = SelectedObject.transform.rotation;
            isRotating = false;
            SetEditMode(EEditMode.None);
        }
    }

    private void HandleNoneMode()
    {
        if (Input.GetMouseButtonDown(0) && !IsPointerOverUI())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f, selectionLayer))
            {
                GameObject clickedObject = hit.collider.gameObject;

                if (terrariumBuilder != null && 
                    (terrariumBuilder.currentVegetationObject.Contains(clickedObject) ||
                     terrariumBuilder.currentStructureObject.Contains(clickedObject)))
                {
                    SetSelectedObject(clickedObject);
                }
                else
                {
                    DeselectObject();
                }
            }
            else
            {
                DeselectObject();
            }
        }
    }

    public void DestroyObject()
    {
        if (SelectedObject != null && terrariumBuilder != null)
        {
            terrariumBuilder.RemoveObject(SelectedObject);
            DeselectObject();
        }
    }

    public void DeselectObject() => SetSelectedObject(null);

    private void SetSelectedObject(GameObject obj)
    {
        if (SelectedObject != obj)
        {
            SelectedObject = obj;
            onSelectionChange?.Invoke(SelectedObject);
        }
    }

    private void HandleClientModeInput()
    {
        //It's mainly UI
    }

    public void StartPlacing(VegetationData vegetationData) => StartPlacing(vegetationData as IPlacableObject);
    public void StartPlacing(DecorationData decorationData) => StartPlacing(decorationData as IPlacableObject);

    private void StartPlacing(IPlacableObject placableObject)
    {
        if (placableObject == null || previewObject == null || previewMesh == null) return;

        DeselectObject();
        currentPlacable = placableObject;

        previewObject.SetActive(true);
        previewMesh.mesh = placableObject.PreviewMesh;

        SetEditMode(EEditMode.Placing);
    }

    [Button("Validate Terrarium")]
    public void ValidateTerrarium()
    {
        if (ClientManager.Instance != null)
        {
            ChangeGamePhase(false);
            ClientManager.Instance.CompleteCurrentClient();
        }
    }
}
