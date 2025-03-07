using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    public Transform teleportTarget; // Điểm dịch chuyển mục tiêu
    public float timeToTeleport = 2f; // Thời gian cần để dịch chuyển (2 giây)

    private float timer = 0f; // Bộ đếm thời gian
    private bool isPlayerInRange = false; // Kiểm tra xem người chơi có đang chạm vào cổng hay không
    public AudioSource soundTele;
    public ParticleSystem teleEffect;
    private void Update()
    {
        if (isPlayerInRange)
        {
            timer += Time.deltaTime; // Tăng bộ đếm thời gian theo mỗi khung hình

            // Nếu thời gian đạt đủ yêu cầu
            if (timer >= timeToTeleport)
            {
                // Dịch chuyển người chơi
                GameObject player = GameObject.FindWithTag("Player");
                if (player != null)
                {
                    player.transform.position = teleportTarget.position;
                }

                // Reset bộ đếm thời gian và thoát khỏi vùng chạm
                timer = 0f;
                isPlayerInRange = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            teleEffect.Play();
            soundTele.Play();
            isPlayerInRange = true; // Người chơi bắt đầu chạm vào cổng
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
            //soundTele.Pause();
            isPlayerInRange = false;
           // Người chơi rời khỏi cổng, hủy bộ đếm
            timer = 0f; // Reset bộ đếm thời gian nếu người chơi ra ngoài
        }
    }
}
