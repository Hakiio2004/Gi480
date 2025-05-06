using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 10f; // ดาเมจที่กระสุนจะสร้าง

    private void OnCollisionEnter(Collision collision)
    {
        EnemyScript enemy = collision.gameObject.GetComponent<EnemyScript>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
