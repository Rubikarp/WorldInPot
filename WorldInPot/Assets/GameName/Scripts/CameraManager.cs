using UnityEngine;
using Unity.Cinemachine;
using NaughtyAttributes;

public class CameraManager : SingletonMono<CameraManager>
{
    [Header("Terraium Holder")]
    public Transform terrariumHolder;

    [Header("Client")]
    public CinemachineCamera cameraClient;

    [Header("Terrarium")]
    public CinemachineCamera cameraTerrarium;
    public CinemachineOrbitalFollow cameraOrbitalFollow;

    [Header("UI")]
    public Canvas terrariumUI;
    public Canvas clientUI;

    [Header("Parameter")]
    [SerializeField] private float scrollSensibility = 10f;
    [SerializeField] private Vector2 dragSensibility = new Vector2(0.1f, 0.1f);
    [SerializeField] private bool inTerrariumMode = true;

    public bool InTerrariumMode
    {
        get => inTerrariumMode;
        set
        {
            inTerrariumMode = value;
            if (inTerrariumMode)
            {
                FocusOnTerrarium();
            }
            else
            {
                FocusOnClient();
            }
        }
    }

    void Start()
    {
        InTerrariumMode = inTerrariumMode;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            inTerrariumMode = !inTerrariumMode;
            if (inTerrariumMode)
            {
                FocusOnTerrarium();
            }
            else
            {
                FocusOnClient();
            }
        }
    }

    public void LateUpdate()
    {
        if (!InTerrariumMode) return;

        //Move Camera on drag
        if (Input.GetMouseButton(1))
        {
            terrariumHolder.RotateAround(terrariumHolder.position, Vector3.up, -Input.mousePositionDelta.x * dragSensibility.x * Time.deltaTime);

            cameraOrbitalFollow.VerticalAxis.Value += Input.mousePositionDelta.y * dragSensibility.y * Time.deltaTime;
            cameraOrbitalFollow.VerticalAxis.Value = cameraOrbitalFollow.VerticalAxis.GetClampedValue();
            cameraOrbitalFollow.VerticalAxis.TrackValueChange();
        }

        //Zoom Camera on scroll
        if (Input.mouseScrollDelta.magnitude != 0)
        {
            cameraOrbitalFollow.RadialAxis.Value += Input.mouseScrollDelta.y * scrollSensibility * Time.deltaTime;
            cameraOrbitalFollow.RadialAxis.Value = cameraOrbitalFollow.RadialAxis.GetClampedValue();
        }
    }

    [Button]
    public void FocusOnTerrarium()
    {
        cameraTerrarium.gameObject.SetActive(true);
        cameraClient.Priority = 0;
        cameraTerrarium.Priority = 10;

        terrariumUI.gameObject.SetActive(true);
        clientUI.gameObject.SetActive(false);
    }
    [Button]
    public void FocusOnClient()
    {
        cameraClient.gameObject.SetActive(true);
        cameraClient.Priority = 10;
        cameraTerrarium.Priority = 0;

        terrariumUI.gameObject.SetActive(false);
        clientUI.gameObject.SetActive(true);
    }
}
