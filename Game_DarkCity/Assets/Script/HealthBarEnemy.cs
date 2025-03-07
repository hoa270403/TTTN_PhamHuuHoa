using UnityEngine;

public class HealthBarEnemy : MonoBehaviour
{
    public Transform healthBarFill; // Phần Fill của thanh máu
    public Transform target;        // Kẻ địch mà thanh máu theo dõi
    public Vector3 offset = new Vector3(0, 1.5f, 0); // Khoảng cách thanh máu trên đầu

    public void SetHealth(float currentHealth, float maxHealth)
    {
        if (healthBarFill == null)
        {
            Debug.LogWarning("HealthBarFill is not assigned.");
            return;
        }

        // Đảm bảo tỷ lệ máu nằm trong khoảng [0, 1]
        float healthPercentage = Mathf.Clamp01(currentHealth / maxHealth);

        // Cập nhật kích thước thanh máu
        healthBarFill.localScale = new Vector3(healthPercentage, 1, 1);

        Debug.Log($"Health updated: {healthPercentage * 100}%");
    }

    void Update()
    {
        // Theo dõi vị trí kẻ địch
        if (target != null)
        {
            transform.position = target.position + offset;
        }
        else
        {
            // Nếu target bị hủy, hủy luôn thanh máu
            Destroy(gameObject);
        }
    }
}
