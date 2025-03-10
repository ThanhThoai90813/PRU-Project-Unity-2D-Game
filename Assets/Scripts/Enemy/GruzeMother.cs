using System.Threading.Tasks;
using Unity.Cinemachine;
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

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip aliveSound;

    private bool isTouchingUp;
    private bool isTouchingDown;
    private bool isTouchingWall;
    private bool goingUp = true;
    private bool facingLeft = true;
   
    private Rigidbody2D enemyRB;
    private Animator enemyAnim;
    private Damageable damageable;

    [SerializeField]
    private Collider2D attackCollider;

    public void EnableAttackCollider()
    {
        if (attackCollider != null)
        {
            attackCollider.enabled = true;
        }
    }
    public void DisableAttackCollider()
    {
        if (attackCollider != null)
        {
            attackCollider.enabled = false;
        }
    }


    void Start()
    {
       idelMoveDirection.Normalize();
       attackMoveDirection.Normalize();
       enemyRB = GetComponent<Rigidbody2D>();
       enemyAnim = GetComponent<Animator>();
       damageable = GetComponent<Damageable>();

        if (audioSource != null && aliveSound != null)
        {
            audioSource.clip = aliveSound;
            audioSource.loop = true; 
            audioSource.Play();
        }
    }
    public async void StopAudio()
    {
        await Task.Delay(2000);

        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    void Update()
    {
        isTouchingUp = Physics2D.OverlapCircle(groundCheckUp.position, groundCheckRadius, groundLayer);
        isTouchingDown = Physics2D.OverlapCircle(groundCheckDown.position, groundCheckRadius, groundLayer);
        isTouchingWall = Physics2D.OverlapCircle(groundCheckWall.position, groundCheckRadius, groundLayer);
    }
    private void FixedUpdate()
    {
        if (damageable.IsAlive)
        {
            if (CanMove)
            {
                randomStatePicker();
            }
            else
            {
                enemyRB.linearVelocity = Vector3.zero;
            }
        }
    }
    public bool CanMove
    {
        get
        {
            return enemyAnim != null && enemyAnim.GetBool(AnimationStrings.canMove);
        }
    }

    void randomStatePicker()
    {
        int randdomState = Random.Range(0, 2);
        if(randdomState == 0)
        {
            enemyAnim.SetTrigger("AttackUpNDown");
        }
        else if (randdomState == 1)
        {
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
