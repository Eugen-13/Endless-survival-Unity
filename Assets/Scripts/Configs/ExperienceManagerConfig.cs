using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ExperienceConfig", menuName = "Configs/ExperienceManagerConfig")]
public class ExperienceManagerConfig : ScriptableObject
{
    public ExperienceCrystal _crystalPrefab;
    public float _chance = 0.3f;
    public int _poolSize = 20;
}

[CreateAssetMenu(fileName = "ExperienceConfig", menuName = "Configs/ExperienceManagerConfig")]
public class PopupManagerConfig : ScriptableObject
{
    public DamagePopup _popupPrefab;
    public float offset = 1.2f;
    public int _poolSize = 20;
}

[CreateAssetMenu(fileName = "ExperienceConfig", menuName = "Configs/ExperienceManagerConfig")]
public class UpgradeManagerConfig : ScriptableObject
{
    public List<UpgradeData> _allUpgrades;
    public GameObject _upgradePanel;
    public UpgradeButton[] _upgradeButtons;
}

[CreateAssetMenu(fileName = "ExperienceConfig", menuName = "Configs/ExperienceManagerConfig")]
public class UIManagerConfig : ScriptableObject
{
    public ExperienceCrystal _crystalPrefab;
    public float _chance = 0.3f;
    public int _poolSize = 20;
}

