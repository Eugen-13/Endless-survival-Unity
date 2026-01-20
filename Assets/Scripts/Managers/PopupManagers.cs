using Collectables;
using Configs;
using UI;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class PopupManager : IInitializable
    {
        private DamagePopup _popupPrefab;
        private PoolManager _poolManager;
        private PopupManagerConfig _config;
        
        private float _offset;
        private float _fadeDuration;
        private int _poolSize;
        private string _poolName;
        
        [Inject]
        private void Construct(DamagePopup popupPrefab,  PoolManager poolManager, PopupManagerConfig config)
        {
            _popupPrefab = popupPrefab;
            _poolManager = poolManager;
            _config = config; 
        }
        
        public void Initialize()
        {
            _offset = _config.Offset;
            _fadeDuration = _config.FadeDuration;
            _poolSize = _config.PoolSize;
            _poolName = _config.PoolName;
            
            _poolManager.CreatePool(_poolName, _popupPrefab.gameObject, _poolSize);
        }
        
        public void ShowPopup(int damage, Vector3 position, Color color)
        {
            _poolManager.Get(_poolName, position + Vector3.up * _offset, Quaternion.identity)
                .GetComponent<DamagePopup>().Show("-" + damage, color, _fadeDuration);
        }
    }
}