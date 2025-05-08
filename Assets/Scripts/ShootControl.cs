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
    [SerializeField] private int maxAmmo = 5; // จำนวนกระสุนสูงสุด
    [SerializeField] private float reloadTime = 2f; // เวลาที่ใช้ในการรีโหลด

    private int currentAmmo;
    private bool isReloading = false;

    private UIManager uiManager;

    void OnEnable()
    {
        UIManager.OnUIShootButton += Shoot;
    }

    void OnDisable()
    {
        UIManager.OnUIShootButton -= Shoot;
    }

    void Start()
    {
        currentAmmo = maxAmmo;
        uiManager = FindObjectOfType<UIManager>();
        uiManager.UpdateBulletUI(currentAmmo); // แสดงกระสุนตอนเริ่ม
    }

    void Update()
    {
        // รีโหลดอัตโนมัติหากกระสุนหมด
        if (currentAmmo == 0 && !isReloading)
        {
            StartCoroutine(Reload());
        }
    }

    void Shoot()
    {
        if (isReloading || currentAmmo <= 0)
            return;

        Debug.Log("Shoot called");

        GameObject bullet = Instantiate(bulletPrefab, cameraTransform.position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(cameraTransform.forward * bulletSpeed, ForceMode.Impulse);
        Destroy(bullet, bulletLifeTime);

        currentAmmo--;
        uiManager.UpdateBulletUI(currentAmmo); // อัปเดต UI กระสุน
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
        uiManager.UpdateBulletUI(currentAmmo); // เติมกระสุนใน UI
        Debug.Log("Reload complete!");
    }
}
