using Spine.Unity.Examples;
using System.Collections;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    public float speedBoostAmount = 10f;  // Tốc độ tăng tốc
    public float boostDuration = 0.5f;    // Thời gian tăng tốc (seconds)
    private bool isBoosting = false;
    public AudioSource boostSource;
    //public bool isHit = false;
    public SpineboyBeginnerModel player;
    // Phương thức tăng tốc
    public void TrySpeedBoost()
    {
        if (player.state == SpineBeginnerBodyState.Dead || player.state == SpineBeginnerBodyState.Power)
            return; // Không làm gì nếu nhân vật đã chết hoặc đang sử dụng Power.

        if (isBoosting) return; // Nếu đang tăng tốc, không làm gì thêm.

        isBoosting = true;

        // Lấy hướng di chuyển dựa trên trạng thái facingLeft
        Vector2 direction = player.facingLeft ? Vector2.left : Vector2.right; // Nếu facingLeft thì di chuyển sang trái, ngược lại di chuyển sang phải
        Vector2 initialPosition = transform.parent.position;

        // Tính toán vị trí đích (di chuyển nhanh 10m theo hướng của nhân vật)
        Vector2 targetPosition = initialPosition + direction * 10f;

        // Thực hiện di chuyển
        StartCoroutine(Boost(initialPosition, targetPosition));
    }

    private IEnumerator Boost(Vector2 initialPosition, Vector2 targetPosition)
    {
        float startTime = Time.time;
        float journeyLength = Vector2.Distance(initialPosition, targetPosition);

        while (Time.time - startTime < boostDuration)
        {
            float distanceCovered = (Time.time - startTime) * speedBoostAmount;
            float fractionOfJourney = distanceCovered / journeyLength;
            transform.parent.position = Vector2.Lerp(initialPosition, targetPosition, fractionOfJourney);

            yield return null;
        }

        // Đảm bảo vị trí cuối cùng đúng
        transform.parent.position = targetPosition;

        isBoosting = false;
    } // Tham chiếu đến script SpeedBoost
      // Trạng thái của nhân vật
    public void Update()
    {
        // Kiểm tra khi người chơi nhấn Shift để kích hoạt tăng tốc
        if (Input.GetKeyDown(KeyCode.B))
        {
            boostSource.Play();
            TrySpeedBoost();
        }

    }
}
