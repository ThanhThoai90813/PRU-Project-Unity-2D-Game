using UnityEngine;
using System.Collections;

public class NPCChasePlayerTrigger : MonoBehaviour
{
    public Transform spawnPoint;         // Điểm xuất hiện của NPC
    public GameObject npcPrefab;         // Prefab của NPC
    public float moveSpeed = 2f;         // Tốc độ di chuyển của NPC
    public float stopDistance = 2f;      // Khoảng cách tối thiểu NPC giữ với player
    private GameObject npcInstance;      // Instance của NPC được tạo ra
    private bool hasSpawned = false;     // Kiểm tra xem NPC đã được sinh ra chưa
    private PlayerController playerController; // Tham chiếu đến PlayerController

    private void Start()
    {
        // Tìm và lưu tham chiếu đến PlayerController
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
        }
        else
        {
            Debug.LogWarning("Player not found! Please ensure the player has the 'Player' tag.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasSpawned && playerController != null)
        {
            hasSpawned = true;
            npcInstance = Instantiate(npcPrefab, spawnPoint.position, Quaternion.identity);
            EnableNPC(npcInstance); // Kích hoạt NPC
            StartCoroutine(NPCChaseMovement());
            LockPlayer(); // Khóa hành động của player
        }
    }

    private void EnableNPC(GameObject npc)
    {
        npc.SetActive(true);
    }

    private IEnumerator NPCChaseMovement()
    {
        if (npcInstance == null || playerController == null)
        {
            Debug.LogWarning("NPC or Player not found!");
            yield break;
        }

        Transform target = playerController.transform;
        Vector3 startPos = npcInstance.transform.position;

        while (true)
        {
            // Tính toán vị trí đích cách player một khoảng stopDistance
            Vector3 directionToPlayer = (target.position - npcInstance.transform.position).normalized;
            Vector3 targetPosition = target.position - directionToPlayer * stopDistance;

            // Di chuyển NPC về vị trí đích
            float distanceToTarget = Vector3.Distance(npcInstance.transform.position, targetPosition);
            if (distanceToTarget > 0.1f)
            {
                npcInstance.transform.position = Vector3.MoveTowards(
                    npcInstance.transform.position,
                    targetPosition,
                    moveSpeed * Time.deltaTime
                );
            }
            else
            {
                // Khi NPC đã ở gần đúng khoảng cách, dừng lại
                npcInstance.transform.position = targetPosition;
                UnlockPlayer(); // Mở khóa hành động của player
                yield break; // Kết thúc coroutine
            }

            yield return null;
        }
    }

    private void LockPlayer()
    {
        if (playerController != null && playerController.IsALive)
        {
            playerController.animator.SetBool("canMove", false); // Khóa hành động của player
        }
    }

    private void UnlockPlayer()
    {
        if (playerController != null && playerController.IsALive)
        {
            playerController.animator.SetBool("canMove", true); // Mở khóa hành động của player
        }
    }

    // Optional: Cập nhật vị trí theo player
    private void Update()
    {
        if (npcInstance != null && hasSpawned)
        {
            // Có thể thêm hiệu ứng bay lượn nhẹ
            float xOffset = Mathf.Sin(Time.time * 3) * 0.2f;
            float yOffset = Mathf.Cos(Time.time * 2) * 0.2f;
            npcInstance.transform.position += new Vector3(xOffset, yOffset, 0) * Time.deltaTime;
        }
    }
}