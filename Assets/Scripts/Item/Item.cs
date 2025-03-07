using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [field: SerializeField]
    public ItemSO InventoryItem { get; private set; }

    [field: SerializeField]
    public int Quantity { get; set; } = 1;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private float duration = 0.3f;

    private Rigidbody2D rb;
    private bool hasLanded = false;

    private int itemToken;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = InventoryItem.ItemImage;
        itemToken = GenerateToken(InventoryItem.ItemID, transform.position);
        if (DBController.Instance.IsItemCollected(itemToken))
        {
            Destroy(gameObject);
        }
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        rb.gravityScale = 1.0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Giữ cho item không bị xoay khi rơi
    }

    public void DestroyItem()
    {
        GetComponent<Collider2D>().enabled = false;
        DBController.Instance.AddCollectedToken(itemToken);
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }
        StartCoroutine(AnimateItemPickup());

    }

    private IEnumerator AnimateItemPickup()
    {
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;
        float duration = 0.3f;
        float currentTime = 0;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, endScale, currentTime / duration);
            yield return null;
        }

        Destroy(gameObject);
    }

    private int GenerateToken(int id, Vector3 position)
    {
        return id * 100000 + Mathf.RoundToInt(position.x * 1000) + Mathf.RoundToInt(position.y * 1000);
    }
    public int GetToken()
    {
        return itemToken;
    }

}