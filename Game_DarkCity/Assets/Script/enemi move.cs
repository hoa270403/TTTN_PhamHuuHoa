using UnityEngine;

public class EnemyFollowPlayerWithFlip : MonoBehaviour
{
    public Transform player; // Tham chiếu tới vị trí của người chơi
    public float speed = 2f; // Tốc độ di chuyển của kẻ địch
    public float stopDistance = 1f; // Khoảng cách tối thiểu để kẻ địch dừng lại
    private bool isFacingRight = true; // Theo dõi hướng kẻ địch đang quay mặt

    void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("Player not assigned in EnemyFollowPlayerWithFlip script!");
            return;
        }

        // Tính khoảng cách giữa kẻ địch và người chơi
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > stopDistance)
        {
            // Tính hướng di chuyển
            Vector2 direction = (player.position - transform.position).normalized;

            // Di chuyển kẻ địch
            transform.position += (Vector3)direction * speed * Time.deltaTime;

            // Xoay mặt kẻ địch dựa trên hướng di chuyển
            FlipTowardsPlayer();
        }
    }

    void FlipTowardsPlayer()
    {
        // Kiểm tra xem người chơi ở bên trái hay bên phải kẻ địch
        if (player.position.x > transform.position.x && !isFacingRight)
        {
            Flip(); // Quay mặt sang phải
        }
        else if (player.position.x < transform.position.x && isFacingRight)
        {
            Flip(); // Quay mặt sang trái
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;

        // Lật kẻ địch bằng cách thay đổi scale trên trục X
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
