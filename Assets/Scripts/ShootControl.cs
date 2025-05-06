using UnityEngine;
using System.Collections;

public class ShootControl : MonoBehaviour
{
    [Header("Refernces")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform cameraTransform;

    [Header("Settings")]
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float bulletLifeTime = 3f;
    [SerializeField] private int maxAmmo = 5; // จำนวนกระสุนสูงสุดที่สามารถยิงได้
    [SerializeField] private float reloadTime = 2f; // เวลาที่ใช้ในการรีโหลด

    private int currentAmmo;
    private bool isReloading = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentAmmo = maxAmmo;
        UIManager.OnUIShootButton += Shoot;
    }

    // Update is called once per frame
    void Update()
    {
        // รีโหลดอัตโนมัติหากกระสุนหมด
        if (currentAmmo == 0 && !isReloading)
        {
            StartCoroutine(Reload());
        }
    }

    // ฟังก์ชันยิงกระสุน
    void Shoot()
    {
        if (isReloading || currentAmmo <= 0)
            return;

        var bullet = Instantiate(bulletPrefab, cameraTransform.position, Quaternion.identity);
        var rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(cameraTransform.forward * bulletSpeed, ForceMode.Impulse);
        Destroy(bullet, bulletLifeTime);

        currentAmmo--;
    }

    // ฟังก์ชันรีโหลด
    private IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
        Debug.Log("Reload complete!");
    }
}
