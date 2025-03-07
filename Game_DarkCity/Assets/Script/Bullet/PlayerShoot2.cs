using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;  // Để sử dụng UI Image

public class PlayerShoot2 : MonoBehaviour
{
    public GameObject[] bulletPrefabs;  // Mảng các prefab viên đạn theo các cấp độ
    public Transform firePoint;         // Điểm bắn (nòng súng)
    public float fireRate = 0.2f;      // Thời gian giữa các lần bắn (giây)

    private float nextFireTime;         // Thời gian tiếp theo có thể bắn
    private int currentBulletLevel = 1; // Cấp độ đạn hiện tại (mặc định là cấp 1)

    public CoinCollector coinCollector; // Tham chiếu đến script CoinCollector

    public Image bulletImage;           // Hình ảnh viên đạn trên UI (Canvas)
    public Sprite[] bulletSprites;      // Mảng hình ảnh đạn theo các cấp độ

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
           
            return;
        }

        // Kiểm tra nhấn phím Fire1 để bắn
        if (Input.GetButton("Fire1"))
        {
            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;  // Thiết lập thời gian bắn tiếp theo
            }
        }

        // Kiểm tra khi ấn phím 'U' để nâng cấp đạn
        if (Input.GetKeyDown(KeyCode.U))
        {
            UpgradeBullet();
        }
    }

    public void Shoot()
    {
        // Tính toán hướng bắn từ vị trí con trỏ chuột
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 firePointPosition = firePoint.position;
        Vector2 direction = (mousePosition - firePointPosition).normalized;

        // Tạo viên đạn theo cấp độ hiện tại
        GameObject bulletObj = Instantiate(bulletPrefabs[currentBulletLevel - 1], firePoint.position, Quaternion.identity);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.SetDirection(direction);
        }

        Rigidbody2D rb = bulletObj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * bullet.speed;  // Cập nhật vận tốc cho viên đạn
        }
    }

    // Nâng cấp đạn khi ấn phím 'U'
    public void UpgradeBullet()
    {
        if (coinCollector.totalCoins >= 100)  // Kiểm tra nếu người chơi có đủ 100 xu
        {
            if (currentBulletLevel < bulletPrefabs.Length)  // Kiểm tra nếu cấp độ đạn chưa đạt tối đa
            {
                currentBulletLevel++;  // Tăng cấp độ đạn lên
                coinCollector.totalCoins -= 100;  // Trừ 100 xu khi nâng cấp
                coinCollector.UpdateCoinUI();  // Cập nhật lại UI số xu

                // Cập nhật hình ảnh viên đạn trên UI khi nâng cấp
                if (bulletSprites.Length >= currentBulletLevel)
                {
                    bulletImage.sprite = bulletSprites[currentBulletLevel - 1];  // Cập nhật hình ảnh viên đạn theo cấp độ
                }

                Debug.Log("Đạn đã được nâng cấp lên cấp " + currentBulletLevel);
            }
            else
            {
                Debug.Log("Đạn đã đạt cấp tối đa.");
            }
        }
        else
        {
            Debug.Log("Bạn không đủ xu để nâng cấp đạn. Cần 100 xu.");
        }
    }
}
