using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

//dùng để quản lý máu và trạng thái sống/chết của character
public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;
    public UnityEvent damageableDeath;
    public UnityEvent<int, int> healthChanged;
    [SerializeField]
    private bool isCheatInvincible = false;
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
            healthChanged?.Invoke(_health, MaxHealth);
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
    public bool isInvincible = false;

    [SerializeField]
    public float damageReductionPercentage = 0f;

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

            if (value == false)
            {
                damageableDeath.Invoke(); // Gọi sự kiện chết
                EnemyScript character = GetComponent<EnemyScript>();
                if (character != null)
                {
                    character.Die();
                    character.SpawnHealthPickup();
                }
            }
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
        if (isInvincible && !isCheatInvincible)
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

    public bool Hit(int damage, Vector2 knockback)
    {
        MeowGoro meowgoro = GetComponent<MeowGoro>();

        if (IsAlive && !isInvincible) // Chỉ nhận sát thương nếu không miễn nhiễm
        {
            float reducedDamage = damage * (1f - damageReductionPercentage / 100f);
            int finalDamage = Mathf.Max(0, (int)reducedDamage);

            Health -= finalDamage;
            isInvincible = true; // Kích hoạt miễn nhiễm
            LockVelocity = true;
            animator.SetTrigger(AnimationStrings.hitTrigger);
            damageableHit?.Invoke(finalDamage, knockback);
            CharacterEvents.characterDamaged.Invoke(gameObject, finalDamage);

            StartCoroutine(BecomeTemporarilyInvincible()); // Bắt đầu thời gian miễn nhiễm

            return true;
        }
        return false;
    }
    private IEnumerator BecomeTemporarilyInvincible()
    {
        yield return new WaitForSeconds(invincibilityTime);
        isInvincible = false; // Hết miễn nhiễm, có thể bị đánh tiếp
    }

    public bool Heal(int healthRestore)
    {
        if (IsAlive && Health < MaxHealth)
        {
            int maxHeal = Mathf.Max(MaxHealth - Health, 0);
            int actuaHeal = Mathf.Min(maxHeal, healthRestore);
            Health += actuaHeal;
            CharacterEvents.characterHealed(gameObject, actuaHeal);
            return true;
        }
        return false;
    }
    public void SetCheatInvincible(bool value)
    {
        isCheatInvincible = value;
        if (value)
        {
            Debug.Log("Cheat Invincibility ON!");
        }
        else
        {
            Debug.Log("Cheat Invincibility OFF!");
        }
    }
}
