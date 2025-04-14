
using UnityEngine;
using UnityEngine.UI;

enum BarType { Health, Poise}
public class HealthbarUI : MonoBehaviour
{
    [SerializeField] BarType barType;
    private Entity entity;
    private CharacterStats stats;
    private RectTransform myTransform;

    private Slider slider;

    private void Start() 
    {
        entity = GetComponentInParent<Entity>();

        entity.OnFlipped += FlipUI;

        myTransform = GetComponent<RectTransform>();
        slider = GetComponentInChildren<Slider>();
        stats = GetComponentInParent<CharacterStats>();

        if(barType == BarType.Health)
        {
            stats.OnDamaged += UpdateHealthUI;
            stats.OnHealed += UpdateHealthUI;
            UpdateHealthUI();
        }
        else
        {
            stats.OnPoiseChanged += UpdatePoiseUI;
            UpdatePoiseUI();
        }
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = stats.health.GetValue();
        slider.value = stats.HP;
    }

    private void UpdatePoiseUI()
    {
        slider.maxValue = stats.poise.GetValue() * 5;
        slider.value = 100 - stats.poiseTracker;
    }

    private void FlipUI() => myTransform.Rotate(0, 180, 0);

    void OnDisable() 
    {
        entity.OnFlipped -= FlipUI;
        stats.OnDamaged -= barType == BarType.Health ? UpdateHealthUI : null;
        stats.OnHealed -= barType == BarType.Health ? UpdateHealthUI : null;
        stats.OnPoiseChanged -= barType == BarType.Poise ? UpdatePoiseUI : null;
    }
}
