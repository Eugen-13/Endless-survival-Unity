using Managers;
using PlayerSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class UpgradeButton : MonoBehaviour
    {
        [FormerlySerializedAs("iconImage")]
        [Header("UI References")]
        [SerializeField] private Image _iconImage;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private Button _button;

        private UpgradeData currentUpgrade;
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
            currentUpgrade = upgrade;

            _iconImage.sprite = upgrade.icon;
            _nameText.text = upgrade.upgradeName;

            string levelInfo = currentLevel > 0 ? $" (Ур. {currentLevel} → {currentLevel + 1})" : " (Новое!)";
            _descriptionText.text = upgrade.description + "\n+" + upgrade.value + levelInfo;

            if (currentLevel > 0)
            {
                _levelText.text = $"Уровень: {currentLevel}/{upgrade.maxLevel}";
                _levelText.gameObject.SetActive(true);
            }
            else
            {
                _levelText.gameObject.SetActive(false);
            }
        }

        private void OnButtonClick()
        {
            if (currentUpgrade != null)
            {
                _upgradeManager.SelectUpgrade(currentUpgrade);
            }
        }
    }
}