using UnityEngine;
using Spine.Unity;

namespace Spine.Unity.Examples
{
    public class EnemyAI : MonoBehaviour
    {
        [Header("Patrol Settings")]
        public float patrolRadius = 10f; // Bán kính tuần tra
        public float speed = 2f; // Tốc độ di chuyển
        private Vector3 startPoint; // Điểm xuất phát ban đầu
        private Vector3 targetPosition; // Điểm đích tiếp theo

        [Header("Animations")]
        public AnimationReferenceAsset moveAnimation; // Hoạt ảnh di chuyển
        private SkeletonAnimation skeletonAnimation; // Component SkeletonAnimation

        void Start()
        {
            skeletonAnimation = GetComponent<SkeletonAnimation>();
            startPoint = transform.position; // Lưu vị trí ban đầu
            SetNewPatrolPoint(); // Chọn điểm đến ban đầu
        }

        void Update()
        {
            Patrol(); // Gọi tuần tra liên tục
        }

        private void Patrol()
        {
            Vector3 direction = (targetPosition - transform.position).normalized;

            // Lật nhân vật theo hướng di chuyển
            if (direction.x > 0)
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            else if (direction.x < 0)
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

            // Di chuyển về phía điểm đích
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            // Khi đến gần điểm đích, chọn điểm mới
            if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
            {
                SetNewPatrolPoint();
            }

            // Kích hoạt hoạt ảnh di chuyển
            PlayAnimation(moveAnimation);
        }

        private void SetNewPatrolPoint()
        {
            // Chọn điểm ngẫu nhiên trong bán kính patrolRadius xung quanh startPoint
            Vector2 randomPoint = Random.insideUnitCircle * patrolRadius;
            targetPosition = startPoint + new Vector3(randomPoint.x, randomPoint.y, 0);
        }

        private void PlayAnimation(AnimationReferenceAsset animation)
        {
            if (animation == null || skeletonAnimation == null) return;
            skeletonAnimation.state.SetAnimation(0, animation, true);
        }
    }
}
