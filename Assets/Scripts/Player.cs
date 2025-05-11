using UnityEngine;

public class Player : MonoBehaviour
{
    public float health = 100f;
    private UIManager uiManager;
    private GameManager gameManager;
    private HealthBar healthBar;

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        gameManager = FindObjectOfType<GameManager>();
        healthBar = FindObjectOfType<HealthBar>();

        if (healthBar != null)
        {
            healthBar.SetMaxHealth(health);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Player took damage: " + damage);

        if (healthBar != null)
        {
            healthBar.SetHealth(health);
        }

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
