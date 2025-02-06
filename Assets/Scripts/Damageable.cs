using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;

    Animator animator;

    [SerializeField]
    private int _maxHealth = 100;
    public int MaxHealth 
    { 
        get
        {
            return _maxHealth;
        }
        set 
        {
            _maxHealth = value;
        }
    }
    [SerializeField]
    private int _health = 100;
    public int Health 
    {
        get 
        {
            return _health;
        }
        set
        {
            _health = value;
            // if health drop 0, character is no longer alive
            if(_health <= 0)
            {
                IsAlive = false;
            }
        }
    }
    [SerializeField]
    private bool _isAlive = true;

    [SerializeField]
    private bool isInvincible = false;


    private float timeSincehit = 0;
    public float invincibilityTime = 0.25f;

    public bool IsAlive
    {
        get
        {
            return _isAlive;
        }
        set
        {
            _isAlive = value;
            animator.SetBool(AnimationStrings.isAlive, value);
            Debug.Log("IsAlive set" + value);
        }
    }

    public bool LockVelocity
    {
        get
        {
            return animator.GetBool(AnimationStrings.lockVelocity);
        }
        set
        {
            animator.SetBool(AnimationStrings.lockVelocity, value);
        }
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isInvincible)
        {
            if (timeSincehit > invincibilityTime)
            {
                //remove invincibility
                isInvincible = false;
                timeSincehit = 0;
            }
            timeSincehit += Time.deltaTime;
        }
    }

    public bool Hit(int damage,Vector2 knockback)
    {
        if (IsAlive && !isInvincible)
        {
            Health -= damage;
            isInvincible = true;
            LockVelocity = true;
            animator.SetTrigger(AnimationStrings.hitTrigger);
            damageableHit?.Invoke(damage, knockback);
            CharacterEvents.characterDamaged.Invoke(gameObject, damage);

            return true;
        }
        //unable to be hit
        return false;
    }
}
