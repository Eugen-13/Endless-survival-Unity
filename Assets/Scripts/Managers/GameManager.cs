using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager
    {
        private readonly EnemyManager _enemyManager;
        private readonly ExperienceManager _experienceManager;
        private readonly PopupManager _popupManager;
        private readonly UpgradeManager _upgradeManager;
        private readonly UIManager _uiManager;

        public GameManager(EnemyManager enemyManager, ExperienceManager experienceManager, PopupManager popupManager, UpgradeManager upgradeManager, UIManager uiManager)
        {
            _enemyManager = enemyManager;
            _experienceManager = experienceManager;
            _popupManager = popupManager;
            _upgradeManager = upgradeManager;
            _uiManager = uiManager;
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            _enemyManager.Clear();
        }

        public void AddExperience(int currentExp, int expToNextLevel)
        {
            _uiManager.UpdateXp(currentExp, expToNextLevel);
        }

        public void LevelUp(int level)
        {
            _upgradeManager.ShowUpgradeChoices();
            _uiManager.UpdateLevel(level);
        }

        public void TrySpawnCrystal(int expCount, Vector3 position, Vector2 scale)
        {
            _experienceManager.TrySpawnCrystal(expCount, position, scale);
        }

        public void ShowPopup(int damage, Vector3 position, Color color)
        {
            _popupManager.ShowPopup(damage, position, color);
        }
    }
}
