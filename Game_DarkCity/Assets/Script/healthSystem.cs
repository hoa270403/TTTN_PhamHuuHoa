using Spine.Unity.Examples;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int maxHealth = 100; // Máu tối đa
    public int currentHealth; // Máu hiện tại
    public HealthBar healthBar; // Tham chiếu tới thanh máu

    void Start()
    {
        // Khởi tạo máu và cập nhật thanh máu
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        // Giảm máu
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        // Cập nhật thanh máu
        UpdateHealthBar();

        // Kiểm tra nếu máu = 0
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth, maxHealth);
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} has died!");
        GetComponent<SpineboyBeginnerModel>()?.Die(); // Báo cho Model xử lý chết
    }
}
