using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(menuName = "Configs/ExperienceManager", fileName = "ExperienceManagerConfig")]
    public class ExperienceManagerConfig : ScriptableObject
    {
        [field: SerializeField] public float Chance { get; private set; } = 0.9f;
        [field: SerializeField] public float MagnetSpeed { get; private set; } = 10f;
        [field: SerializeField] public int PoolSize { get; private set; } = 20;
        [field: SerializeField] public string PoolName { get; private set; } = "CrystalPool1";
    }
}