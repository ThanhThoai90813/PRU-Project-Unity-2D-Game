using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class EdibleItemSO : ItemSO, IDestroyableItem, IItemAction
    {
        [SerializeField]
        private List<ModifierData> modifiersData = new List<ModifierData>();

        [SerializeField]
        private bool isConsumable = true;
        public string ActionName => "Consume";

        [field: SerializeField]
        public AudioClip actionSFX { get; private set; }

        public bool PerformAction(GameObject character)
        {
            if (actionSFX != null)
            {
                AudioSource.PlayClipAtPoint(actionSFX, character.transform.position);
            }

            foreach (ModifierData data in modifiersData)
            {
                data.statModifier.AffectChararacter(character, data.value);
            }
            return isConsumable;
        }

        public bool ShouldBeDestroyed()
        {
            return isConsumable;
        }
    }

    public interface IDestroyableItem
    {
        bool ShouldBeDestroyed(); // Xác định xem vật phẩm có bị hủy sau khi sử dụng không

    }

    public interface IItemAction
    {
        public string ActionName { get; }
        public AudioClip actionSFX { get; }
        bool PerformAction(GameObject character);
    }

    [Serializable]
    public class ModifierData
    {
        public CharacterStatModifierSO statModifier;
        public float value;
    }
}