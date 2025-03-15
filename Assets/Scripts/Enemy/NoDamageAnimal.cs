using System.Collections.Generic;
using UnityEngine;

public class NodamageAnimal : MonoBehaviour
{
    public float flightSpeed = 1.5f;
    private float waypointReachedDistance = 0.2f;
    public List<Transform> waypoints;

    public float weaveAmplitude = 0.3f;
    public float weaveFrequency = 4f;
    public bool useWeaving = true;
    private float randomOffset = 0f;

    // Thời gian chờ tối thiểu và tối đa trước khi thay đổi trạng thái
    public float minWaitTime = 2f;
    public float maxWaitTime = 5f;

    // Tên animation cho từng trạng thái, có thể gán trong Inspector
    [Header("Animation Names")]
    public string walkAnimationName = "Walk";
    public string idleAnimationName = "Idle";

    private enum AnimalState { Moving, Idle }
    private AnimalState currentState = AnimalState.Moving;

    Animator animator;
    Rigidbody2D rb;

    Transform nextWaypoint;
    int waypointNum = 0;
    private float timeCounter = 0f;
    private float stateChangeTimer = 0f;
    private float timeToNextStateChange = 0f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (waypoints.Count > 0)
        {
            waypointNum = 0;
            nextWaypoint = waypoints[waypointNum];
        }
        else
        {
            Debug.LogWarning($"Danh sách waypoints trống cho {gameObject.name}! Vui lòng thêm waypoint trong Inspector.");
        }
        randomOffset = Random.Range(0f, 2f);
        SetNextStateChangeTime();
    }

    private void FixedUpdate()
    {
        timeCounter += Time.fixedDeltaTime;
        stateChangeTimer += Time.fixedDeltaTime;

        // Kiểm tra nếu đã đến lúc chuyển trạng thái
        if (stateChangeTimer >= timeToNextStateChange)
        {
            SwitchStateRandomly();
            SetNextStateChangeTime();
            stateChangeTimer = 0f;
        }

        // Thực thi hành vi dựa trên trạng thái
        switch (currentState)
        {
            case AnimalState.Moving:
                Flight();
                PlayAnimation(walkAnimationName); // Phát animation di chuyển
                break;
            case AnimalState.Idle:
                rb.linearVelocity = Vector2.zero; // Dừng di chuyển
                PlayAnimation(idleAnimationName); // Phát animation đứng im
                break;
        }
    }

    private void Flight()
    {
        if (nextWaypoint == null) return; // Tránh lỗi nếu waypoint không tồn tại

        Vector2 directionToWaypoint = (nextWaypoint.position - transform.position).normalized;
        Vector2 targetVelocity = directionToWaypoint * flightSpeed;

        if (useWeaving)
        {
            Vector2 perpendicularDir = new Vector2(-directionToWaypoint.y, directionToWaypoint.x);
            float weaveOffset = Mathf.Sin((timeCounter + randomOffset) * weaveAmplitude) * weaveFrequency;

            rb.linearVelocity = targetVelocity;
            Vector2 weavePosition = perpendicularDir * weaveOffset;
            rb.position += weavePosition * Time.fixedDeltaTime * weaveFrequency;
        }
        else
        {
            rb.linearVelocity = targetVelocity;
        }

        UpdateDirection();

        float distance = Vector2.Distance(nextWaypoint.position, transform.position);
        if (distance <= waypointReachedDistance)
        {
            waypointNum++;
            if (waypointNum >= waypoints.Count)
            {
                waypointNum = 0;
            }
            nextWaypoint = waypoints[waypointNum];
            randomOffset = Random.Range(0f, 1f);
        }
    }

    private void UpdateDirection()
    {
        Vector3 locScale = transform.localScale;
        if (transform.localScale.x > 0)
        {
            if (rb.linearVelocity.x < 0)
            {
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            }
        }
        else
        {
            if (rb.linearVelocity.x > 0)
            {
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            }
        }
    }

    // Hàm để phát animation với tên bất kỳ
    // Trong hàm PlayAnimation
    private void PlayAnimation(string animationName)
    {
        if (animator != null && !string.IsNullOrEmpty(animationName))
        {
            if (animator.HasState(0, Animator.StringToHash(animationName)))
            {
                animator.Play(animationName);
            }
            else
            {
                Debug.LogError($"Trạng thái animation '{animationName}' không tồn tại trong Animator của {gameObject.name}!");
            }
        }
        else
        {
            Debug.LogWarning($"Animator hoặc tên animation ({animationName}) không hợp lệ cho {gameObject.name}!");
        }
    }

    // Chuyển đổi trạng thái ngẫu nhiên
    private void SwitchStateRandomly()
    {
        // Chọn ngẫu nhiên giữa Moving và Idle
        currentState = (Random.Range(0, 2) == 0) ? AnimalState.Moving : AnimalState.Idle;
    }

    // Đặt thời gian cho lần chuyển trạng thái tiếp theo
    private void SetNextStateChangeTime()
    {
        timeToNextStateChange = Random.Range(minWaitTime, maxWaitTime);
    }
}