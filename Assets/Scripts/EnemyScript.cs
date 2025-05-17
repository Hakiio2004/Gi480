using UnityEngine;
using System;

public class EnemyScript : MonoBehaviour
{
    public event Action<int> OnEnemyDestoryed;
    public event Action OnEnemyDied; // Event แจ้งเมื่อตาย (สำหรับการ Respawn)

    [Header("Enemy Settings")]
    public float health = 10f;
    public float moveSpeed = 3f;

    private Transform playerTransform;
    private bool isDestroyed = false;
    private GameManager gameManager;
    private int scoreValue = 1;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogWarning("Player not found in scene!");
        }
        gameManager = FindObjectOfType<GameManager>();

        if (gameObject.name.StartsWith("virus spore"))
        {
            scoreValue = 10;
        }
        else if (gameObject.name.StartsWith("virus_gale_GEO"))
        {
            scoreValue = 1;
        }
    }

    private void Update()
    {
        if (isDestroyed || playerTransform == null || (gameManager != null && gameManager.IsGameOver())) return;

        transform.LookAt(playerTransform);
        transform.position = Vector3.MoveTowards(
            transform.position,
            playerTransform.position,
            moveSpeed * Time.deltaTime
        );
    }
    public void TakeDamage(float amount)
    {
        if (isDestroyed) return;

        health -= amount;
        Debug.Log($"Enemy took damage: {amount}, remaining: {health}");

        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die(bool giveScore = true)
    {
        if (isDestroyed) return;
        isDestroyed = true;

        if (giveScore)
        {
            OnEnemyDestoryed?.Invoke(scoreValue);
        }

        // แจ้ง GameManager ว่าตายแล้ว (สำหรับการ Respawn ถ้าเป็น virus spore)
        if (gameObject.name.StartsWith("virus spore") && gameManager != null && gameManager.IsGameStarted())
        {
            OnEnemyDied?.Invoke();
            gameObject.SetActive(false); // ปิดการทำงานของ GameObject ชั่วคราว
        }
        else
        {
            Destroy(gameObject); // ทำลายมอนสเตอร์อื่นๆ ตามปกติ
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Bullet bullet = other.GetComponent<Bullet>();
            if (bullet != null)
            {
                Destroy(other.gameObject);
                TakeDamage(bullet.damage);
            }
        }
        else if (other.CompareTag("Player"))
        {
            DealDamageToPlayer();
        }
    }

    private void DealDamageToPlayer()
    {
        if (playerTransform != null)
        {
            Player playerHealth = playerTransform.GetComponent<Player>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(20f);
                Debug.Log("Enemy hit player for 20 damage!");

                Die(false);
            }
        }
    }
}