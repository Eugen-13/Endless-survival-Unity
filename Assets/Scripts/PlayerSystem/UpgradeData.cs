using UnityEngine;

namespace PlayerSystem
{
    [CreateAssetMenu(fileName = "New Upgrade", menuName = "Upgrades/Upgrade")]
    public class UpgradeData : ScriptableObject
    {
        public string UpgradeName;
        public string Description;
        public Sprite Icon;
        public UpgradeType UpgradeTypeValue;
        public float Value;
        public int MaxLevel = 5;

        public enum UpgradeType
        {
            MaxHealth,
            MoveSpeed,
            Damage,
            AttackSpeed,
            ProjectileCount,
            Range,
            PickupRadius,
            Experience,
            Armor,
            CritChance,
            CritDamage,
            NewWeapon
        }
    }
}