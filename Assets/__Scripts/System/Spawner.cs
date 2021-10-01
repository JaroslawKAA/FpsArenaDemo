using System.Collections;
using System.Collections.Generic;
using System.Linq;
using __Scripts;
using Player;
using UnityEngine;
using UnityEngine.Assertions;

public class Spawner : Singleton<Spawner>
{
    [SerializeField] private Transform healBoxSpawnPoint;
    [SerializeField] private Transform[] enemySpawnPoints;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject healBoxPrefab;
    [SerializeField] private float nextSpawnDelay = 1f;

    private int _wave = 1;
    private List<Character> _enemies;
    private GameObject _healBoxInstance;

    public int Wave => _wave;

    protected override void OnAwake()
    {
        Assert.IsNotNull(healBoxSpawnPoint);
        Assert.IsNotNull(enemySpawnPoints);
        Assert.IsTrue(enemySpawnPoints.Length > 0);
        Assert.IsNotNull(enemyPrefab);
        Assert.IsNotNull(healBoxPrefab);
    }

    private void Start()
    {
        _enemies = new List<Character>();

        UIManager.S.SpawnCounter.OnCountingFinish += () =>
        {
            StartCoroutine(SpawnEnemies());
        };
        
        GameManager.S.OnNextWave += SpawnHealBox;

        GameManager.S.OnGameOver += () =>
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
                Destroy(enemy);
            _enemies = new List<Character>();
            _wave = 1;
            Destroy(_healBoxInstance);
        };
    }

    private void SpawnHealBox()
    {
        if (_healBoxInstance == null && Wave != 1)
        {
            _healBoxInstance = Instantiate(healBoxPrefab, healBoxSpawnPoint.transform.position, Quaternion.identity);
        }
    }

    private IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < _wave; i++)
        {
            if (GameManager.S.GameState != GameState.Play)
                break;
            
            Transform spawnPoint = enemySpawnPoints.GetRandom();

            GameObject obj = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            Character character = obj.GetComponent<Character>();
            character.OnDeath += () =>
            {
                if (!AreEnemiesAlive() && _enemies.Count == Wave)
                {
                    _wave++;
                    GameManager.S.Invoke_OnNextWave();
                    UIManager.S.ShowSpawningCounter();
                    
                    // Reset list
                    _enemies = new List<Character>();
                }
            };
            _enemies.Add(character);

            yield return new WaitForSeconds(nextSpawnDelay);
        }
    }

    private bool AreEnemiesAlive()
    {
        return _enemies.Any(e =>
        {
            if (e != null && e.TryGetComponent(out Character ch))
            {
                return ch.Alive;
            }
            return false;
        });
    }
}
