using UnityEngine;
using Spine.Unity;

namespace Spine.Unity.Examples
{
    public class EnemyAttack : MonoBehaviour
    {
        [Header("Detection Range")]
        public float detectionRange = 10f; // Phạm vi phát hiện người chơi
        public Transform player; // Tham chiếu đến người chơi

        [Header("Attack Settings")]
        public GameObject projectilePrefab; // Đạn sẽ được ném
        public float attackCooldown = 2f; // Thời gian giữa các lần tấn công
        private float lastAttackTime = 0f; // Thời gian của lần tấn công trước
        public AnimationReferenceAsset attackAnimation; // Hoạt ảnh tấn công từ Spine
        public AnimationReferenceAsset idleAnimation; // Animation Idle

        [Header("Attack Direction")]
        public Transform attackPoint; // Điểm bắt đầu để ném đạn (có thể là tay hoặc miệng của kẻ địch)
        public AudioSource attackSound;
        public float attackSoundPitchOffset = 0.13f;
        private SkeletonAnimation skeletonAnimation; // Component SkeletonAnimation

        private bool isAttacking = false;

        void Start()
        {
            skeletonAnimation = GetComponent<SkeletonAnimation>(); // Lấy SkeletonAnimation component
        }

        void Update()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= detectionRange)
            {
                DetectAndAttackPlayer();
            }
            else
            {
                SetIdleAnimation();
            }
        }

        private void DetectAndAttackPlayer()
        {
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                lastAttackTime = Time.time;
                isAttacking = true;

                // Kích hoạt hoạt ảnh tấn công trước khi ném đạn
                PlayAttackAnimation();

                // Tạo một đạn mới và ném về phía người chơi
                AttackPlayer();

                // Chuyển về idle sau khi tấn công xong
                Invoke(nameof(SetIdleAnimation), 1f);
            }
        }

        private void AttackPlayer()
        {
            attackSound.Play();
            attackSound.pitch = GetRandomPitch(attackSoundPitchOffset);

            GameObject projectile = Instantiate(projectilePrefab, attackPoint.position, Quaternion.identity);
            Vector3 direction = (player.position - transform.position).normalized;

            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * 10f; // 10f là tốc độ bay của đạn
            }

            projectile.transform.right = direction;
        }

        private void PlayAttackAnimation()
        {
            if (attackAnimation != null && skeletonAnimation != null)
            {
                skeletonAnimation.state.SetAnimation(0, attackAnimation, false);
            }
        }

        private void SetIdleAnimation()
        {
            if (idleAnimation != null && skeletonAnimation != null)
            {
                // Chỉ đổi animation nếu nó khác animation hiện tại
                if (skeletonAnimation.AnimationName != idleAnimation.name)
                {
                    skeletonAnimation.state.SetAnimation(0, idleAnimation, true);
                }
                isAttacking = false;
            }
        }


        public float GetRandomPitch(float maxPitchOffset)
        {
            return 1f + Random.Range(-maxPitchOffset, maxPitchOffset);
        }
    }
}
