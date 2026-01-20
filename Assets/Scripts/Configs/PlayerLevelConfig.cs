using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(menuName = "Configs/PlayerLevel", fileName = "PlayerLevelConfig")]
    public class PlayerLevelConfig : ScriptableObject
    {
        [field: SerializeField] public int Level { get; private set; }  = 1;
        [field: SerializeField] public int CurrentExp { get; private set; }  = 0;
        [field: SerializeField] public int ExpToNextLevel { get; private set; }  = 100;
        [field: SerializeField] public float NextLevelMultip { get; private set; }  = 1.5f;
    }
}