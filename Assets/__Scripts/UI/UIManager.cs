using Player;
using UnityEngine;
using UnityEngine.Assertions;
using UnityTemplateProjects;

public class UIManager : Singleton<UIManager>
{
    #region Fields

    [SerializeField] private MainMenuUI _mainMenu;
    [SerializeField] private InterfaceUI _interface;
    [SerializeField] private GameObject _damageFX;
    [SerializeField] private SpawnCounter _spawnCounter;
    [SerializeField] private GameOverUI _gameOverUI;

    #endregion

    #region Properties

    public SpawnCounter SpawnCounter => _spawnCounter;
    public GameOverUI GameOverUI => _gameOverUI;

    #endregion

    #region Methods

    // Start is called before the first frame update
    protected override void OnAwake()
    {
        Assert.IsNotNull(_mainMenu);
        Assert.IsNotNull(_interface);
        Assert.IsNotNull(_damageFX);
        Assert.IsNotNull(_spawnCounter);
        Assert.IsNotNull(_gameOverUI);
    }

    private void Start()
    {
        GameManager.S.Player
            .GetComponent<Character>()
            .OnDamage += ShowDamageFX;

        GameManager.S.OnGameStart += ShowSpawningCounter;
        GameManager.S.OnGameStart += ShowInterface;
        GameManager.S.OnGameOver += ShowGameOverScreen;

        GameOverUI.OnGameOverUIDissappear += () =>
        {
            _mainMenu.ShowMenu();
        };
    }

    private void ShowGameOverScreen()
    {
        _gameOverUI.gameObject.SetActive(true);
    }

    public void ShowInterface()
    {
        _interface.gameObject.SetActive(true);
    }

    public void HideInterface()
    {
        _interface.gameObject.SetActive(false);
    }

    public void ShowDamageFX()
    {
        _damageFX.SetActive(true);
    }

    public void ShowSpawningCounter()
    {
        _spawnCounter.gameObject.SetActive(true);
    }

    #endregion
}