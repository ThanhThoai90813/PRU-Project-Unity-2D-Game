using Inventory.Model;
using Inventory.UI;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField]
        private UIInventoryPage inventoryUI;

        [SerializeField]
        public InventorySO inventoryData;

        public List<InventoryItem> initialItems = new List<InventoryItem>();

        InventoryData inventoryDB;

        //private void Start()
        //{
        //    inventoryDB = DBController.Instance.INVENTORY_DATA;
        //    PrepareUI();
        //    PrepareInventoryData();
        //}

        private void PrepareInventoryData()
        {
            inventoryData.Initialize(inventoryDB);
            inventoryData.OnInventoryUpdated += UpdateInventoryUI;
            inventoryData.OnInventoryUpdated += SaveInventoryDatabase;
            SetInventoryData();
        }

        void SetInventoryData()
        {
            var items = inventoryData.InventoryItems;
            for (int i = 0; i < items.Count; i++)
            {
                initialItems.Add(items[i]);
            }
        }

        private void SaveInventoryDatabase(Dictionary<int, InventoryItem> inventoryState)
        {
            // Reset tất cả slot trong inventoryDB về trạng thái mặc định trước
            for (int i = 0; i < inventoryDB.itemDatas.Count; i++)
            {
                if (inventoryState.TryGetValue(i, out InventoryItem inventoryItem))
                {
                    inventoryDB.SetItemQuantity(inventoryItem.item.ItemID, inventoryItem.quantity);
                    Debug.Log($"[SAVE] Slot {i}: ID {inventoryItem.item.ItemID} - Quantity {inventoryItem.quantity}");
                }
            }
            // Lưu lại vào DBController
            //DBController.Instance.INVENTORY_DATA = inventoryDB;
        }

        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            inventoryUI.ResetAllItems();
            foreach (var item in inventoryState)
            {
                inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage,
                    item.Value.quantity);
            }
        }

        private void PrepareUI()
        {
            inventoryUI.InitializeInventoryUI(inventoryData.Size);
            inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
            inventoryUI.OnSwapItems += HandleSwapItems;
            inventoryUI.OnStartDragging += HandleDragging;
            inventoryUI.OnItemActionRequested += HandleItemActionRequest;
        }

        private void HandleItemActionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;
           
            IItemAction itemAction = inventoryItem.item as IItemAction;
            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;

            bool shouldDestroy = false;

            if (itemAction != null)
            {
                shouldDestroy = itemAction.PerformAction(gameObject);
            }
            if (destroyableItem != null && destroyableItem.ShouldBeDestroyed())
            {
                inventoryData.RemoveItem(itemIndex, 1);
            }
        }

        private void HandleDragging(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;
            inventoryUI.CreateDraggedItem(inventoryItem.item.ItemImage, inventoryItem.quantity);
        }

        private void HandleSwapItems(int itemIndex_1, int itemIndex_2)
        {
            inventoryData.SwapItems(itemIndex_1, itemIndex_2);
        }

        private void HandleDescriptionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                inventoryUI.ResetSelection();
                return;
            }

            ItemSO item = inventoryItem.item;
            inventoryUI.UpdateDescription(itemIndex, item.ItemImage,
                item.name, item.Description);

        }

        public void Update()
        {
            if (Input.GetKeyUp(KeyCode.I))
            {
                if (inventoryUI.isActiveAndEnabled == false)
                {
                    inventoryUI.Show();
                    foreach (var item in inventoryData.GetCurrentInventoryState())
                    {
                        inventoryUI.UpdateData(item.Key,
                            item.Value.item.ItemImage,
                            item.Value.quantity);
                    }
                }
                else
                {
                    inventoryUI.Hide();
                }
            }
        }


        private void OnDestroy()
        {
            inventoryUI.OnDescriptionRequested -= HandleDescriptionRequest;
            inventoryUI.OnSwapItems -= HandleSwapItems;
            inventoryUI.OnStartDragging -= HandleDragging;
            inventoryUI.OnItemActionRequested -= HandleItemActionRequest;
        }

    }
}