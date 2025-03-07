using UnityEngine;

public class BulletUpgrade : MonoBehaviour
{
    public float speed = 20f;          // Tốc độ di chuyển
    public float lifetime = 2f;        // Thời gian tồn tại của viên đạn
    private Vector2 direction;         // Hướng di chuyển
    public GameObject explosionPrefab; // Prefab hiệu ứng nổ (animation hoặc particle system)

    public int bulletLevel = 1;        // Cấp độ đạn (mặc định là 1)

    void Start()
    {
        Destroy(gameObject, lifetime); // Hủy viên đạn sau thời gian tồn tại
    }

    void Update()
    {
        // Di chuyển viên đạn theo hướng
        transform.Translate(direction * speed * Time.deltaTime);
    }

    // Thiết lập hướng cho viên đạn
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized; // Chuẩn hóa hướng
    }

    // Cập nhật các thuộc tính của đạn khi thay đổi cấp độ
    public void UpgradeBullet(int level)
    {
        bulletLevel = level;

        // Thay đổi tốc độ và sát thương tùy vào cấp độ
        switch (bulletLevel)
        {
            case 1:
                speed = 20f; // Tốc độ đạn cấp 1
                break;
            case 2:
                speed = 25f; // Tốc độ đạn cấp 2
                break;
            case 3:
                speed = 30f; // Tốc độ đạn cấp 3
                break;
            case 4:
                speed = 35f; // Tốc độ đạn cấp 4
                break;
            case 5:
                speed = 40f; // Tốc độ đạn cấp 5
                break;
            default:
                speed = 20f; // Mặc định nếu không có cấp độ
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Player"))
        {
            return; // Không xử lý va chạm với người chơi
        }
        if (hitInfo.CompareTag("Bullet"))
        {
            return; // Không xử lý va chạm với các viên đạn khác
        }

        // Tạo hiệu ứng nổ tại vị trí va chạm
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        // Xử lý va chạm với kẻ địch
        EnemyHealth enemy = hitInfo.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(bulletLevel * 2); // Sát thương tăng dần theo cấp độ đạn
        }

        // Hủy viên đạn sau va chạm
        Destroy(gameObject);
    }
}
