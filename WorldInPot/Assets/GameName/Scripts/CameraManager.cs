using UnityEngine;
using Unity.Cinemachine;
using NaughtyAttributes;

public class CameraManager : SingletonMono<CameraManager>
{
    private GameHandler gameHandler;

    [Header("Client")]
    public CinemachineCamera cameraClient;

    [Header("Terrarium")]
    public CinemachineCamera cameraTerrarium;
    public CinemachineOrbitalFollow cameraOrbitalFollow;
    public Transform terrariumHolder;

    [Header("UI")]
    public Canvas terrariumUI;
    public Canvas clientUI;

    [Header("Parameter")]
    [SerializeField] private float scrollSensibility = 10f;
    [SerializeField] private Vector2 dragSensibility = new Vector2(0.1f, 0.1f);

    protected override void Awake()
    {
        base.Awake();
        gameHandler = GameHandler.Instance;
    }
    private void Update()
    {
        if (!gameHandler.InTerrariumMode) return;

        //If not right click, return
        if (!Input.GetMouseButton(1)) return;

        terrariumHolder.RotateAround(terrariumHolder.position, Vector3.up, -Input.mousePositionDelta.x * dragSensibility.x * Time.deltaTime);

        cameraOrbitalFollow.VerticalAxis.Value += Input.mousePositionDelta.y * dragSensibility.y * Time.deltaTime;
        cameraOrbitalFollow.VerticalAxis.Value = cameraOrbitalFollow.VerticalAxis.GetClampedValue();
        cameraOrbitalFollow.VerticalAxis.TrackValueChange();

        //Zoom Camera on scroll
        if (Input.mouseScrollDelta.magnitude != 0)
        {
            cameraOrbitalFollow.RadialAxis.Value += Input.mouseScrollDelta.y * scrollSensibility * Time.deltaTime;
            cameraOrbitalFollow.RadialAxis.Value = cameraOrbitalFollow.RadialAxis.GetClampedValue();
        }

    }

    public void FocusOnTerrarium()
    {
        cameraTerrarium.gameObject.SetActive(true);
        cameraClient.Priority = 0;
        cameraTerrarium.Priority = 10;

        terrariumUI.gameObject.SetActive(true);
        clientUI.gameObject.SetActive(false);
    }
    public void FocusOnClient()
    {
        cameraClient.gameObject.SetActive(true);
        cameraClient.Priority = 10;
        cameraTerrarium.Priority = 0;

        terrariumUI.gameObject.SetActive(false);
        clientUI.gameObject.SetActive(true);
    }
}
