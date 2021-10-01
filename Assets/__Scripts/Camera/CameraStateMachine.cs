using System;
using Player;
using UnityEngine;
using UnityTemplateProjects.Camera;

public class CameraStateMachine : MonoBehaviour
{
    #region Fields

    [Header("Settings")] [SerializeField] 
    internal float mouseSensitivity = 1f;
    [SerializeField] internal float verticalRotationLimit = 60f;
    [SerializeField] internal float distanceFromPlayer = 2f;
    [SerializeField] internal float offsetX = 0f;
    [SerializeField] internal LayerMask physicMask;

    protected internal CameraState state;
    private CameraStateBase _currentState;
    private ShowLevelCameraState _showLevelCameraState;
    private PlayerCameraState _playerCameraState;
    private ChangeStateCameraState _changeStateCameraState;

    #endregion

    public void ChangeState(CameraState state, bool withChangingStateBefore)
    {
        CurrentState = state switch
        {
            CameraState.FollowPlayer => withChangingStateBefore
                ? ChangeStateInit(CameraState.FollowPlayer)
                : _playerCameraState,
            CameraState.ShowLevel => withChangingStateBefore
                ? ChangeStateInit(CameraState.ShowLevel)
                : _showLevelCameraState,
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
        };
    }

    private CameraStateBase ChangeStateInit(CameraState nextState)
    {
        Transform target = nextState switch
        {
            CameraState.FollowPlayer => _playerCameraState.GetTarget(),
            CameraState.ShowLevel => _showLevelCameraState.GetTarget(),
            _ => throw new ArgumentOutOfRangeException(nameof(nextState), nextState, null)
        };

        transform.parent = null;
        _changeStateCameraState.SetTarget(target);
        _changeStateCameraState.SetNextState(nextState);
        return _changeStateCameraState;
    }

    private CameraStateBase CurrentState
    {
        get => _currentState;
        set
        {
            _currentState?.OnExit();
            _currentState = value;
            _currentState.OnEnter();
        }
    }

    private void Awake()
    {
        _showLevelCameraState = new ShowLevelCameraState(this);
        _playerCameraState = new PlayerCameraState(this);
        _changeStateCameraState = new ChangeStateCameraState(this);

        // Always we started with showing level
        CurrentState = _showLevelCameraState;
        
        GameManager.S.OnGameStart += () =>
        {
            ChangeState(CameraState.FollowPlayer, true);
        };
        
        GameManager.S.Player.GetComponent<Character>().OnDeath += () =>
        {
            ChangeState(CameraState.ShowLevel, true);
        };

    }

    void Start()
    {
        state = CameraState.ShowLevel;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentState.OnUpdate();
    }
}