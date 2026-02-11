using UI;
using Zenject;

namespace Managers
{
    public class UIManager
    {
        private UIXpPanel _xpPanel;

        [Inject]
        private void Construct(UIXpPanel xpPanel)
        {
            _xpPanel = xpPanel;
        }

        public void UpdateXp(float current, float required)
        {
            _xpPanel.UpdateXp(current, required);
        }

        public void UpdateLevel(int level)
        {
            _xpPanel.UpdateLevel(level);
        }
    }
}