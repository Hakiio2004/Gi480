using UnityEngine;

public class Player : MonoBehaviour
 
{
    public float health = 100f;

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
        // อาจทำ Game Over หรือเอฟเฟกต์จบเกมที่นี่
    }
}

