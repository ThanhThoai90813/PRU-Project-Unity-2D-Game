using Inventory.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryData
{
    private int defaultSlot = 10;
    public List<ItemData> itemDatas = new List<ItemData>();
    public InventoryData()
    {
        itemDatas = new List<ItemData>();
        for (int i = 0; i < defaultSlot; i++)
        {
            itemDatas.Add(new ItemData());
        }
    }

    public void SetSlotData(int slot, int itemID, int quantity)
    {
        itemDatas[slot].ItemQuantity = quantity;
        itemDatas[slot].ItemID = itemID;
    }

    public void ResetSlotDataIndex(int slot)
    {
        itemDatas[slot] = new ItemData();
    }

    public void SetItemQuantity(int itemID, int quantity)
    {
        if (IsSlotHaveItemYet(itemID))
        {
            for (int i = 0; i < itemDatas.Count; i++)
            {
                if (itemDatas[i].ItemID == itemID)
                {
                    itemDatas[i].ItemQuantity = quantity;
                }
            }
        }
        else
        {
            int freeSlotIndex = GetFirstFreeSlotIndex();
            SetSlotData(freeSlotIndex, itemID, quantity);
        }
    }

    int GetFirstFreeSlotIndex()
    {
        for (int i = 0; i < itemDatas.Count; i++)
        {
            if (itemDatas[i].ItemID == -1) return i;
        }
        return -1; // Full slot already
    }

    bool IsSlotHaveItemYet(int itemID)
    {
        for (int i = 0; i < itemDatas.Count; i++)
        {
            if (itemDatas[i].ItemID == itemID) return true;
        }
        return false;
    }
}

[Serializable]
public class ItemData
{
    public int ItemID;
    public int ItemQuantity;
    
    public ItemData()
    {
        ItemID = -1;
        ItemQuantity = 0;
    }
}