using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Spine.Unity.Examples {
    [SelectionBase]
    public class SpineboyBeginnerModel : MonoBehaviour {

        #region Inspector
        [Header("Current State")]
        public SpineBeginnerBodyState state; // Trạng thái hiện tại.
        public bool facingLeft; // Quay mặt trái.
        [Range(-1f, 1f)]
        public float currentSpeed;
        // Tốc độ hiện tại.

        [Header("Balance")]
        public float moveSpeed = 5f; // Tốc độ di chuyển nhân vật.
        public float shootInterval = 0.12f; // Thời gian tối thiểu giữa các lần bắn.
        #endregion

        float lastShootTime;

        public event System.Action ShootEvent;
        public event System.Action StartAimEvent;
        public event System.Action StopAimEvent;
        public event System.Action DeathEvent; // Thêm sự kiện chết.
        public event System.Action PowerEvent; // Sự kiện sử dụng chiêu thức
        public event System.Action HitEvent;
        public float powerCooldownTime = 0f;  // Thời gian hồi chiêu
        public float powerCooldown = 5f;      // Thời gian hồi chiêu (có thể chỉnh sửa theo yêu cầu)
        public Text cooldownText;
        /*public float speedBoostAmount = 10f;  // Tốc độ tăng tốc
        public float boostDuration = 0.5f;    // Thời gian tăng tốc (seconds)
        private bool isBoosting = false;
        public bool isHit=false;

        // Phương thức tăng tốc
        public void TrySpeedBoost()
        {
            if (state == SpineBeginnerBodyState.Dead || state == SpineBeginnerBodyState.Power)
                return; // Không làm gì nếu nhân vật đã chết hoặc đang sử dụng Power.

            if (isBoosting) return; // Nếu đang tăng tốc, không làm gì thêm.

            isBoosting = true;

            // Lấy hướng di chuyển dựa trên trạng thái facingLeft
            Vector2 direction = facingLeft ? Vector2.left : Vector2.right; // Nếu facingLeft thì di chuyển sang trái, ngược lại di chuyển sang phải
            Vector2 initialPosition = transform.position;

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
                transform.position = Vector2.Lerp(initialPosition, targetPosition, fractionOfJourney);

                yield return null;
            }

            // Đảm bảo vị trí cuối cùng đúng
            transform.position = targetPosition;

            isBoosting = false;
        } // Tham chiếu đến script SpeedBoost
        // Trạng thái của nhân vật */
        #region API
        // Phương thức để bắt đầu hồi chiêu
        public void StartPowerCooldown()
        {
            powerCooldownTime = powerCooldown; // Đặt lại thời gian hồi chiêu
        }
        public void TryJump() {
            if (state == SpineBeginnerBodyState.Dead || state == SpineBeginnerBodyState.Power)
                return; // Không nhảy nếu đang chết hoặc đang sử dụng Power.
            StartCoroutine(JumpRoutine());
        }

        public void TryShoot() {
            if (state == SpineBeginnerBodyState.Dead || state == SpineBeginnerBodyState.Power)
                return; // Không nhảy nếu đang chết hoặc đang sử dụng Power.
            float currentTime = Time.time;

            if (currentTime - lastShootTime > shootInterval) {
                lastShootTime = currentTime;
                if (ShootEvent != null) ShootEvent();
            }
        }
        public void TryPower()
        {
            if (state == SpineBeginnerBodyState.Dead || state == SpineBeginnerBodyState.Power || powerCooldownTime > 0)
                return; // Không làm gì nếu nhân vật đã chết hoặc đang sử dụng chiêu thức

            state = SpineBeginnerBodyState.Power; // Đặt trạng thái là Power

            if (PowerEvent != null) PowerEvent(); // Kích hoạt sự kiện
            StartPowerCooldown();
        }
        public void StartAim() {
            if (StartAimEvent != null) StartAimEvent();
        }

        public void StopAim() {
            if (StopAimEvent != null) StopAimEvent();
        }

        /*public void TryMove(float speed) {
            if (state == SpineBeginnerBodyState.Dead || state == SpineBeginnerBodyState.Power)
                return; // Không nhảy nếu đang chết hoặc đang sử dụng Power.

            currentSpeed = speed;

            if (speed != 0) {
                bool speedIsNegative = (speed < 0f);
                facingLeft = speedIsNegative;
                Vector3 moveDirection = new Vector3(speed * moveSpeed * Time.deltaTime, 0, 0);
                transform.Translate(moveDirection);
            }

            if (state != SpineBeginnerBodyState.Jumping) {
                state = (speed == 0) ? SpineBeginnerBodyState.Idle : SpineBeginnerBodyState.Running;
            }
        }*/
        public void TryHit()
        {
            if (state == SpineBeginnerBodyState.Dead || state == SpineBeginnerBodyState.Power)
                return; // Không làm gì nếu nhân vật đã chết hoặc đang sử dụng Power.

            state = SpineBeginnerBodyState.Hit; // Đặt trạng thái là Hit
            if (HitEvent != null) HitEvent(); // Kích hoạt sự kiện Hit
            Debug.Log("Hit");
            // Sau một thời gian ngắn, trở về trạng thái Idle
            StartCoroutine(ResetHitStateAfterDelay(0.3f)); // Thời gian bị Hit, có thể điều chỉnh
        }

        private IEnumerator ResetHitStateAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            if (state == SpineBeginnerBodyState.Hit)
            {
                state = SpineBeginnerBodyState.Idle; // Trở về trạng thái Idle
            }
        }
        public void TryMove(float speed)
        {
            if (state == SpineBeginnerBodyState.Dead || state == SpineBeginnerBodyState.Power ||state==SpineBeginnerBodyState.Hit)
                return; // Không di chuyển nếu nhân vật chết hoặc đang sử dụng power.

            currentSpeed = speed;

            // Debugging: In ra giá trị của speed và currentSpeed
            //Debug.Log($"TryMove called with speed: {speed}, currentSpeed: {currentSpeed}");

            if (speed != 0)
            {
                bool speedIsNegative = (speed < 0f);
                facingLeft = speedIsNegative;

                // Debugging: In ra hướng di chuyển
               // Debug.Log($"Moving direction: {(speedIsNegative ? "Left" : "Right")}");

                Vector3 moveDirection = new Vector3(speed * moveSpeed * Time.deltaTime, 0, 0);
                transform.Translate(moveDirection);
            }

            // Debugging: In ra trạng thái hiện tại của nhân vật (state)
            if (state != SpineBeginnerBodyState.Jumping)
            {
                //Debug.Log($"Updating state: {(speed == 0 ? "Idle" : "Running")}");
                state = (speed == 0) ? SpineBeginnerBodyState.Idle : SpineBeginnerBodyState.Running;
            }
        }

        public void Die() {
            if (state == SpineBeginnerBodyState.Dead) return;

            state = SpineBeginnerBodyState.Dead;
            if (DeathEvent != null) DeathEvent(); // Gọi sự kiện chết.
        }
        #endregion
        // Update để giảm dần thời gian cooldown
        void Update()
        {
            if (powerCooldownTime > 0)
            {
                powerCooldownTime -= Time.deltaTime;
                if (cooldownText != null)
                {
                    cooldownText.text = Mathf.Ceil(powerCooldownTime).ToString(); // Cập nhật đếm ngược
                }
            }
            else
            {
                if (cooldownText != null)
                {
                    cooldownText.text = "Ready"; // Hiển thị khi chiêu đã sẵn sàng
                }
            }
            // Kiểm tra khi người chơi nhấn Shift để kích hoạt tăng tốc
            
        }
        IEnumerator JumpRoutine() {
            if (state == SpineBeginnerBodyState.Jumping) yield break;

            state = SpineBeginnerBodyState.Jumping;

            const float jumpTime = 1.0f;
            const float half = jumpTime * 0.5f;
            const float jumpPower = 50f;

            for (float t = 0; t < half; t += Time.deltaTime) {
                float d = jumpPower * (half - t);
                transform.Translate((d * Time.deltaTime) * Vector3.up);
                yield return null;
            }

            for (float t = 0; t < half; t += Time.deltaTime) {
                float d = jumpPower * t;
                transform.Translate((d * Time.deltaTime) * Vector3.down);
                yield return null;
            }

            state = SpineBeginnerBodyState.Idle;
        }
    }

    public enum SpineBeginnerBodyState
    {
        Idle,
        Running,
        Jumping,
        Dead,
        Power,
        Hit
    }
}
