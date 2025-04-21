using NUnit.Framework;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using System.Collections;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    [Header("References")]

    [Header("AR Components")]
    [SerializeField] private ARSession arSession;
    [SerializeField] private ARPlaneManager _planeManager;
    
    [SerializeField] private UIManager uiManager;
    [SerializeField] private  GameObject enemyPrefab;

    [Header("Enemy Settings")]
    [SerializeField] private int enemyCount = 2;
    [SerializeField] private float spawnRate = 2f;
    [SerializeField] private float deSpawnRate = 4f;

    private List<GameObject> _spawnedEnemies = new List<GameObject>();
    private int _score = 0;

    private bool _gameStarted;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
        print("Restarted!");
        _gameStarted = false;
        arSession.Reset();
        _planeManager.enabled = true;
    }

    void SpawnEnemy()
    {
        if (_planeManager.trackables.count == 0) return;
        List<ARPlane>planes = new List<ARPlane>();
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
            print("Run event");
            enemyScript.OnEnemyDestoryed += AddScore;
        }


        StartCoroutine(SpawnEnemies());
    }

    Vector3 GetRandomPosition(ARPlane plane)
    {
       var center = plane.center;
        var size = plane.size * 0.5f;
        var randomX = Random.Range(center.x, size.x);
        var randomZ = Random.Range(randomX, center.z);
        
        return new Vector3(center.x + randomX , center.y,center.z + randomZ);
    }

    IEnumerator SpawnEnemies()
    {
        while (_gameStarted)
        {
            if (_spawnedEnemies.Count < enemyCount)
            {
                SpawnEnemy();
            }
            yield return new WaitForSeconds(spawnRate);
        }
    }
    IEnumerator DespawnEnemies()
    {
        yield return new WaitForSeconds(deSpawnRate);
        if (_spawnedEnemies.Contains(enemyPrefab))
        {
            _spawnedEnemies.Remove(enemyPrefab);
            Destroy(enemyPrefab );
        }
    }
    void AddScore()
    {
        _score++;
        uiManager.UpdateScore(_score);
    }
}