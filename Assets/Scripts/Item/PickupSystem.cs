using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSystem : MonoBehaviour
{
    [SerializeField]
    private InventorySO inventoryData;
    private Item nearbyItem;

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Item item = collision.GetComponent<Item>();
    //    if (item != null)
    //    {
    //        int reminder = inventoryData.AddItem(item.InventoryItem, item.Quantity);
    //        if (reminder == 0)
    //            item.DestroyItem();
    //        else
    //            item.Quantity = reminder;
    //    }
    //}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item item = collision.GetComponent<Item>();
        if (item != null)
        {
            nearbyItem = item; // Lưu vật phẩm gần nhất
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Item item = collision.GetComponent<Item>();
        if (item != null && item == nearbyItem)
        {
            nearbyItem = null; // Xóa vật phẩm khỏi danh sách nếu người chơi rời khỏi vùng
        }
    }
    private void Update()
    {
        // Kiểm tra nếu người chơi nhấn phím "E" và có vật phẩm gần đó
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
        nearbyItem = null; // Xóa vật phẩm khỏi danh sách sau khi nhặt
    }
}