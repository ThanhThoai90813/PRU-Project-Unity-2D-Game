using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHealBar : MonoBehaviour
{
    public Slider healthSlider;
    public TMP_Text healthBarText;

    private Damageable enemyDamageable;

    private void Awake()
    {
        // Tắt UI mặc định khi khởi tạo
        gameObject.SetActive(false);
    }

    // Hàm khởi tạo thanh máu khi được bật
    public void Initialize(GameObject enemy)
    {
        if (enemy == null)
        {
            Debug.LogError("No enemy found with tag 'GruzMother'.");
            return;
        }

        enemyDamageable = enemy.GetComponent<Damageable>();
        if (enemyDamageable == null)
        {
            Debug.LogError("Enemy does not have Damageable component.");
            return;
        }

        // Cập nhật UI ban đầu
        healthSlider.value = CalculateSliderPercentage(enemyDamageable.Health, enemyDamageable.MaxHealth);
        healthBarText.text = "HP " + enemyDamageable.Health + " / " + enemyDamageable.MaxHealth;

        // Đăng ký sự kiện
        enemyDamageable.healthChanged.AddListener(OnPlayerHealthChanged);
    }

    private void OnDisable()
    {
        if (enemyDamageable != null)
        {
            enemyDamageable.healthChanged.RemoveListener(OnPlayerHealthChanged);
        }
    }

    private float CalculateSliderPercentage(float currentHealth, float maxHealth)
    {
        return currentHealth / maxHealth;
    }

    private void OnPlayerHealthChanged(int newHealth, int maxHealth)
    {
        healthSlider.value = CalculateSliderPercentage(newHealth, maxHealth);
        healthBarText.text = "HP " + newHealth + " / " + maxHealth;
    }
}