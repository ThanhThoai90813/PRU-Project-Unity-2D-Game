using UnityEngine;
using System.Collections;

public class NPCSpawnTrigger : MonoBehaviour
{
    public Transform spawnPoint;         // Điểm xuất hiện của NPC
    public GameObject npcPrefab;         // Prefab của NPC
    public Transform landingPoint;       // Điểm đáp của NPC
    private bool hasSpawned = false;     // Kiểm tra xem NPC đã được sinh ra chưa
    private PlayerController playerController; // Tham chiếu đến PlayerController
    private GameObject npcInstance;      // Instance của NPC được tạo ra

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
            EnableFairy(npcInstance); // Kích hoạt Fairy
            LockPlayer(); // Khóa hành động của player
            StartCoroutine(FairyMovement(npcInstance));
        }
    }

    private void EnableFairy(GameObject fairy)
    {
        fairy.SetActive(true);
    }

    private IEnumerator FairyMovement(GameObject npc)
    {
        float duration = 3f; // Thời gian bay lượn
        float elapsedTime = 0f;
        Vector3 startPos = npc.transform.position;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float xOffset = Mathf.Sin(Time.time * 3) * 0.5f; // Di chuyển qua lại
            float yOffset = Mathf.Sin(Time.time * 2) * 0.3f; // Nhấp nhô lên xuống
            npc.transform.position = startPos + new Vector3(xOffset, yOffset, 0);
            yield return null;
        }

        // Sau khi bay lượn xong, NPC đáp xuống
        float fallDuration = 2f;
        elapsedTime = 0f;
        Vector3 endPos = landingPoint.position;

        while (elapsedTime < fallDuration)
        {
            elapsedTime += Time.deltaTime;
            npc.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / fallDuration);
            yield return null;
        }

        // Mở khóa hành động của player sau khi NPC đáp xuống
        UnlockPlayer();
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
}