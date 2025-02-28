using Inventory.Model;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public float posX, posY; // Lưu vị trí nhân vật
    public int playerHealth;
    public List<InventoryItem> inventoryItems; //lưu inventory
    public string sceneName;
    public GameData(string sceneName , Vector3 position, int playerHealth, List<InventoryItem> inventoryItem)
    {
        this.sceneName = sceneName;
        this.posX = position.x;
        this.posY = position.y;
        this.playerHealth = playerHealth;
        this.inventoryItems = inventoryItem;
    }
}
