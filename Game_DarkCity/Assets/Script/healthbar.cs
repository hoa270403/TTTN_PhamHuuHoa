using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Transform healthBarFill; // Thanh máu (phần Fill)
    public Transform target; // Nhân vật mà thanh máu theo dõi
    public Vector3 offset = new Vector3(0, 1.5f, 0); // Khoảng cách thanh máu trên đầu nhân vật

    public void SetHealth(float currentHealth, float maxHealth)
    {
        // Tính tỷ lệ máu còn lại
        float healthPercentage = currentHealth / maxHealth;

        // Cập nhật kích thước thanh máu
        healthBarFill.localScale = new Vector3(healthPercentage, 1, 1);

        // Điều chỉnh vị trí để thanh máu rút từ phải qua trái
        float offsetX = (1 - healthPercentage) * -0.5f; // Dịch sang trái dựa trên kích thước
        healthBarFill.localPosition = new Vector3(offsetX, 0, 0);
    }

    void Update()
    {
        // Theo dõi vị trí nhân vật
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
}
