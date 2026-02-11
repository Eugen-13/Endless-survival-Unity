using Managers;
using PlayerSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class UpgradeButton : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Image _iconImage;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private Button _button;

        private UpgradeData _currentUpgrade;
        private UpgradeManager _upgradeManager;

        [Inject]
        public void Construct(UpgradeManager upgradeManager)
        {
            _upgradeManager = upgradeManager;
        }

        private void Awake()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        public void SetupButton(UpgradeData upgrade, int currentLevel)
        {
            _currentUpgrade = upgrade;

            _iconImage.sprite = upgrade.Icon;
            _nameText.text = upgrade.UpgradeName;

            string levelInfo = currentLevel > 0 ? $" (Ур. {currentLevel} → {currentLevel + 1})" : " (Новое!)";
            _descriptionText.text = upgrade.Description + "\n+" + upgrade.Value + levelInfo;

            if (currentLevel > 0)
            {
                _levelText.text = $"Уровень: {currentLevel}/{upgrade.MaxLevel}";
                _levelText.gameObject.SetActive(true);
            }
            else
            {
                _levelText.gameObject.SetActive(false);
            }
        }

        private void OnButtonClick()
        {
            if (_currentUpgrade != null)
            {
                _upgradeManager.SelectUpgrade(_currentUpgrade);
            }
        }
    }
}