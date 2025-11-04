using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Upgrades/Upgrade")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName;
    public string description;
    public Sprite icon;
    public UpgradeType upgradeType;
    public float value; 
    public int maxLevel = 5; 

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