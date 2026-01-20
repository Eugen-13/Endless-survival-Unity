using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(menuName = "Configs/PlayerStats", fileName = "PlayerStatsConfig")]
    public class PlayerStatsConfig : ScriptableObject
    {
        [field: SerializeField] public float FireRate { get; private set; } = 1.6f;
        [field: SerializeField] public int ProjectileCount { get; private set; } = 1;
        [field: SerializeField] public float BulletSpeed { get; private set; }  = 15f;
        [field: SerializeField] public float Damage { get; private set; }  = 5;
        [field: SerializeField] public float Speed { get; private set; }  = 7f;
        [field: SerializeField] public float MaxHealth { get; private set; }  = 100;
        [field: SerializeField] public float PickUpRadius { get; private set; }  = 2f;
        [field: SerializeField] public float DetectionRadius { get; private set; }  = 10f;
    }
}