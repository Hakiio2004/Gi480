using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    // ตรวจสอบว่า slider ได้รับการกำหนดหรือไม่
    private void Awake()
    {
        if (slider == null)
        {
            Debug.LogError("Slider ไม่ได้ถูกกำหนด!");
        }
    }

    // ตั้งค่าสุขภาพสูงสุดและกำหนดค่า slider
    public void SetMaxHealth(float health)
    {
        if (slider != null)
        {
            slider.maxValue = health;
            slider.value = health;
        }
    }

    // อัปเดตสุขภาพปัจจุบันบน slider
    public void SetHealth(float health)
    {
        if (slider != null)
        {
            slider.value = health;
        }
    }

    // เพิ่มฟังก์ชันสำหรับการฟังค่าการเปลี่ยนแปลงของ slider (เพื่อทดสอบ)
    public void OnSliderValueChanged()
    {
        // สามารถทำงานบางอย่างเมื่อค่าใน slider เปลี่ยน
        Debug.Log("สุขภาพถูกเปลี่ยนเป็น: " + slider.value);
    }
}
