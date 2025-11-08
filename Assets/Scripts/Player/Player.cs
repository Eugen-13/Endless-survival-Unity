using System;
using UnityEngine;

public class Player: MonoBehaviour, IHealth
{
    public static Player Instance;

    [Header("Base stats")]
    [SerializeField] private float _fireRate = 1.6f;
    [SerializeField] private int _projectileCount = 1;
    [SerializeField] private float _bulletSpeed = 15f;
    [SerializeField] private float _damage = 5;
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _maxHealth = 100;
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _pickUpRadius = 2f;



    public float PickUpRadius => _pickUpRadius;
    public int Level => _level;
    public float Speed { get { return _speed; } set { _speed = value; } }
    public float MaxHealth { get { return _maxHealth; } set { _maxHealth = value; } }
    public float CurrentHealth { get { return _currentHealth; } set { _currentHealth = value; } }
    public float FireRate { get { return _fireRate; } set { _fireRate = value; } }
    public float BulletSpeed { get { return _bulletSpeed; } set { _bulletSpeed = value; } }
    public int ProjectileCount { get { return _projectileCount; } set { _projectileCount = value; } }
    public float Damage { get { return _damage; } set { _damage = value; } }

    [Header("Level Settings")]
    [SerializeField] private int _level = 1;
    [SerializeField] private int _currentExp = 0;
    [SerializeField] private int _expToNextLevel = 100;
    [SerializeField] private float _nextLevelMultip = 1.5f;




    private PlayerMovement _playerMovement; 
    private PlayerShooting _playerShooting;
    
    private HealthBarFollow _healthBar;

    [SerializeField] private GameObject _HealthBarPrefab;

    public event Action<float, float> OnHealthChanged;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    void Start()
    {
        _currentHealth = MaxHealth;
        _playerMovement = GetComponent<PlayerMovement>();
        _playerShooting = GetComponent<PlayerShooting>();

        _healthBar = (Instantiate(_HealthBarPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity)).GetComponent<HealthBarFollow>();
        _healthBar.SetTarget(transform);
        _healthBar.SetHealthSource(this);
    }

    public void AddExperience(int amount)
    {
        _currentExp += amount;
        UIManager.Instance.UpdateXP(_currentExp, _expToNextLevel);
        if (_currentExp >= _expToNextLevel)
        {
            LevelUp();
        }
    }
    private void LevelUp()
    {
        UpgradeManager.Instance.ShowUpgradeChoices();
        _level++;
        _currentExp -= _expToNextLevel;
        _expToNextLevel = Mathf.RoundToInt(_expToNextLevel * _nextLevelMultip);
        UIManager.Instance.UpdateLevel(_level);
        UIManager.Instance.UpdateXP(_currentExp, _expToNextLevel);
    }

    public void TakeDamage(float amount)
    {
        _currentHealth -= amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);

        if (_currentHealth <= 0)
            GameManager.Instance.Restart();
    }

    public void Heal(float amount)
    {
        _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
    }
}
