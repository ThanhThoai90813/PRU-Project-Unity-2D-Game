using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D),typeof(TouchingDirections),typeof(Damageable))]
public class EnemyScript : MonoBehaviour
{
    public float walkAcceleration = 3f;
    public float maxSpeed = 3f;
    public float walkStopRate = 0.05f;
    public DetectionZone attackZone;
    public DetectionZone cliffDetectionZone;
    public GameObject healthPickupPrefab;
    private Transform target; 
    public float chaseSpeed = 4f; 
    public float attackRange = 1.5f;
    private bool isKnockedBack = false;
    private float knockbackTime = 0.2f;
    private bool isChasing = false;  


    Rigidbody2D rb;
    TouchingDirections touchingDirections;
    Animator animator;
    Damageable damageable;
    public enum WalkableDirection
    {
        Right, Left
    }
    private WalkableDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.right;

    public WalkableDirection WalkDirection

    {
        get { return _walkDirection; }
        set {
            if (_walkDirection != value)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
                if(value == WalkableDirection.Right )
                {
                    walkDirectionVector = Vector2.right;
                }else if(value == WalkableDirection.Left)
                {
                    walkDirectionVector = Vector2.left;
                }
            }
            _walkDirection = value; }
    }

    public bool _hasTarget  = false;
    public bool HasTarget { get { return _hasTarget; } private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    public float AttackCooldown { get {
            return animator.GetFloat(AnimationStrings.attackCooldown);
        } private set { 
            animator.SetFloat(AnimationStrings.attackCooldown, Mathf.Max(value, 0));
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
        WalkDirection = WalkableDirection.Left;
    }
    void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0;

        if (AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }

        if (HasTarget) // Nếu phát hiện Player
        {
            target = attackZone.detectedColliders[0].transform; // Lấy vị trí Player
            float distanceToPlayer = Vector2.Distance(transform.position, target.position);

            if(distanceToPlayer > attackRange)
            {
                isChasing = true;
            }
            else
            {
                isChasing= false;
            }
        }
        else
        {
            target = null; 
            isChasing=false;
        }
    }

    private void FixedUpdate()
    {
        if (isKnockedBack)
        {
            return; 
        }

        if (target != null && isChasing) 
        {
            float direction = Mathf.Sign(target.position.x - transform.position.x); // Xác định hướng
            rb.linearVelocity = new Vector2(direction * chaseSpeed, rb.linearVelocity.y);

            if (direction > 0)
                WalkDirection = WalkableDirection.Right;
            else
                WalkDirection = WalkableDirection.Left;
        }
        else if (CanMove && target == null) // Nếu không có Player, đi tuần bình thường
        {
            rb.linearVelocity = new Vector2(Mathf.Clamp(rb.linearVelocity.x + (walkAcceleration * walkDirectionVector.x * Time.fixedDeltaTime), -maxSpeed, maxSpeed), rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(Mathf.Lerp(rb.linearVelocity.x, 0, walkStopRate), rb.linearVelocity.y);
        }
    }
    private void ChasePlayer()
    {
        if (target != null)
        {
            float direction = Mathf.Sign(target.position.x - transform.position.x); 

            rb.linearVelocity = new Vector2(direction * chaseSpeed, rb.linearVelocity.y);

            if (direction > 0)
                WalkDirection = WalkableDirection.Right;
            else if (direction < 0)
                WalkDirection = WalkableDirection.Left;
        }
    }

    private void FlipDirection()
    {
        if(WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        }else if (WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }
    }

    public void OnHit(int damage, Vector2 knockback) {
        isKnockedBack = true;
        rb.linearVelocity = knockback;
        StartCoroutine(ResetKnockback());
    }
    private IEnumerator ResetKnockback()
    {
        yield return new WaitForSeconds(knockbackTime);
        isKnockedBack = false;  
    }
    //quay dau khi ko gap dat nua
    public void OnCliffDetected()
    {
        if(touchingDirections.IsGrounded)
        {
            FlipDirection();
        }
    }
    public void SpawnHealthPickup()
    {
        if (healthPickupPrefab != null)
        {
            Instantiate(healthPickupPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }
    }
    private void OnDrawGizmos()
    {
        if (target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }


}
