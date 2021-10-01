using System;
using Player;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerController : MonoBehaviour
{
    #region Fields

    [SerializeField] private Transform cameraPositionTarget;
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 7f;
    [SerializeField] private float jumpSpeed = 10f;
    [SerializeField] private float gravityDownForce = -60f;
    [SerializeField] private Transform lookYBone;

    private CharacterController _characterController;
    private WeaponController _weaponController;
    private float _cameraVerticalAngle;
    private float _characterVelocityY;
    private Camera _playerCamera;
    private MovementState _movementState = MovementState.Run;
    private Character _character;

    /// <summary>
    /// Bone witch work when camera is looking down or up
    /// </summary>
    private float _startLookYBone;

    private float _currentLookYBone;

    private Vector3 _characterVelocity;

    #endregion

    #region Properties

    public Vector3 CharacterVelocity => _characterVelocity;

    public Transform CameraPositionTarget => cameraPositionTarget;

    public float RunSpeed => runSpeed;

    #endregion

    #region Private Methods

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _weaponController = GetComponent<WeaponController>();
        _character = GetComponent<Character>();

        Assert.IsNotNull(cameraPositionTarget);
        Assert.IsNotNull(lookYBone);

        _startLookYBone = lookYBone.rotation.eulerAngles.x;
        _currentLookYBone = _startLookYBone;
        
        GetComponent<Character>().OnDeath += () =>
        {
            GameManager.S.GameState = GameState.Death;
            transform.position = GameManager.S.PlayerSpawnPoint.position;
            transform.rotation = GameManager.S.PlayerSpawnPoint.rotation;
            GameManager.S.Invoke_OnGameOver();
        };

        GameManager.S.OnGameStart += () =>
        {
            _character.Heal(_character.HealthMax);
            _weaponController.Reload(true);
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.S.GameState != GameState.Play ||
            GameManager.S.Camera.state != CameraState.FollowPlayer)
            return;

        HandleYRotation();
        HandleYLook();
        HandleCharacterMovement();
        HandleRunWalk();
        HandleShoot();
        HandleReloadWeapon();
    }

    private void HandleReloadWeapon()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            _weaponController.Reload();
            OnReload?.Invoke();
        }
    }

    private void HandleShoot()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _weaponController.Shoot();
            OnShoot?.Invoke();
        }
    }

    private void HandleRunWalk()
    {
        // Invert moving state on pressing shift
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.LeftShift))
        {
            _movementState = _movementState == MovementState.Run ? MovementState.Walk : MovementState.Run;
        }

        // Invert moving state on pressing capsLock
        if (Input.GetKeyDown(KeyCode.CapsLock))
        {
            _movementState = _movementState == MovementState.Run ? MovementState.Walk : MovementState.Run;
        }
    }

    /// <summary>
    /// Rotate player character spine, with camera.
    /// </summary>
    private void HandleYLook()
    {
        float lookY = Input.GetAxisRaw("Mouse Y");

        _currentLookYBone -= lookY * GameManager.S.Camera.mouseSensitivity * Time.deltaTime;
        _currentLookYBone = Mathf.Clamp(
            _currentLookYBone,
            _startLookYBone - GameManager.S.Camera.verticalRotationLimit,
            _startLookYBone + GameManager.S.Camera.verticalRotationLimit
        );

        lookYBone.localRotation = Quaternion.Euler(
            new Vector3(_currentLookYBone,
                -90,
                -22.293f));
    }

    private void HandleCharacterMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        _characterVelocity = Vector3.right * moveX *
                             (_movementState == MovementState.Run ? runSpeed : walkSpeed)
                             + Vector3.forward * moveZ *
                             (_movementState == MovementState.Run ? runSpeed : walkSpeed);

        if (_characterController.isGrounded)
        {
            _characterVelocityY = 0f;
            // Jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _characterVelocityY = jumpSpeed;
            }
        }

        // Apply gravity to the velocity
        _characterVelocityY += gravityDownForce * Time.deltaTime;

        // Apply Y velocity to move vector
        _characterVelocity.y = _characterVelocityY;

        // Move
        _characterController.Move(transform.rotation * _characterVelocity * Time.deltaTime);
    }

    private void HandleYRotation()
    {
        float lookX = Input.GetAxisRaw("Mouse X");

        transform.Rotate(
            new Vector3(0f, lookX * rotationSpeed * Time.deltaTime, 0f),
            Space.Self);
    }

    #endregion

    #region Events

    public event Action OnShoot;
    public event Action OnReload;

    #endregion
}