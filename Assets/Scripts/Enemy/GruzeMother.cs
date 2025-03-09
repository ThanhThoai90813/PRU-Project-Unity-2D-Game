using UnityEngine;

public class GruzeMother : MonoBehaviour
{
    //For Idel Stage
    [Header("Idel")]
    [SerializeField]
    float idelMoveSpeed;
    [SerializeField]
    Vector2 idelMoveDirection;


    //For Attack Up N down Stage
    [Header("AttackUpNDown")]
    [SerializeField]
    float attackMoveSpeed;
    [SerializeField]
    Vector2 attackMoveDirection;

    //For Attack Player Stage
    [Header("AttackPlayer")]
    [SerializeField]
    float attackPlayerSpeed;
    [SerializeField]
    Transform player;
    private Vector2 playerPositon;
    private bool hasPlayerPosition;

    //Other
    [Header("Other")]
    [SerializeField]
    Transform groundCheckUp;
    [SerializeField]
    Transform groundCheckDown;
    [SerializeField]
    Transform groundCheckWall;
    [SerializeField]
    float groundCheckRadius;
    [SerializeField]
    LayerMask groundLayer;

    [Header("Damage Settings")]
    [SerializeField]
    private int attackDamage = 20;
    [SerializeField]
    private Vector2 knockback = new Vector2(2f, 2f); 
    [SerializeField]
    private float attackCooldown = 5f;
    private float lastAttackTime;
   
    private bool isTouchingUp;
    private bool isTouchingDown;
    private bool isTouchingWall;
    private bool goingUp = true;
    private bool facingLeft = true;
   
    private Rigidbody2D enemyRB;
    private Animator enemyAnim;

    void Start()
    {
       idelMoveDirection.Normalize();
       attackMoveDirection.Normalize();
       enemyRB = GetComponent<Rigidbody2D>();
       enemyAnim = GetComponent<Animator>();
       lastAttackTime = -attackCooldown;
    }

    void Update()
    {
        isTouchingUp = Physics2D.OverlapCircle(groundCheckUp.position, groundCheckRadius, groundLayer);
        isTouchingDown = Physics2D.OverlapCircle(groundCheckDown.position, groundCheckRadius, groundLayer);
        isTouchingWall = Physics2D.OverlapCircle(groundCheckWall.position, groundCheckRadius, groundLayer);
    }

    void randomStatePicker()
    {
        int randdomState = Random.Range(0, 2);
        if(randdomState == 0)
        {
            //attackUpNDown aniamton
            enemyAnim.SetTrigger("AttackUpNDown");
        }
        else if (randdomState == 1)
        {
            //attack player animation
            enemyAnim.SetTrigger("AttackPlayer");
        }
    }

    public void IdelState()
    {
        if (isTouchingUp && goingUp)
        {
            ChangeDirection();
        }
        else if (isTouchingDown && !goingUp)
        {
            ChangeDirection();
        }

        if(isTouchingWall)
        {
            if (facingLeft)
            {
                Flip();
            }
            else if (!facingLeft)
            {
                Flip();
            }
        }

        enemyRB.linearVelocity = idelMoveSpeed * idelMoveDirection;
    }
    public void AttackUpNDown()
    {
        if (isTouchingUp && goingUp)
        {
            ChangeDirection();
        }
        else if (isTouchingDown && !goingUp)
        {
            ChangeDirection();
        }

        if (isTouchingWall && facingLeft)
        {

            Flip();
        }
        else if (!facingLeft && isTouchingWall)
        {
            Flip();
        }

        enemyRB.linearVelocity = attackMoveSpeed * attackMoveDirection;
    }

    public void AttackPlayer()
    {
        if (!hasPlayerPosition)
        {
            FlipTowardsPlayer();
            playerPositon = player.position - transform.position;
            playerPositon.Normalize();
            hasPlayerPosition = true;
        }
        if (hasPlayerPosition)
        {
            enemyRB.linearVelocity = playerPositon * attackPlayerSpeed;
        }
        if(isTouchingWall || isTouchingDown)
        {   
            //play slame animation
            enemyAnim.SetTrigger("Slammed");
            enemyRB.linearVelocity = Vector2.zero;
            hasPlayerPosition = false;
        }
    }

    public void FlipTowardsPlayer()
    {
        float playerDirection = player.position.x - transform.position.x;

        if(playerDirection>0 && facingLeft)
        {
            Flip();
        }
        else if(playerDirection<0 && !facingLeft)
        {
            Flip();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Time.time >= lastAttackTime + attackCooldown)
        {
            Damageable playerDamageable = collision.GetComponent<Damageable>();
            if (playerDamageable != null)
            {
                Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                Vector2 appliedKnockback = knockbackDirection * knockback;

                bool hitSuccessful = playerDamageable.Hit(attackDamage, appliedKnockback);
                if (hitSuccessful)
                {
                    lastAttackTime = Time.time;
                }
            }
        }
    }


    public void ChangeDirection()
    {
        goingUp = !goingUp;
        idelMoveDirection.y *= -1;
        attackMoveDirection.y *= -1;
    }

    public void Flip()
    {
        facingLeft = !facingLeft;
        idelMoveDirection.x *= -1;
        attackMoveDirection.x *= -1;
        transform.Rotate(0, 180, 0);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(groundCheckUp.position, groundCheckRadius);
        Gizmos.DrawWireSphere(groundCheckDown.position, groundCheckRadius);
        Gizmos.DrawWireSphere(groundCheckWall.position, groundCheckRadius);

    }

}
