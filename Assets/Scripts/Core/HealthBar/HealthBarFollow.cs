using Managers;
using UnityEngine;
using Zenject;
using IPoolable = Core.ObjectPool.IPoolable;

namespace Core.HealthBar
{
    public class HealthBarFollow : MonoBehaviour, IPoolable
    {
        [SerializeField] private SpriteRenderer _fillRenderer;
        [SerializeField] private Vector3 _offset = new Vector3(0, 0.8f, 0);
        private Vector2 _baseScale;

        private Vector3 _currentOffset;
        private Transform _target;
        private Transform _fillTransform;
        private IHealth _healthSource;
        private string _poolName;

        [Inject] private PoolManager _poolManager;

        private void Awake()
        {
            _baseScale = transform.localScale;
            _fillTransform = _fillRenderer.transform;
            _currentOffset = _offset;
        }

        public void SetHealthSource(IHealth source, float healthBarScale = 1f)
        {
            _currentOffset.y = _offset.y * healthBarScale;
            transform.localScale = new Vector3(_baseScale.x * healthBarScale, _baseScale.y * healthBarScale);

            if (_healthSource != null)
            {
                _healthSource.OnHealthChanged -= UpdateBar;
            }

            _healthSource = source;

            if (_healthSource != null)
            {
                _healthSource.OnHealthChanged += UpdateBar;
                UpdateBar(_healthSource.CurrentHealth, _healthSource.MaxHealth);
            }
        }

        private void UpdateBar(float currentHealth, float maxHealth)
        {
            float ratio = currentHealth / maxHealth;
            _fillTransform.localScale = new Vector3(ratio, 1f, 1f);
        }

        public void SetTarget(Transform target) => _target = target;

        private void LateUpdate()
        {
            if (!_target)
            {
                return;
            }

            Vector3 pos = _target.position;
            pos.y += _currentOffset.y;
            transform.position = pos;
            transform.rotation = Quaternion.identity;
        }

        public void SetPoolName(string poolName) => _poolName = poolName;

        public void ReturnToPool()
        {
            _poolManager.Return(_poolName, gameObject);
        }
    }
}