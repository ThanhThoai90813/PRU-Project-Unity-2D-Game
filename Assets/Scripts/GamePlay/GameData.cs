using UnityEngine;

[System.Serializable]
public class GameData
{
    public float posX, posY; // Lưu vị trí nhân vật
    public int playerHealth;
    public GameData(Vector3 position, int playerHealth)
    {
        this.posX = position.x;
        this.posY = position.y;
        this.playerHealth = playerHealth;
    }
}
