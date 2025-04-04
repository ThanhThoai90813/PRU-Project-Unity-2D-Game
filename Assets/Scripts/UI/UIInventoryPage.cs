using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inventory.UI
{
    public class UIInventoryPage : MonoBehaviour
    {

        [SerializeField]
        private UIInventoryItem itemPrefab;

        [SerializeField]
        private RectTransform contentPanel;

        [SerializeField]
        private UIInventoryDescription itemDescription;

        [SerializeField]
        private MouseFollower mouseFollower;

        [SerializeField]
        private PlayerInput PlayerInput;

        List<UIInventoryItem> listOfUIItems = new List<UIInventoryItem>();

        private int currentlyDraggedItemIndex = -1;

        public event Action<int> OnDescriptionRequested,
                   OnItemActionRequested,
                   OnStartDragging;

        public event Action<int, int> OnSwapItems;

        private void Awake()
        {
            Hide();
            mouseFollower.Toggle(false);
            itemDescription.ResetDescription();
        }

        public void InitializeInventoryUI(int inventorysize)
        {
            for (int i = 0; i < inventorysize; i++)
            {
                UIInventoryItem uiItem =
                    Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
                uiItem.transform.SetParent(contentPanel);
                listOfUIItems.Add(uiItem);
                uiItem.OnItemClicked += HandleItemSelection;
                uiItem.OnItemBeginDrag += HandleBeginDrag;
                uiItem.OnItemDroppedOn += HandleSwap;
                uiItem.OnItemEndDrag += HandleEndDrag;
                uiItem.OnRightMouseBtnClick += HandleShowItemActions;
            }
        }

        internal void UpdateDescription(int itemIndex, Sprite itemImage, string name, string description)
        {
            if (itemIndex < 0 || itemIndex >= listOfUIItems.Count || listOfUIItems[itemIndex] == null)
                return;
            itemDescription.SetDescription(itemImage, name, description);
            DeselectAllItems();
            listOfUIItems[itemIndex].Select();
        }

        public void UpdateData(int itemIndex,
               Sprite itemImage, int itemQuantity)
        {
            if (itemIndex < 0 || itemIndex >= listOfUIItems.Count || listOfUIItems[itemIndex] == null)
                return;

            listOfUIItems[itemIndex].SetData(itemImage, itemQuantity);
        }

        private void HandleShowItemActions(UIInventoryItem inventoryItemUI)
        {
            if (inventoryItemUI == null) return;

            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1)
                return;

            OnItemActionRequested?.Invoke(index);
        }

        private void HandleEndDrag(UIInventoryItem inventoryItemUI)
        {
            if (inventoryItemUI == null) return;
            if (TrashCan.Instance != null && TrashCan.Instance.IsPointerOverTrashCan())
            {
                int index = listOfUIItems.IndexOf(inventoryItemUI);
                if (index != -1)
                {
                    OnItemActionRequested?.Invoke(index); // Gọi sự kiện xóa item
                }
            }
            ResetDraggtedItem();

        }
        private void HandleSwap(UIInventoryItem inventoryItemUI)
        {
            if (inventoryItemUI == null) return;

            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1 || currentlyDraggedItemIndex < 0 || currentlyDraggedItemIndex >= listOfUIItems.Count)
                return;

            OnSwapItems?.Invoke(currentlyDraggedItemIndex, index);
            HandleItemSelection(inventoryItemUI);
        }

        private void ResetDraggtedItem()
        {
            mouseFollower.Toggle(false);
            currentlyDraggedItemIndex = -1;
        }

        private void HandleBeginDrag(UIInventoryItem inventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1)
                return;
            currentlyDraggedItemIndex = index;
            HandleItemSelection(inventoryItemUI);
            OnStartDragging?.Invoke(index);
        }
        public void CreateDraggedItem(Sprite sprite, int quantity)
        {
            mouseFollower.Toggle(true);
            mouseFollower.SetData(sprite, quantity);
        }

        private void HandleItemSelection(UIInventoryItem inventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1)
                return;
            OnDescriptionRequested?.Invoke(index);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            ResetSelection();
            if (PlayerInput != null)
            {
                PlayerInput.enabled = false; 
            }
        }
        public void ResetSelection()
        {
            itemDescription.ResetDescription();
            DeselectAllItems();
        }
        private void DeselectAllItems()
        {
            foreach (UIInventoryItem item in listOfUIItems)
            {
                if (item != null)
                    item.Deselect();
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            ResetDraggtedItem();
            if (PlayerInput != null)
            {
                PlayerInput.enabled = true;
            }
        }

        internal void ResetAllItems()
        {
            foreach (var item in listOfUIItems)
            {
                if (item != null)
                {
                    item.ResetData();
                    item.Deselect();
                }         
            }
        }
    }
}