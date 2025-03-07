using UnityEngine;
using UnityEngine.UI;  // Để sử dụng UI Button

public class UpgradeButton : MonoBehaviour
{
    public Button upgradeButton; // Tham chiếu đến nút Upgrade
    public PlayerShoot2 playerShoot; // Tham chiếu đến PlayerShoot2 (nơi có hàm UpgradeBullet)

    void Start()
    {
        // Kiểm tra nếu nút và PlayerShoot2 đã được gán
        if (upgradeButton != null && playerShoot != null)
        {
            // Đăng ký sự kiện cho nút
            upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
        }
    }

    // Hàm gọi khi nút Upgrade được nhấn
    void OnUpgradeButtonClicked()
    {
        // Gọi hàm UpgradeBullet từ PlayerShoot2
        playerShoot.UpgradeBullet();
    }
}
