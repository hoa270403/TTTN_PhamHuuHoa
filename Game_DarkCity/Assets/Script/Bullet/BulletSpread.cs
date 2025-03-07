using UnityEngine;

public class BulletSpread : MonoBehaviour
{
    public float speed = 20f;              // Tốc độ di chuyển của viên đạn
    public float lifetime = 2f;            // Thời gian tồn tại của viên đạn
    public int bulletDame = 10;            // Sát thương của viên đạn
    private Vector2 direction;             // Hướng di chuyển của viên đạn
    public GameObject explosionPrefab;     // Prefab hiệu ứng nổ (animation hoặc particle system)

    public GameObject bulletPrefab;        // Prefab của viên đạn con
    public int numberOfChildBullets = 8;   // Số lượng viên đạn con (càng nhiều viên đạn con, vòng tròn càng chặt)

    void Start()
    {
        Destroy(gameObject, lifetime);     // Hủy viên đạn sau thời gian tồn tại
    }

    void Update()
    {
        // Di chuyển viên đạn chính theo hướng
        transform.Translate(direction * speed * Time.deltaTime);
    }

    // Thiết lập hướng di chuyển của viên đạn
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized; // Chuẩn hóa hướng
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // Tránh va chạm với người chơi hoặc các viên đạn khác
        if (hitInfo.CompareTag("Player") || hitInfo.CompareTag("Bullet"))
        {
            return;
        }

        // Tạo hiệu ứng nổ nếu có
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        // Tạo các viên đạn con theo hình vòng tròn khi viên đạn chính va chạm
        FireChildBullets();

        // Xử lý va chạm với kẻ địch
        EnemyHealth enemy = hitInfo.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(bulletDame); // Gây sát thương cho kẻ địch
        }

        // Hủy viên đạn chính sau va chạm
        Destroy(gameObject);
    }

    // Tạo các viên đạn con theo hình vòng tròn
    private void FireChildBullets()
    {
        // Tính góc giữa các viên đạn con
        float angleStep = 360f / numberOfChildBullets;

        // Vị trí trung tâm của viên đạn chính
        Vector2 centerPosition = transform.position;

        for (int i = 0; i < numberOfChildBullets; i++)
        {
            // Tính toán góc cho mỗi viên đạn con
            float angle = i * angleStep;
            Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle)); // Chuyển đổi góc thành vector hướng

            // Tạo viên đạn con
            GameObject childBullet = Instantiate(bulletPrefab, centerPosition, Quaternion.identity);
            BulletSpread childBulletScript = childBullet.GetComponent<BulletSpread>();
            if (childBulletScript != null)
            {
                childBulletScript.SetDirection(direction); // Gán hướng di chuyển cho viên đạn con
            }

            Rigidbody2D rb = childBullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * speed; // Cập nhật vận tốc cho viên đạn con
            }
        }
    }
}
