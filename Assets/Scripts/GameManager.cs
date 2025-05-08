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
    [SerializeField] private GameObject enemyPrefab;

    [Header("Enemy Settings")]
    [SerializeField] private int enemyCount = 20; // เกิดทั้งหมด 20 ตัว
    [SerializeField] private float spawnInterval = 5f; // เวลาห่างกันระหว่างแต่ละตัว

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

        StartCoroutine(SpawnEnemies());
    }

    void RestartGame()
    {
        _gameStarted = false;
        arSession.Reset();
        _planeManager.enabled = true;
    }

    void SpawnEnemy()
    {
        if (_planeManager.trackables.count == 0) return;

        List<ARPlane> planes = new List<ARPlane>();
        foreach (var plane in _planeManager.trackables)
        {
            planes.Add(plane);
        }

        var randomPlane = planes[Random.Range(0, planes.Count)];
        var randomPlanPosition = GetRandomPosition(randomPlane);

        var enemy = Instantiate(enemyPrefab, randomPlanPosition, Quaternion.identity);
        _spawnedEnemies.Add(enemy);

        var enemyScript = enemy.GetComponentInChildren<EnemyScript>();
        if (enemyScript != null)
        {
            enemyScript.OnEnemyDestoryed += AddScore;
        }
    }

    Vector3 GetRandomPosition(ARPlane plane)
    {
        var center = plane.center;
        var size = plane.size * 0.5f;
        var randomX = Random.Range(center.x, size.x);
        var randomZ = Random.Range(center.z, size.y);

        return new Vector3(center.x + randomX, center.y, center.z + randomZ);
    }

    IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void AddScore()
    {
        _score++;
        uiManager.UpdateScore(_score);
    }
}