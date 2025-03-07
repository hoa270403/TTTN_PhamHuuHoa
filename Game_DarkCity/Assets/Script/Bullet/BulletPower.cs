using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPower : MonoBehaviour
{
    public GameObject bulletPrefab;  // Prefab của viên đạn
    public float bulletSpeed = 10f;  // Tốc độ bay của viên đạn
    public int bulletCount = 50;     // Số lượng viên đạn bắn ra mỗi đợt
    public float fireAngleSpread = 10f; // Góc phân tán của các viên đạn
    public Transform playerTransform;  // Tham chiếu đến đối tượng người chơi
    public int totalRounds = 5;  // Số lượng đợt bắn
    public float delayBetweenRounds = 0.5f;  // Độ trễ giữa các đợt bắn
    public AudioSource powerSound;
    void Update()
    {
        // Kiểm tra nếu người chơi nhấn phím P
        if (Input.GetKeyDown(KeyCode.P))
        {
            Power();  // Sử dụng Coroutine để bắn đạn theo đợt
        }
    }

    // Coroutine để bắn đạn theo đợt
    IEnumerator FirePower()
    {
        for (int round = 0; round < totalRounds; round++)
        {
            FireBulletRound();  // Bắn 1 đợt viên đạn
            yield return new WaitForSeconds(delayBetweenRounds);  // Chờ trước khi bắn đợt tiếp theo
        }
    }

    // Hàm bắn 1 đợt viên đạn
    void FireBulletRound()
    {
        // Tính toán góc phân tán giữa các viên đạn
        float angleStep = 360f / bulletCount; // Góc phân tán đều quanh vòng tròn
        float startAngle = 0f; // Bắt đầu từ góc 0 độ

        // Tạo các viên đạn theo nhiều hướng (xung quanh vòng tròn)
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + i * angleStep; // Tính toán góc cho từng viên đạn
            ShootBullet(angle);
        }
    }

    // Hàm tạo và bắn viên đạn
    void ShootBullet(float angle)
    {
        // Tạo viên đạn mới tại vị trí của người chơi
        Vector3 bulletSpawnPosition = playerTransform.position;  // Vị trí tạo viên đạn ở trung tâm (người chơi)
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPosition, Quaternion.identity);
        float angleInRad = angle * Mathf.Deg2Rad; // Chuyển đổi góc sang radians

        // Tính toán hướng của viên đạn (xung quanh vòng tròn)
        Vector2 direction = new Vector2(Mathf.Cos(angleInRad), Mathf.Sin(angleInRad)).normalized;
        bullet.GetComponent<Bullet>().SetDirection(direction);  // Gán hướng cho viên đạn

        // Thêm lực để viên đạn bay
        bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
    }
    public void Power()
    {
        powerSound.Play();
        StartCoroutine(FirePower());
    }
}
