using UnityEngine;

[System.Serializable]
public class GameData
{
    public float posX, posY; // Lưu vị trí nhân vật

    public GameData(Vector3 position)
    {
        this.posX = position.x;
        this.posY = position.y;
    }
}
