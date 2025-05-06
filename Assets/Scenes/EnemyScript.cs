using UnityEngine;
using System;

public class EnemyScript : MonoBehaviour
{
    public event Action OnEnemyDestoryed;

    [Header("Enemy Settings")]
    public float health = 10f;

    private bool isDestroyed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Bullet bullet = other.GetComponent<Bullet>(); // ตรวจสอบว่าเป็น Bullet และดึงข้อมูลดาเมจจาก Bullet
            if (bullet != null)
            {
                // ทำลายกระสุนที่ชน
                Destroy(other.gameObject); 

                // เรียก TakeDamage หลังจากทำลายกระสุนแล้ว
                TakeDamage(bullet.damage);
            }
        }
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
            OnEnemyDestoryed?.Invoke(); // แจ้ง GameManager เพื่อเพิ่มคะแนน
        }

        Destroy(gameObject); // ลบศัตรูออกจากฉาก
    }
}
