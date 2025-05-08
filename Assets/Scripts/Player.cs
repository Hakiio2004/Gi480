using UnityEngine;

public class Player : MonoBehaviour
 
{
    public float health = 100f;
    private UIManager uiManager;
    private GameManager gameManager;

    private int score = 0; // สมมติว่าคุณมีระบบเก็บ score แล้ว

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        uiManager = FindObjectOfType<UIManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Player took damage: " + damage);

        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player died!");

        if (uiManager != null && gameManager != null)
        {
            int finalScore = gameManager.GetScore();
            uiManager.ShowGameOver(finalScore);
            gameManager.GameOver(); // ✅ หยุดทุกอย่าง
        }
    }
}

