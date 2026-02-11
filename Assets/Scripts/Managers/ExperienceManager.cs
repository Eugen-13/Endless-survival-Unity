using Collectables;
using Configs;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class ExperienceManager : IInitializable
    {
        private ExperienceCrystal _crystalPrefab;
        private PoolManager _poolManager;
        private ExperienceManagerConfig _config;

        private float _chance;
        private float _magnetSpeed;
        private int _poolSize;
        private string _poolName;

        [Inject]
        private void Construct(ExperienceCrystal crystalPrefab,  PoolManager poolManager, ExperienceManagerConfig config)
        {
            _crystalPrefab = crystalPrefab;
            _poolManager = poolManager;
            _config = config;
        }

        public void Initialize()
        {
            _chance = _config.Chance;
            _magnetSpeed = _config.MagnetSpeed;
            _poolSize = _config.PoolSize;
            _poolName = _config.PoolName;

            _poolManager.CreatePool(_poolName, _crystalPrefab.gameObject, _poolSize);
        }

        public void TrySpawnCrystal(int exp, Vector3 position, Vector2 scale)
        {
            float random = Random.Range(0.0f, 1.0f);
            if (random <= _chance)
            {
                GameObject crystal = _poolManager.Get(_poolName, position, Quaternion.identity);
                crystal.GetComponent<ExperienceCrystal>().Init(exp, _magnetSpeed);
            }
        }
    }
}