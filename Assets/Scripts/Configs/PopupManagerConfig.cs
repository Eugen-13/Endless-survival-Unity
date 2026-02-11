using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(menuName = "Configs/PopupManager", fileName = "PopupManagerConfig")]
    public class PopupManagerConfig : ScriptableObject
    {
        [field: SerializeField] public float Offset { get; private set; } = 1.2f;

        [field: SerializeField] public float FadeDuration { get; private set; } = 1f;

        [field: SerializeField] public int PoolSize { get; private set; } = 100;

        [field: SerializeField] public string PoolName { get; private set; } = "DamagePopupPool1";
    }
}