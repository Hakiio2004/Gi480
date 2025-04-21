using UnityEngine;
using System;

public class EnemyScript : MonoBehaviour
{
    public event Action OnEnemyDestoryed;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            OnEnemyDestoryed?.Invoke();
            Destroy(gameObject);
        }
    }
}
