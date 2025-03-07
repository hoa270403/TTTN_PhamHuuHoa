using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBarUI : MonoBehaviour
{
    public Image healthBarFill; // Thanh máu (phần Fill) // Nhân vật mà thanh máu theo dõi
    public Vector3 offset = new Vector3(0, 1.5f, 0); // Khoảng cách thanh máu trên đầu nhân vật

    public void SetHealth(float currentHealth, float maxHealth)
    {
        // Tính tỷ lệ máu còn lại
        float healthPercentage = currentHealth / maxHealth;

        // Cập nhật fillAmount của thanh máu
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = healthPercentage; // Giá trị từ 0 đến 1
        }
    }

}
