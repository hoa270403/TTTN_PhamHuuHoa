using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemi : MonoBehaviour
{
    public float speed = 20f;          // Tốc độ di chuyển
    public float lifetime = 2f;        // Thời gian tồn tại của viên đạn
    private Vector2 direction;         // Hướng di chuyển
    public GameObject explosionPrefab; // Prefab hiệu ứng nổ (animation hoặc particle system)

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

        /*if (hitInfo.CompareTag("Bullet"))
        {
            return; 
        }
        if (hitInfo.CompareTag("Enemi"))
        {
            return;
        }
        if (hitInfo.CompareTag("Coin"))
        {
            return;
        }*/
        if (!hitInfo.CompareTag("Player"))
        {
            return;
        }
        // Tạo hiệu ứng nổ tại vị trí va chạm
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        // Xử lý va chạm với kẻ địch
        HealthBarSystem player = hitInfo.GetComponent<HealthBarSystem>();
        if (player != null)
        {
            player.TakeDamage(10); // Gây 10 sát thương
        }

        // Hủy viên đạn sau va chạm
        Destroy(gameObject);
    }
}
