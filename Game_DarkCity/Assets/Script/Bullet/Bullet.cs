using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;          // Tốc độ di chuyển
    public float lifetime = 2f;
    public int bulletDame=10;// Thời gian tồn tại của viên đạn
    private Vector2 direction;         // Hướng di chuyển
    public GameObject explosionPrefab; // Prefab hiệu ứng nổ (animation hoặc particle system)
    //public AudioSource soundHieuUngNo;
    void Start()
    {
        Destroy(gameObject, lifetime); // Hủy viên đạn sau thời gian tồn tại
    }

    void Update()
    {
        // Di chuyển viên đạn theo hướng
        transform.Translate(direction * speed * Time.deltaTime);
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized; // Chuẩn hóa hướng
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Player"))
        {
            return; // Không xử lý va chạm với người chơi
        }
        if (hitInfo.CompareTag("Bullet"))
        {
            return; // Không xử lý va chạm với người chơi
        }
        // Tạo hiệu ứng nổ tại vị trí va chạm
        if (explosionPrefab != null)
        {
            
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            //soundHieuUngNo.GetComponent<AudioSource>().Play();
        }

        // Xử lý va chạm với kẻ địch
        EnemyHealth enemy = hitInfo.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(bulletDame); // Gây 10 sát thương
        }

        // Hủy viên đạn sau va chạm
        Destroy(gameObject);
    }
}
