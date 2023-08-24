using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLogic : MonoBehaviour, ISurfControllable
{
    public Text SpeedText; 
    [Header("Walk And Jump")] public float WalkSpeed = 80f;
    public float JumpForce = 40f;

    [Header("Physics Settings")] public int TickRate = 128;
    public Camera MainCam;

    [Header("Movement Config")] [SerializeField]
    public MovementConfig MovementConfig = new();

    private Rigidbody _rb;
    private Vector3 _startPos;
    private Quaternion _originalRot;
    private Vector3 _checkpointPos;

    private SurfController _surfController = new SurfController();

    public PlayerData PlayerData { get; } = new PlayerData();
    public InputData InputData { get; } = new();

    public CapsuleCollider CapsuleCollider { get; private set; }

    public Vector3 BaseVelocity { get; }
    public Camera FpsCamera => MainCam;

    private void Awake()
    {
        Application.targetFrameRate = 144;
        QualitySettings.vSyncCount = 1;

        Time.fixedDeltaTime = 1f / TickRate;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = true;
        _rb.freezeRotation = true;

        CapsuleCollider = gameObject.GetComponent<CapsuleCollider>();
        CapsuleCollider.isTrigger = true;
        
        PlayerData.Origin = transform.position;
        _startPos = transform.position;
        _checkpointPos = transform.position;
        _originalRot = transform.localRotation;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        InputData.CalculateMovement(MovementConfig);
        UpdateViewAngle();
        SpeedText.text = (10f * PlayerData.Velocity.magnitude).ToString("F0");
    }

    private void FixedUpdate()
    {
        if (InputData.ResetPressed) {
            ResetPosition();
        }

        if (InputData.GetCheckpoint)
        {
            CheckpointLoad();
        }
        if (InputData.SetCheckpoint)
        {
            SetCheckpoint();
        }

        var fixedDeltaTime = Time.fixedDeltaTime;
        _surfController.ProcessMovement(this, MovementConfig, fixedDeltaTime);

        ApplyPlayerMovement();
    }

    private void LateUpdate()
    {
        ApplyMouseMovement();
    }
    
    private void UpdateViewAngle()
    {
        var rot = PlayerData.ViewAngles + new Vector3(-InputData.MouseY, InputData.MouseX, 0f);
        rot.x = GameUtils.ClampAngle(rot.x, -85f, 85f);
        PlayerData.ViewAngles = rot;
    }

    private void ApplyPlayerMovement() {
        transform.position = PlayerData.Origin;
    }

    private void ApplyMouseMovement() {
        // Get the rotation you will be at next as a Quaternion
        var yQuaternion = Quaternion.AngleAxis(PlayerData.ViewAngles.x, Vector3.right);
        var xQuaternion = Quaternion.AngleAxis(PlayerData.ViewAngles.y, Vector3.up);

        // Rotate the rigidbody for horizontal move
        transform.localRotation = xQuaternion;

        // Rotate the attached camera for vertival move
        MainCam.transform.localRotation = yQuaternion;
    }

    public void ResetPosition()
    {
        PlayerData.Velocity = Vector3.zero;
        PlayerData.Origin = _startPos;
    }

    public void SetCheckpoint()
    {
        _checkpointPos = transform.position;
    }

    public void CheckpointLoad()
    {
        PlayerData.Velocity = Vector3.zero;
        PlayerData.Origin = _checkpointPos;
    }
}