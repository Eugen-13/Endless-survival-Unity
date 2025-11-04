using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    [Header("All Available Upgrades")]
    [SerializeField] private List<UpgradeData> _allUpgrades;

    [Header("References")]
    [SerializeField] private GameObject _upgradePanel;
    [SerializeField] private UpgradeButton[] _upgradeButtons; 

    private Dictionary<UpgradeData, int> _upgradelevels = new Dictionary<UpgradeData, int>();
    private Player _player;

    void Awake()
    {
        Instance = this;

        foreach (var upgrade in _allUpgrades)
        {
            _upgradelevels[upgrade] = 0;
        }
    }

    void Start()
    {
        _player = Player.Instance;
        _upgradePanel.SetActive(false);
    }

    public void ShowUpgradeChoices()
    {
     
        Time.timeScale = 0f;
        _upgradePanel.SetActive(true);

       
        List<UpgradeData> availableUpgrades = GetAvailableUpgrades();
        List<UpgradeData> randomUpgrades = GetRandomUpgrades(availableUpgrades, 3);

    
        for (int i = 0; i < _upgradeButtons.Length; i++)
        {
            if (i < randomUpgrades.Count)
            {
                _upgradeButtons[i].SetupButton(randomUpgrades[i], _upgradelevels[randomUpgrades[i]]);
                _upgradeButtons[i].gameObject.SetActive(true);
            }
            else
            {
                _upgradeButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private List<UpgradeData> GetAvailableUpgrades()
    {
        List<UpgradeData> available = new List<UpgradeData>();

        foreach (var upgrade in _allUpgrades)
        {
 
            if (_upgradelevels[upgrade] < upgrade.maxLevel)
            {
                available.Add(upgrade);
            }
        }

        return available;
    }

    private List<UpgradeData> GetRandomUpgrades(List<UpgradeData> available, int count)
    {
        if (available.Count <= count)
            return available;


        return available.OrderBy(x => Random.value).Take(count).ToList();
    }

    public void SelectUpgrade(UpgradeData upgrade)
    {

        _upgradelevels[upgrade]++;


        ApplyUpgrade(upgrade);


        _upgradePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    private void ApplyUpgrade(UpgradeData upgrade)
    {
        switch (upgrade.upgradeType)
        {
            case UpgradeData.UpgradeType.MaxHealth:
                _player.MaxHealth += (int)upgrade.value;
                _player.CurrentHealth += upgrade.value;
                break;

            case UpgradeData.UpgradeType.MoveSpeed:
                _player.Speed += upgrade.value;
                break;

            case UpgradeData.UpgradeType.Damage:
                _player.Damage += upgrade.value;
                break;

            case UpgradeData.UpgradeType.AttackSpeed:
                _player.FireRate -= upgrade.value;
                break;

            case UpgradeData.UpgradeType.NewWeapon:
                break;
        }

    }

    public int GetUpgradeLevel(UpgradeData upgrade)
    {
        return _upgradelevels.ContainsKey(upgrade) ? _upgradelevels[upgrade] : 0;
    }
}