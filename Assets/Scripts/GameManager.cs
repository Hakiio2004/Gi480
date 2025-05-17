using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using Random = UnityEngine.Random;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ARSession arSession;
    [SerializeField] private ARPlaneManager _planeManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameObject virusSporePrefab; 
    [SerializeField] private GameObject otherEnemyPrefab; 

    [Header("Enemy Settings")]
    [SerializeField] private int virusSporeCount = 5; 
    [SerializeField] private float virusSporeSpawnInterval = 5f; 
    [SerializeField] private int otherEnemyCount = 15; 
    [SerializeField] private float otherEnemySpawnInterval = 3f; 

    private List<GameObject> _spawnedEnemies = new List<GameObject>();
    private int _score = 0;
    private bool _gameStarted;

    void Start()
    {
        UIManager.OnUIStartButton += StartGame;
        UIManager.OnUIRestartButton += RestartGame;
    }

    void StartGame()
    {
        if (_gameStarted) return;
        _gameStarted = true;

        _planeManager.enabled = false;
        foreach (var plane in _planeManager.trackables)
        {
            var meshVisual = GetComponent<ARFaceMeshVisualizer>();
            if (meshVisual) meshVisual.enabled = false;

            var lineVisual = GetComponent<LineRenderer>();
            if (lineVisual) lineVisual.enabled = false;
        }

        StartCoroutine(SpawnVirusSpores());
        StartCoroutine(SpawnOtherEnemies());
    }

    void RestartGame()
    {
        _gameStarted = false;
        arSession.Reset();
        _planeManager.enabled = true;

        FindObjectOfType<Player>()?.ResetHealth();
        foreach (var enemy in _spawnedEnemies)
        {
            if (enemy != null)
                Destroy(enemy);
        }
        _spawnedEnemies.Clear();

        _score = 0;
        uiManager.UpdateScore(_score);

        isGameOver = false;
    }

    void SpawnEnemy(GameObject prefabToSpawn)
    {
        if (_planeManager.trackables.count == 0) return;

        List<ARPlane> planes = new List<ARPlane>();
        foreach (var plane in _planeManager.trackables)
        {
            planes.Add(plane);
        }

        var randomPlane = planes[Random.Range(0, planes.Count)];
        var randomPlanPosition = GetRandomPosition(randomPlane);

        var enemy = Instantiate(prefabToSpawn, randomPlanPosition, Quaternion.identity);

        var enemyScript = enemy.GetComponentInChildren<EnemyScript>();
        if (enemyScript != null)
        {
            enemyScript.OnEnemyDestoryed += AddScore;
        }
        _spawnedEnemies.Add(enemy);
    }

    Vector3 GetRandomPosition(ARPlane plane)
    {
        var center = plane.center;
        var size = plane.size * 0.5f;
        var randomX = Random.Range(center.x, size.x);
        var randomZ = Random.Range(center.z, size.y);

        return new Vector3(center.x + randomX, center.y, center.z + randomZ);
    }

    IEnumerator SpawnVirusSpores()
    {
        for (int i = 0; i < virusSporeCount; i++)
        {
            SpawnEnemy(virusSporePrefab);
            yield return new WaitForSeconds(virusSporeSpawnInterval);
        }
    }

    IEnumerator SpawnOtherEnemies()
    {
        for (int i = 0; i < otherEnemyCount; i++)
        {
            SpawnEnemy(otherEnemyPrefab);
            yield return new WaitForSeconds(otherEnemySpawnInterval);
        }
    }

    public void AddScore(int points)
    {
        _score += points;
        uiManager.UpdateScore(_score);
    }

    public int GetScore()
    {
        return _score;
    }
    private bool isGameOver = false;

    public void GameOver()
    {
        isGameOver = true;

        StopAllCoroutines();

        foreach (var enemy in _spawnedEnemies)
        {
            if (enemy != null)
                Destroy(enemy);
        }

        _spawnedEnemies.Clear();
    }
    public bool IsGameOver()
    {
        return isGameOver;
    }
    public bool IsGameStarted()
    {
        return _gameStarted;
    }
}