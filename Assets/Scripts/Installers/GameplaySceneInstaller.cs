using Collectables;
using Configs;
using Core.HealthBar;
using DG.Tweening;
using Managers;
using PlayerSystem;
using UI;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GameplaySceneInstaller : MonoInstaller
    {
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private ExperienceCrystal _crystalPrefab;
        [SerializeField] private DamagePopup _popupPrefab;
        [SerializeField] private HealthBarFollow _healthBarPlayerPrefab;

        [SerializeField] private GameObject _hitEffectPrefab;
        [SerializeField] private Transform _firePoint;
        [SerializeField] private Transform _poolsParent;

        [SerializeField] private UIXpPanel _xpPanel;

        public override void InstallBindings()
        {
            DOTween.SetTweensCapacity(1000, 200);

            BindConfigs();

            Container.Bind<EnemyManager>().AsSingle().NonLazy();
            Container.Bind<GameManager>().AsSingle().NonLazy();
            Container.Bind<UIManager>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<PopupManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PoolManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<ExperienceManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<EnemyController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PlayerMovement>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PlayerShooting>().AsSingle().NonLazy();

            Container.Bind<Player>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<UpgradeManager>().FromComponentInHierarchy().AsSingle().NonLazy();

            Container.Bind<Projectile>().FromInstance(_projectilePrefab).AsSingle().NonLazy();
            Container.Bind<ExperienceCrystal>().FromInstance(_crystalPrefab).AsSingle().NonLazy();
            Container.Bind<DamagePopup>().FromInstance(_popupPrefab).AsSingle().NonLazy();
            Container.Bind<UIXpPanel>().FromInstance(_xpPanel).AsSingle().NonLazy();

            Container.Bind<HealthBarFollow>().WithId("HealthBarPlayerPrefab").FromInstance(_healthBarPlayerPrefab).AsCached().NonLazy();
            Container.Bind<GameObject>().WithId("HitEffectPrefab").FromInstance(_hitEffectPrefab).AsCached().NonLazy();
            Container.Bind<Transform>().WithId("FirePoint").FromInstance(_firePoint).AsCached().NonLazy();
            Container.Bind<Transform>().WithId("PoolsParent").FromInstance(_poolsParent).AsCached().NonLazy();
        }

        private void BindConfigs()
        {
            ExperienceManagerConfig experienceManagerConfig = Resources.Load<ExperienceManagerConfig>("Configs/ExperienceManagerConfig");
            PopupManagerConfig popupManagerConfig = Resources.Load<PopupManagerConfig>("Configs/PopupManagerConfig");
            PlayerStatsConfig playerStatsConfig = Resources.Load<PlayerStatsConfig>("Configs/PlayerStatsConfig");
            PlayerLevelConfig playerLevelConfig = Resources.Load<PlayerLevelConfig>("Configs/PlayerLevelConfig");

            Container.Bind<ExperienceManagerConfig>().FromInstance(experienceManagerConfig).AsSingle();
            Container.Bind<PopupManagerConfig>().FromInstance(popupManagerConfig).AsSingle();
            Container.Bind<PlayerStatsConfig>().FromInstance(playerStatsConfig).AsSingle();
            Container.Bind<PlayerLevelConfig>().FromInstance(playerLevelConfig).AsSingle();
        }
    }
}