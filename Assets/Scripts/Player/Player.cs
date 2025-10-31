using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player: MonoBehaviour
{
    public static Player Instance;

    [SerializeField] private float _speed = 10f;
    [SerializeField] private int _health = 100;

    [Header("Level Settings")]
    [SerializeField] private int _level = 1;
    [SerializeField] private int _currentExp = 0;
    [SerializeField] private int _expToNextLevel = 100;
    [SerializeField] private float _nextLevelMultip = 1.5f;
    [SerializeField] private float _pickUpRadius = 2f;

    public float PickUpRadius => _pickUpRadius;

    private InputSystem _inputSystem;
    private Vector2 _direction;
    private Rigidbody2D _rb;

    private GameObject _healthBar;

    [SerializeField] private GameObject _HealthBarPrefab;
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
        _inputSystem = new InputSystem();
        _inputSystem.Enable();

        _rb = GetComponent<Rigidbody2D>();
        _rb.interpolation = RigidbodyInterpolation2D.Interpolate;


        _healthBar = Instantiate(_HealthBarPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
        _healthBar.GetComponent<HealthBarFollow>().SetTarget(transform);
        _healthBar.GetComponent<HealthBarFollow>().SetMaxHealth(_health);

 
    }
    public void TakeDamage(float amount)
    {
        if (_healthBar.GetComponent<HealthBarFollow>().TakeDamageToDie(amount))
            GameManager.Instance.Restart();
    }

    void Update()
    {
        _direction = _inputSystem.Player.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        Move();

    }
    void Move()
    {
        Vector2 newPos = _rb.position + _direction * _speed * Time.fixedDeltaTime;
        _rb.MovePosition(newPos);
        if (_direction.sqrMagnitude > 0.01f){
            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg - 90f;
            _rb.MoveRotation(angle);
        }
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
        _level++;
        _currentExp -= _expToNextLevel;
        _expToNextLevel = Mathf.RoundToInt(_expToNextLevel * _nextLevelMultip);
        UIManager.Instance.UpdateLevel(_level);
        UIManager.Instance.UpdateXP(_currentExp, _expToNextLevel);
    }

}
