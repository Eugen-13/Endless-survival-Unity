using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeButton : MonoBehaviour
{
    [Header("UI References")]
    public Image iconImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI levelText;
    public Button button;

    private UpgradeData currentUpgrade;

    void Awake()
    {
        button.onClick.AddListener(OnButtonClick);
    }

    public void SetupButton(UpgradeData upgrade, int currentLevel)
    {
        currentUpgrade = upgrade;

        iconImage.sprite = upgrade.icon;
        nameText.text = upgrade.upgradeName;

        string levelInfo = currentLevel > 0 ? $" (Ур. {currentLevel} → {currentLevel + 1})" : " (Новое!)";
        descriptionText.text = upgrade.description + "\n+" + upgrade.value + levelInfo;

        if (currentLevel > 0)
        {
            levelText.text = $"Уровень: {currentLevel}/{upgrade.maxLevel}";
            levelText.gameObject.SetActive(true);
        }
        else
        {
            levelText.gameObject.SetActive(false);
        }
    }

    void OnButtonClick()
    {
        if (currentUpgrade != null)
        {
            UpgradeManager.Instance.SelectUpgrade(currentUpgrade);
        }
    }
}