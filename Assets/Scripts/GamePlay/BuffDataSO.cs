using UnityEngine;

[CreateAssetMenu(fileName = "BuffData", menuName = "Buffs/BuffData")]
public class BuffDataSO : ScriptableObject
{
    public string buffName;
    public Sprite buffIcon;
    public float defaultDuration;
    public Color iconColor = Color.white;
    public string description;
}