using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 50;                  // Máu tối đa của kẻ địch
    private int currentHealth;                 // Máu hiện tại
    //public GameObject healthBarPrefab;         // Prefab thanh máu
    private HealthBarEnemy healthBar;          // Tham chiếu tới script HealthBarEnemy
    public GameObject lootPrefab;              // Prefab vật phẩm rơi ra khi kẻ địch chết

    void Start()
    {
        Debug.Log("EnemyHealth script is running!");
        // Khởi tạo máu
        currentHealth = maxHealth;

        // Tạo thanh máu
        /*if (healthBarPrefab != null)
        {
            GameObject healthBarObj = Instantiate(healthBarPrefab, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);

            // Kiểm tra xem đối tượng thanh máu đã được tạo hay chưa
            if (healthBarObj != null)
            {
                Debug.Log("Health bar created successfully.");
            }
            else
            {
                Debug.LogError("Failed to create health bar.");
            }

            healthBar = healthBarObj.GetComponent<HealthBarEnemy>();

            if (healthBar != null)
            {
                healthBar.target = this.transform; // Gắn kẻ địch làm mục tiêu của thanh máu
                healthBar.SetHealth(currentHealth, maxHealth);
            }
            else
            {
                Debug.LogError("HealthBarEnemy script is missing on the health bar prefab.");
            }
        }
        else
        {
            Debug.LogError("Health bar prefab is not assigned in the Inspector.");
        }*/
    }

    public void TakeDamage(int damage)
    {
        // Giảm máu
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        // Cập nhật thanh máu
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth, maxHealth);
        }

        // Kiểm tra nếu máu = 0
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} has been defeated!");

        // Rơi vật phẩm nếu có
        if (lootPrefab != null)
        {
            Instantiate(lootPrefab, transform.position, Quaternion.identity);
        }

        // Hủy kẻ địch
        Destroy(gameObject);
    }
}
