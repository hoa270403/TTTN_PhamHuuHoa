using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damage = 10; // Lượng sát thương gây ra

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Kiểm tra nếu đối tượng va chạm có HealthSystem
        HealthBarSystem playerHealth = collision.gameObject.GetComponent<HealthBarSystem>();
        if (playerHealth != null)
        {
            // Gây sát thương cho đối tượng
            playerHealth.TakeDamage(damage);
        }
    }
}
