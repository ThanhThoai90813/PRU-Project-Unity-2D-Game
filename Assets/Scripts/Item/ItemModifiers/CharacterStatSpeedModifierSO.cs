using UnityEngine;

[CreateAssetMenu(fileName = "SpeedBoostItem", menuName = "Items/SpeedBoostItem")]
public class CharacterStatSpeedModifierSO : CharacterStatModifierSO
{
    [SerializeField]
    private float duration = 10f;

    [SerializeField]
    private float speedIncrease = 5f; 

    [SerializeField]
    private BuffDataSO buffData; 

    public override void AffectChararacter(GameObject character, float val)
    {
        PlayerController player = character.GetComponent<PlayerController>();
        if (player != null)
        {
            // Lưu tốc độ ban đầu để khôi phục sau khi buff hết
            float originalWalkSpeed = player.walkSpeed;
            float originalRunSpeed = player.runSpeed;
            float originalAirWalkSpeed = player.airWalkSpeed;

            // Áp dụng buff tốc độ
            player.walkSpeed += speedIncrease;
            player.runSpeed += speedIncrease;
            player.airWalkSpeed += speedIncrease;

            // Hiển thị UI buff
            BuffUIManager.Instance.AddBuffUI(buffData, duration);

            // Khởi động coroutine để khôi phục tốc độ sau khi hết thời gian
            player.StartCoroutine(ApplySpeedBoost(originalWalkSpeed, originalRunSpeed, originalAirWalkSpeed));
        }
        else
        {
            Debug.LogWarning("PlayerController not found on the character!");
        }
    }

    private System.Collections.IEnumerator ApplySpeedBoost(float originalWalkSpeed, float originalRunSpeed, float originalAirWalkSpeed)
    {
        yield return new WaitForSeconds(duration);

        PlayerController player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerController>();
        if (player != null)
        {
            // Khôi phục tốc độ ban đầu
            player.walkSpeed = originalWalkSpeed;
            player.runSpeed = originalRunSpeed;
            player.airWalkSpeed = originalAirWalkSpeed;

            Debug.Log("Speed boost expired. Restored original speeds.");
        }
    }
}