using System;
using Configs;
using Core.HealthBar;
using Managers;
using UnityEngine;
using Zenject;

namespace PlayerSystem
{
    public class Player : MonoBehaviour, IHealth
    {
        private float _fireRate;
        private int _projectileCount;
        private float _bulletSpeed;
        private float _damage;
        private float _speed;
        private float _maxHealth;
        private float _currentHealth;
        private float _pickUpRadius;
        private float _detectionRadius;

        private int _level;
        private int _currentExp;
        private int _expToNextLevel;
        private float _nextLevelMultip;
        
        private HealthBarFollow _healthBar;
        
        
        private HealthBarFollow _healthBarPrefab;
        private GameManager _gameManager;
        
        private PlayerMovement _playerMovement; 
        private PlayerShooting _playerShooting;
        
        private PlayerStatsConfig _playerStatsConfig;
        private PlayerLevelConfig _playerLevelConfig;

        [Inject]
        private void Construct(
            [Inject(Id = "HealthBarPlayerPrefab")]HealthBarFollow healthBarPrefab,
            GameManager gameManager, 
            PlayerMovement playerMovement, 
            PlayerShooting playerShooting,
            PlayerStatsConfig playerStatsConfig, 
            PlayerLevelConfig playerLevelConfig)
        {
            _healthBarPrefab = healthBarPrefab;
            _gameManager = gameManager;
            _playerMovement = playerMovement;
            _playerShooting = playerShooting;
            _playerStatsConfig = playerStatsConfig;
            _playerLevelConfig = playerLevelConfig;
        }
        
        public float PickUpRadius => _pickUpRadius;
        public float DetectionRadius => _detectionRadius;
        public int Level => _level;
        public float Speed { get => _speed; set => _speed = value; }
        public float MaxHealth { get => _maxHealth; set => _maxHealth = value; }
        public float CurrentHealth { get => _currentHealth; set => _currentHealth = value; }
        public float FireRate { get => _fireRate; set => _fireRate = value; }
        public float BulletSpeed { get => _bulletSpeed; set => _bulletSpeed = value; }
        public int ProjectileCount { get => _projectileCount; set => _projectileCount = value; }
        public float Damage { get => _damage; set => _damage = value; }
        
        
        public event Action<int> OnLevelUp;
        public event Action<int, int> OnExperienceGained; 
        public event Action<float, float> OnHealthChanged;

        

        private void Start()
        {
            InitStats();
            InitLevel();
            
            OnLevelUp += _gameManager.LevelUp;
            OnExperienceGained += _gameManager.AddExperience;
            
            _currentHealth = MaxHealth;

            _healthBar = Instantiate(_healthBarPrefab, transform.position + new Vector3(0, 0.8f, 0),
                Quaternion.identity);
            
            _healthBar.SetTarget(transform);
            _healthBar.SetHealthSource(this);
        }
        
        private void OnDisable()
        {
            OnLevelUp -= _gameManager.LevelUp;
            OnExperienceGained -= _gameManager.AddExperience;
        }

        private void InitStats()
        {
            _fireRate = _playerStatsConfig.FireRate;
            _projectileCount =  _playerStatsConfig.ProjectileCount;
            _bulletSpeed = _playerStatsConfig.BulletSpeed;
            _damage = _playerStatsConfig.Damage;
            _speed = _playerStatsConfig.Speed;
            _maxHealth = _playerStatsConfig.MaxHealth;
            _pickUpRadius = _playerStatsConfig.PickUpRadius;
            _detectionRadius = _playerStatsConfig.DetectionRadius;
        }
        
        private void InitLevel()
        {
            _level = _playerLevelConfig.Level;
            _currentExp = _playerLevelConfig.CurrentExp;
            _expToNextLevel = _playerLevelConfig.ExpToNextLevel;
            _nextLevelMultip = _playerLevelConfig.NextLevelMultip;
        }

        public void AddExperience(int amount)
        {
            _currentExp += amount;
            
            OnExperienceGained?.Invoke(_currentExp, _expToNextLevel);
            
            if (_currentExp >= _expToNextLevel)
            {
                LevelUp();
            }
        }
        private void LevelUp()
        {
            _level++;
            _currentExp -= _expToNextLevel;
            _expToNextLevel = Mathf.RoundToInt(_expToNextLevel * _nextLevelMultip);
            
            OnLevelUp?.Invoke(_level);
            OnExperienceGained?.Invoke(_currentExp, _expToNextLevel);
        }

        public void TakeDamage(float amount)
        {
            _currentHealth -= amount;
            _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);

            if (_currentHealth <= 0)
                _gameManager.Restart();
        }

        public void Heal(float amount)
        {
            _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);
            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
        }
    }
}
