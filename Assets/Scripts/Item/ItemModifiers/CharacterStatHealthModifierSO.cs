using UnityEngine;

[CreateAssetMenu]
public class CharacterStatHealthModifierSO : CharacterStatModifierSO
{
    public override void AffectChararacter(GameObject character, float val)
    {
        Damageable damageable = character.GetComponent<Damageable>();
        if(damageable != null )
            damageable.Heal((int)val); // Gọi phương thức Heal trong Damageable
    }
}
