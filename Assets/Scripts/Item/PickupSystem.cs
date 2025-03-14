using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSystem : MonoBehaviour
{
    [SerializeField]
    private InventorySO inventoryData;
    private Item nearbyItem;

    [SerializeField] 
    private GameObject pickUpText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Player entered trigger of item: " + collision.name);
        Item item = collision.GetComponent<Item>();
        if (item != null)
        {
            nearbyItem = item;
            if (pickUpText != null)
            {
                pickUpText.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Item item) && item == nearbyItem)
        {
            nearbyItem = null;
            if (pickUpText != null)
            {
                pickUpText.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && nearbyItem != null)
        {
            PickUpItem(nearbyItem);
        }
    }
    private void PickUpItem(Item item)
    {
        int remainder = inventoryData.AddItem(item.InventoryItem, item.Quantity);
        if (remainder == 0)
        {
            item.DestroyItem();
        }
        else
        {
            item.Quantity = remainder;
        }
        nearbyItem = null;

        if (pickUpText != null)
        {
            pickUpText.SetActive(false);
        }
    }
}