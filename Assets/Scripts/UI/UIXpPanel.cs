using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class UIXpPanel : MonoBehaviour
    {
        [SerializeField] private Image _xpBar;
        [SerializeField] private TMP_Text _levelText;

        public void UpdateXp(float current, float required)
        {
            _xpBar.fillAmount = current / required;
        }

        public void UpdateLevel(int level)
        {
            _levelText.text = level.ToString();
        }
    }
}