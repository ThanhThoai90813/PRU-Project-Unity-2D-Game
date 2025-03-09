using UnityEngine;

[CreateAssetMenu(fileName = "HealthRegenItem", menuName = "Items/HealthRegenItem")]
public class CharacterStatHealthRegenModifierSO : CharacterStatModifierSO
{
    [SerializeField]
    private float duration = 10f; 
    [SerializeField]
    private int healthPerSecond = 1; 
    [SerializeField]
    private BuffDataSO buffData;
    public override void AffectChararacter(GameObject character, float val)
    {
        PlayerController player = character.GetComponent<PlayerController>();
        if (player != null && player.IsALive) 
        {
            Damageable damageable = character.GetComponent<Damageable>();
            if (damageable != null)
            {
                player.StartCoroutine(ApplyHealthRegen(damageable, duration, healthPerSecond));
                BuffUIManager.Instance.AddBuffUI(buffData, duration);
            }
            else
            {
                Debug.LogWarning("Damageable component not found on the character!");
            }
        }
        else
        {
            Debug.LogWarning("PlayerController not found or character is not alive!");
        }
    }

    private System.Collections.IEnumerator ApplyHealthRegen(Damageable damageable, float duration, int healthPerSecond)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration && damageable.IsAlive && damageable.Health < damageable.MaxHealth)
        {
            // Hồi máu 1 HP mỗi giây
            if (damageable.Heal(healthPerSecond))
            {
                Debug.Log($"Healed {healthPerSecond} HP. Current HP: {damageable.Health}");
            }

            elapsedTime += 1f; // Tăng thời gian theo từng giây
            yield return new WaitForSeconds(1f); // Chờ 1 giây trước khi hồi lại
        }

        Debug.Log("Health regen buff expired.");
    }
}