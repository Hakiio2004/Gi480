using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    
    private void Awake()
    {
        if (slider == null)
        {
            Debug.LogError("Slider ไม่ได้ถูกกำหนด!");
        }
    }

    
    public void SetMaxHealth(float health)
    {
        if (slider != null)
        {
            slider.maxValue = health;
            slider.value = health;
        }
    }

    
    public void SetHealth(float health)
    {
        if (slider != null)
        {
            slider.value = health;
        }
    }

   
    public void OnSliderValueChanged()
    {
        
        Debug.Log("สุขภาพถูกเปลี่ยนเป็น: " + slider.value);
    }
}
