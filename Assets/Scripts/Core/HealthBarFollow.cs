using System.Linq;
using UnityEngine;

public class HealthBarFollow : MonoBehaviour, IPoolable
{
    [SerializeField] private SpriteRenderer _fillRenderer;
    [SerializeField] private Vector3 _offset = new Vector3(0, 0.8f, 0);
    private Transform _target;
    private Transform _fillTransform; // Кэшируем transform
    private float _maxHealth = 100f;
    private float _currentHealth;
    private string _poolName;

    void Awake()
    {
        _fillTransform = _fillRenderer.transform;
    }

    void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void SetMaxHealth(float maxHealth) => _maxHealth = maxHealth;
    public void SetTarget(Transform target) => _target = target;

    public bool TakeDamageToDie(float amount)
    {
        _currentHealth -= amount;

        if (_currentHealth <= 0)
            return true;

        _currentHealth = Mathf.Max(_currentHealth, 0);
        UpdateBar();
        return false;
    }


    public void Heal(float amount)
    {
        _currentHealth += amount;
        _currentHealth = Mathf.Min(_currentHealth, 100);
        UpdateBar();
    }
    private void UpdateBar()
    {
        float ratio = _currentHealth / _maxHealth;
        _fillTransform.localScale = new Vector3(ratio, 1, 1);
    }

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