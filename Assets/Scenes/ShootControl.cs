using UnityEngine;

public class ShootControl : MonoBehaviour
{
    [Header("Refernces")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform cameraTransform;

    [Header("Settings")]
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float bulletLifeTime = 3f; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIManager.OnUIShootButton += Shoot;
    }

    // Update is called once per frame
    void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, cameraTransform.position,Quaternion.identity);
        var rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(cameraTransform.forward * bulletSpeed, ForceMode.Impulse);
        Destroy(bullet, bulletLifeTime);
    }
}
