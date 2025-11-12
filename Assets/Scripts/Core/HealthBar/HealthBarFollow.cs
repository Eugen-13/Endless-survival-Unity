using System.Linq;
using UnityEngine;

public class HealthBarFollow : MonoBehaviour, IPoolable
{
    [SerializeField] private SpriteRenderer _fillRenderer;
    [SerializeField] private Vector3 _offset = new Vector3(0, 0.8f, 0);
    private Transform _target;
    private Transform _fillTransform;
    private IHealth _healthSource;
    private string _poolName;

    void Awake()
    {
        _fillTransform = _fillRenderer.transform;
    }


    public void SetHealthSource(IHealth source)
    {
        if (_healthSource != null)
            _healthSource.OnHealthChanged -= UpdateBar;

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
        _fillTransform.localScale = new Vector3(ratio, 1, 1);
    }

    public void SetTarget(Transform target) => _target = target;


    void LateUpdate()
    {
        if (_target == null) return;

        Vector3 pos = _target.position;
        pos.y += _offset.y;
        transform.position = pos;
        transform.rotation = Quaternion.identity;
    }

    public void SetPoolName(string poolName) => _poolName = poolName;
    public void ReturnToPool()
    {
        PoolManager.Instance.Return(_poolName, gameObject);
    }
}