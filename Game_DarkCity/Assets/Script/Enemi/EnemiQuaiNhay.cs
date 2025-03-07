using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity; // Dùng Spine cho animation

public class EnemyAI : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float speed = 2f; // Tốc độ di chuyển bình thường
    public float chaseSpeed = 4f; // Tốc độ khi đuổi người chơi
    public float attackRange = 1.5f; // Phạm vi tấn công
    public float detectionRange = 6f; // Phạm vi phát hiện người chơi
    public int damage = 10; // Sát thương gây ra
    public float attackCooldown = 1.5f; // Thời gian giữa các lần tấn công

    [Header("Patrol Settings")]
    public Transform[] waypoints; // Các điểm tuần tra
    private int currentWaypointIndex = 0;

    [Header("Animations")]
    public AnimationReferenceAsset walkAnimation;
    public AnimationReferenceAsset runAnimation;
    public AnimationReferenceAsset attackAnimation;
    private SkeletonAnimation skeletonAnimation;

    private Transform player;
    public bool isChasing = false;
    public bool isAttacking = false;
    private float lastAttackTime = 0f;
    public HealthBarSystem player2;
    private void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>(); // Lấy component animation
        player = GameObject.FindGameObjectWithTag("hutdame").transform; // Tìm player theo tag
        Patrol(); // Bắt đầu tuần tra
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            StartAttack();
        }
        else if (distanceToPlayer <= detectionRange)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        if (waypoints.Length == 0 || isAttacking) return; // Không tuần tra nếu không có waypoint hoặc đang tấn công

        isChasing = false; // Không đuổi theo nữa

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        MoveTowards(targetWaypoint.position, speed, walkAnimation);

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.2f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // Chuyển sang waypoint tiếp theo
        }
    }

    private void ChasePlayer()
    {
        if (isAttacking) return; // Không đuổi nếu đang tấn công

        isChasing = true;
        MoveTowards(player.position, chaseSpeed, runAnimation);
    }

    private void StartAttack()
    {
        if (isAttacking || Time.time - lastAttackTime < attackCooldown) return;

        isAttacking = true;
        PlayAnimation(attackAnimation);
        lastAttackTime = Time.time;
        StartCoroutine(AttackPlayer());
    }

    private IEnumerator AttackPlayer()
    {
        yield return new WaitForSeconds(0.5f); // Đợi animation ra đòn
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
           player2.TakeDamage(damage); // Gây sát thương cho người chơi
        }
        yield return new WaitForSeconds(attackCooldown - 0.5f); // Đợi cooldown
        isAttacking = false;
    }

    private void MoveTowards(Vector3 target, float moveSpeed, AnimationReferenceAsset animation)
    {
        if (animation == null || skeletonAnimation == null) return; // Đảm bảo animation hợp lệ

        // Tính hướng di chuyển
        Vector3 direction = (target - transform.position).normalized;

        // Di chuyển theo cả X và Y
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        // Quay mặt theo hướng di chuyển chỉ dựa trên trục X
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        // Chạy animation tương ứng
        PlayAnimation(animation);
    }

    private void PlayAnimation(AnimationReferenceAsset animation)
    {
        if (animation == null || skeletonAnimation == null) return;

        // Chỉ đổi animation khi nó khác animation hiện tại
        if (skeletonAnimation.AnimationName != animation.name)
        {
            skeletonAnimation.state.SetAnimation(0, animation, true);
        }
    }

}
