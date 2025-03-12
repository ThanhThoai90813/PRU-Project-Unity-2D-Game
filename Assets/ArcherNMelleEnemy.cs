using System;
using UnityEngine;

public class ArcherNMelleEnemy : MonoBehaviour
{
    public float chaseRange = 4f;        // Phạm vi để enemy di chuyển về phía player
    public float detectionRange = 5f;    // Phạm vi để kích hoạt tấn công tầm xa
    public float meleeRange = 1.5f;      // Phạm vi để tấn công tầm gần
    public float shootCooldown = 2f;
    public float meleeCooldown = 1f;
    public GameObject arrowPrefab;
    public Transform firePoint;
    public float firePointOffsetX = 0.5f;
    public float moveSpeed = 3f;

    public Transform[] waypoints;
    public float waypointTolerance = 0.5f; 
    private int currentWaypointIndex = 0;

    private Animator animator;
    private Transform player;
    private float nextAttackTime;
    private bool facingRight = true;
    private Damageable damageable;
    private bool isChasing = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        damageable = GetComponent<Damageable>();    
        damageable.damageableDeath.AddListener(OnDeath);
        if (waypoints.Length == 0)
        {
            Debug.LogWarning("No waypoints assigned to " + gameObject.name);
        }
    }

    void Update()
    {
        if (damageable.IsAlive)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            FacePlayer();
            UpdateFirePointPosition();

            if (Time.time >= nextAttackTime)
            {
                if (distanceToPlayer <= meleeRange)
                {
                    // Ưu tiên tấn công tầm gần khi trong meleeRange
                    MeleeAttack();
                    isChasing = false;
                    animator.SetBool("isMoving", false);
                }
                else if (distanceToPlayer <= chaseRange)
                {
                    // Di chuyển về phía player nếu trong chaseRange
                    ChasePlayer();
                }
                else if (distanceToPlayer <= detectionRange)
                {
                    // Tấn công tầm xa nếu trong detectionRange nhưng ngoài meleeRange và chaseRange
                    Shoot();
                    animator.SetBool("isMoving", false);
                }
                //else
                //{
                //    // Nếu ra ngoài tất cả các phạm vi, dừng mọi hành động
                //    animator.SetBool("isShooting", false);
                //    animator.SetBool("isAttacking", false);
                //    animator.SetBool("isMoving", false);
                //    isChasing = false;
                //}
                else
                {
                    Patrol();
                }
            }
        }
    }

    void Patrol()
    {
        if (waypoints.Length == 0) return;

        animator.SetBool("isShooting", false);
        animator.SetBool("isAttacking", false);
        animator.SetBool("isMoving", true);

        // Di chuyển tới waypoint hiện tại
        Vector2 targetPosition = waypoints[currentWaypointIndex].position;
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        // Kiểm tra nếu đã đến gần waypoint
        if (Vector2.Distance(transform.position, targetPosition) <= waypointTolerance)
        {
            // Chuyển sang waypoint tiếp theo
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }

        // Xoay mặt theo hướng di chuyển
        if (direction.x < 0 && facingRight)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            facingRight = false;
        }
        else if (direction.x > 0 && !facingRight)
        {
            transform.localScale = new Vector3(1, 1, 1);
            facingRight = true;
        }
    }

    void ChasePlayer()
    {
        isChasing = true;
        animator.SetBool("isAttacking", false);
        animator.SetBool("isShooting", false);
        animator.SetBool("isMoving", true);
        Debug.Log("ChasePlayer: isMoving = " + animator.GetBool("isMoving"));

        // Di chuyển về phía player
        Vector2 direction = (player.position - transform.position).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    void FacePlayer()
    {
        if (player.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            facingRight = false;
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
            facingRight = true;
        }
    }

    void UpdateFirePointPosition()
    {
        Vector3 firePointLocalPos = firePoint.localPosition;
        firePointLocalPos.x = facingRight ? firePointOffsetX : -firePointOffsetX;
        firePoint.localPosition = firePointLocalPos;
    }

    void Shoot()
    {
        if (Time.time >= nextAttackTime)
        {
            animator.SetBool("isShooting", true);
            animator.SetTrigger("shootTrigger");
            nextAttackTime = Time.time + shootCooldown;
        }
    }

    private void MeleeAttack()
    {
        animator.SetBool("isAttacking", true);
        animator.SetTrigger("meleeTrigger");
        nextAttackTime = Time.time + meleeCooldown;
        Invoke(nameof(ResetMeleeAttack), meleeCooldown - 0.1f);
    }

    void ResetMeleeAttack()
    {
        animator.SetBool("isAttacking", false);
    }

    void SpawnArrow()
    {
        if (arrowPrefab != null && firePoint != null)
        {
            GameObject arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
            EnemyArrow arrowScript = arrow.GetComponent<EnemyArrow>();
            if (arrowScript != null)
            {
                arrowScript.SetDirection(facingRight ? 1 : -1);
            }
        }
        else
        {
            Debug.LogError("arrowPrefab hoặc firePoint chưa được gán!");
        }
    }

    void OnDeath()
    {
        animator.SetTrigger("dieTrigger");
        animator.SetBool("isShooting", false);
        animator.SetBool("isAttacking", false);
        animator.SetBool("isMoving", false);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}