using UnityEngine;
using System.Collections;

public class NPCSpawnTrigger : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject npcPrefab;
    public Transform landingPoint; // Điểm đáp của NPC
    private bool hasSpawned = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasSpawned)
        {
            hasSpawned = true;
            GameObject npc = Instantiate(npcPrefab, spawnPoint.position, Quaternion.identity);
            EnableFairy(npc); // Kích hoạt Fairy
            StartCoroutine(FairyMovement(npc));
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
    }
}
