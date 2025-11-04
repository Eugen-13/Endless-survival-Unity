using DG.Tweening;
using System;
using System.Collections;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class TriangleEnemyBehaviour : PoolableObject, IHealth
{

    [Header("Attack Settings")]
    [SerializeField] private float _attackRange = 1f;
    [SerializeField] private float _attackCooldown = 1f;
    [SerializeField] private float _damage = 10;

    [Header("Movement")]
    [SerializeField] private float _agentSpeed = 5f;
    [SerializeField] private float _repelRadius = 1f;
    [SerializeField] private float _repelForce = 1.5f;

    [SerializeField] private float _maxHealth = 5;
    [SerializeField] private float _currentHealth;

    [SerializeField] private LayerMask _enemyLayer;

    private float _lastAttackTime;
    private bool _isAttacking = false;
    private float _attackRangeSqr;

    private Transform _player;
    private NavMeshAgent _agent;
    private HealthBarFollow _healthBar;
    private Sequence _sequence;
    private Tween _rotationTween;

    public event Action<float, float> OnHealthChanged;

    public NavMeshAgent Agent => _agent;
    public float RepelRadius => _repelRadius;
    public float RepelForce => _repelForce;

    public float MaxHealth { get { return _maxHealth; } set { _maxHealth = value; } }

    public float CurrentHealth => _currentHealth;

    public void Initalice(EnemyStats enemyStats)
    {
        _agentSpeed = enemyStats.Speed;
        _agent.speed = enemyStats.Speed;

        _maxHealth = enemyStats.Health;
        _currentHealth = enemyStats.Health;

        _damage = enemyStats.Damage;
        _attackCooldown = enemyStats.AtackCooldawn;
    }
    public void SetHealthBarSource(HealthBarFollow source)
    {
        _healthBar = source;
        _healthBar.SetHealthSource(this);
    }


    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _agent.radius = 0.5f;
        _agent.speed = _agentSpeed;
        _agent.angularSpeed = 1000f;
        _agent.acceleration = 8f;
        _agent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
    }
    void Start()
    {
        var spriteRender = GetComponent<SpriteRenderer>();
        _sequence = DOTween.Sequence();
        _sequence.Append(transform.DOScale(0.2f, 0.1f));
        _sequence.Append(transform.DOScale(0.12f, 0.2f));
        _sequence.Join(spriteRender.DOColor(Color.red, 0.1f));
        _sequence.Append(spriteRender.DOColor(Color.white, 0.2f))
        .Pause()
        .SetAutoKill(false);

        if (_rotationTween == null || !_rotationTween.IsActive())
        {
            _rotationTween = transform.DORotate(new Vector3(0, 0, 360), 2f, RotateMode.LocalAxisAdd)
                .SetLoops(-1, LoopType.Restart)
                .SetEase(Ease.Linear);
        }


        _player = Player.Instance.transform;

    }
    private void OnEnable()
    {
        _rotationTween?.Play();
        _attackRangeSqr = _attackRange * _attackRange;
        _currentHealth = _maxHealth;
    }
    private void OnDisable()
    {
        _rotationTween?.Pause();
        _sequence?.Pause();
    }

    public void FixedUpdate()
    {
        Behavior();
    }

    public void Behavior()
    {
        if (_player == null) return;

        float sqr = (transform.position - _player.position).sqrMagnitude;

        if (sqr <= _attackRangeSqr)
        {
            _agent.isStopped = true;
        }
        else if (!_isAttacking)
        {
            _agent.isStopped = false;
            _agent.SetDestination(_player.position);
        }

        if (sqr <= _attackRangeSqr && Time.time >= _lastAttackTime + _attackCooldown)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    private IEnumerator AttackRoutine()
    {
        if (_isAttacking)
            yield break;

        _isAttacking = true;

        _sequence.Restart();
        yield return _sequence.WaitForCompletion();

        _player.GetComponent<Player>().TakeDamage(_damage);

        yield return new WaitForSeconds(0.3f);

        _agent.isStopped = false;
        _lastAttackTime = Time.time;
        _isAttacking = false;
    }

    public override void ReturnToPool()  
    {
        EnemyManager.Unregister(this);
        _healthBar.ReturnToPool();
        base.ReturnToPool();
    }

    public void TakeDamage(float amount)
    {
        _currentHealth -= amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);

        DamagePopupManager.Instance.ShowPopup((int)amount, transform.position, Color.white);

        if (_currentHealth <= 0)
        {
            ReturnToPool();
            ExperienceManager.Instance.TrySpawnCrystal((int)_maxHealth, transform.position, Vector2.one);
        }
    }

    public void Heal(float amount)
    {
        _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
    }
}
