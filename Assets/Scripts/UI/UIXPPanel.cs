using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIXPPanel : MonoBehaviour
{
    [SerializeField] private Image xpBar;
    [SerializeField] private TMP_Text levelText;

    public void UpdateXP(float current, float required)
    {
        xpBar.fillAmount = current / required;
    }

    public void UpdateLevel(int level)
    {
        levelText.text = level.ToString();
    }
}