using System.Collections.Generic;
using System.Linq;
using PlayerSystem;
using UI;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class UpgradeManager : MonoBehaviour
    {
        [Header("All Available Upgrades")]
        [SerializeField] private List<UpgradeData> _allUpgrades;
        
        [Header("References")]
        [SerializeField] private GameObject _upgradePanel;
        [SerializeField] private UpgradeButton[] _upgradeButtons; 

        private readonly Dictionary<UpgradeData, int> _upgradeLevels = new();
        private Player _player;

        [Inject]
        private void Construct(Player player)
        {
            _player = player;
        }
        
        private void Awake()
        {
            foreach (UpgradeData upgrade in _allUpgrades)
            {
                _upgradeLevels[upgrade] = 0;
            }
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
                    _upgradeButtons[i].SetupButton(randomUpgrades[i], _upgradeLevels[randomUpgrades[i]]);
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
            return _allUpgrades.Where(upgrade => _upgradeLevels[upgrade] < upgrade.maxLevel).ToList();
        }

        private List<UpgradeData> GetRandomUpgrades(List<UpgradeData> available, int count)
        {
            return available.Count <= count ? available : available.OrderBy(x => Random.value).Take(count).ToList();
        }

        public void SelectUpgrade(UpgradeData upgrade)
        {
            _upgradeLevels[upgrade]++;
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
    }
}