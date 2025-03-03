using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

//quản lý Player
[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections),typeof(Damageable))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 10f;
    public float runSpeed = 7f;
    public float airWalkSpeed = 5f;
	public float jumpImpulse = 9f;
	Vector2 moveInput;
    TouchingDirections touchingDirections;
    Damageable damageable;
    private bool IsDead = false;

    public float CurrentMoveSpeed { get
        {
            if (CanMove)
            {
                if (IsMoving && !touchingDirections.IsOnWall)
                {
                    if (touchingDirections.IsGrounded)
                    {
                        if (IsRunning)
                        {
                            return runSpeed;
                        }
                        else
                        {
                            return walkSpeed;
                        }
                    }
                    else
                    {
                        return airWalkSpeed;
                    }
                }
                else
                {
                    return 0;
                }
            }     
            else
            {
                return 0;
            }
        }
    }


    [SerializeField] 
    private bool _isMoving = false;

    public bool IsMoving 
    {   
        get 
        { 
            return _isMoving;
        } private set 
        { 
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        } 
    }

	[SerializeField]
	private bool _isRunning = false;

    public bool IsRunning 
    { 
        get
        {
            return _isRunning;
        }
        set
        {
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }

    public bool _isFacingRight = true;
	public bool IsFacingRight { get { return _isFacingRight; } private set 
        {
            if(_isFacingRight != value) 
            {
                transform.localScale *= new Vector2(-1, 1);
            }

            _isFacingRight = value;
        } }

	Rigidbody2D rb;
    Animator animator;
	
    public bool CanMove { get
        {
            return animator.GetBool(AnimationStrings.canMove);
        } }

    public bool IsALive
    {
        get
        {
            return animator.GetBool(AnimationStrings.isAlive);
        }
    }

    public void OnHealthChange(int currentHealth,int maxHealth)
    {
        if (currentHealth >= maxHealth || currentHealth < 0) return;

        DBController.Instance.PLAYERHEAL = currentHealth;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
        damageable.Health = DBController.Instance.PLAYERHEAL;
        damageable.healthChanged.AddListener(OnHealthChange);
    }


    [System.Obsolete]
    private void FixedUpdate()
    {   
        if (!damageable.LockVelocity)
            rb.linearVelocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.linearVelocity.y);

        animator.SetFloat(AnimationStrings.yVelocity, rb.linearVelocity.y);

        if (rb.velocity.y > 0)
        {
            rb.gravityScale = 1.5f;  // Nhảy lên, trọng lực nhẹ hơn
        }
        else if (rb.velocity.y < 0)
        {
            rb.gravityScale = 3f;  // Rơi xuống nhanh hơn
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (IsALive)
        {
            IsMoving = moveInput != Vector2.zero;

            SetFacingDirection(moveInput);
        }
        else
        {
            IsMoving = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Vector2 knockbackDir = (transform.position - collision.transform.position).normalized;
            Vector2 knockbackForce = new Vector2(knockbackDir.x * 4f, 6f);

            rb.linearVelocity = Vector2.zero;
            rb.AddForce(knockbackForce, ForceMode2D.Impulse);
        }
    }

    private void SetFacingDirection(Vector2 moveInput)
	{
		if(moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if(moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight= false;
        }
	}

	public void OnRun(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            IsRunning = true;
        }else if(context.canceled)
        {
            IsRunning=false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.started && touchingDirections.IsGrounded && CanMove)
        {
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpImpulse);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }
    public void OnHit(int damage,Vector2 knockback)
    {
        if (!IsDead)
        {
            rb.linearVelocity = new Vector2(knockback.x, rb.linearVelocity.y + knockback.y);
            StartCoroutine(UnlockVelocityAfterHit());
            if (!IsALive)
            {
                Die();
            }
        }
    }

    private IEnumerator UnlockVelocityAfterHit()
    {
        yield return new WaitForSeconds(0.5f); // Thời gian chờ sau khi bị đánh
        animator.SetBool("lockVelocity", false);
    }
    public void Die()
    {
        if (!IsDead)
        {
            IsDead = true;
            animator.SetBool(AnimationStrings.isAlive, false); 
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 1f; 
            this.enabled = false;
        }
    }


}
