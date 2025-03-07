using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;  // Prefab viên đạn
    public Transform firePoint;      // Điểm bắn (nòng súng)
    public float bulletSpeed = 20f;  // Tốc độ viên đạn
    public float fireRate = 0.2f;    // Thời gian giữa các lần bắn (giây)

    private float nextFireTime;      // Thời gian tiếp theo có thể bắn

    void Update()
    {
        
        // Kiểm tra nếu giữ chuột trái
        if (Input.GetButton("Fire1"))
        {
            if (Time.time >= nextFireTime) // Kiểm tra nếu đến thời điểm bắn
            {
                Debug.Log("ban");
                Shoot();
                nextFireTime = Time.time + fireRate; // Thiết lập thời điểm bắn tiếp theo
            }
        }
    }

    void Shoot()
    {

        // Tính toán hướng bắn dựa trên vị trí con trỏ chuột
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 firePointPosition = firePoint.position;
        Vector2 direction = (mousePosition - firePointPosition).normalized; // Hướng từ firePoint đến con trỏ
        // Tạo viên đạn tại firePoint
        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        // Gán hướng cho viên đạn
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.SetDirection(direction); // Gán hướng bắn
        }

        // Nếu sử dụng Rigidbody2D, thiết lập vận tốc
        Rigidbody2D rb = bulletObj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * bulletSpeed; // Đặt vận tốc cho viên đạn
        }
    }
}
