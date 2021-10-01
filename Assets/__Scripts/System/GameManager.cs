using System;
using UnityEngine;
using UnityEngine.Assertions;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private CameraStateMachine _camera;
    [SerializeField] private GameObject _showLevelCameraPivot;
    [SerializeField] private Transform _playerSpawnPoint;
    
    private GameState _state;
    
    public PlayerController Player => _player;

    public CameraStateMachine Camera => _camera;

    public GameObject ShowLevelCameraPivot => _showLevelCameraPivot;

    public Transform PlayerSpawnPoint => _playerSpawnPoint;

    public GameState GameState
    {
        get => _state;
        set
        {
            _state = value;
        }
    }
    
    protected override void OnAwake()
    {
        base.OnAwake();
        Assert.IsNotNull(Player);
        Assert.IsNotNull(Camera);
        Assert.IsNotNull(ShowLevelCameraPivot);
        Assert.IsNotNull(PlayerSpawnPoint);
        
        // Start game state
        _state = GameState.ShowLevel;

        OnGameStart += () =>
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            GameState = GameState.Play;
        };

        UIManager.S.GameOverUI.OnGameOverUIDissappear += () =>
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            GameState = GameState.ShowLevel;
        };
    }

    #region Events

    public event Action OnGameStart;

    public void Invoke_OnGameStart()
    {
        OnGameStart?.Invoke();
    }

    public event Action OnNextWave;

    public void Invoke_OnNextWave()
    {
        OnNextWave?.Invoke();
    }

    public event Action OnGameOver;

    public void Invoke_OnGameOver()
    {
        OnGameOver?.Invoke();
    }

    #endregion
}
