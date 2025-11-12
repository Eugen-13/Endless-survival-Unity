using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class EnemyBase : PoolableObject, IHealth
{

    [Header("Base stats")]
    [SerializeField] protected float _attackRange = 1f;
    [SerializeField] protected float _attackCooldown = 1f;
    [SerializeField] protected float _damage = 10;
    [SerializeField] protected float _agentSpeed = 5f;
    [SerializeField] protected float _repelRadius = 2f;
    [SerializeField] protected float _repelForce = 3f;
    [SerializeField] protected float _maxHealth = 5;
    [SerializeField] protected float _currentHealth;


    protected float _lastAttackTime;
    protected bool _isAttacking = false;
    protected float _attackRangeSqr;

    protected Transform _player;
    protected NavMeshAgent _agent;
    protected HealthBarFollow _healthBar;


    protected SpriteRenderer _spriteRenderer;
    protected Sequence _attackSequence;
    protected Tween _movementTween;

    public event Action<float, float> OnHealthChanged;

    public NavMeshAgent Agent => _agent;
    public float RepelRadius => _repelRadius;
    public float RepelForce => _repelForce;

    public float MaxHealth { get { return _maxHealth; } set { _maxHealth = value; } }

    public float CurrentHealth => _currentHealth;

    public virtual void InitaliceStats(EnemyStats enemyStats)
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

    protected virtual void InitaliceAtackSequence() { _attackSequence = null; }
    protected virtual void InitaliceMovementTween() { _movementTween = null; }
    protected virtual void Awake()
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
    protected virtual void Start()
    {
        InitaliceAtackSequence();

        InitaliceMovementTween();
        _movementTween?.Play();

        _player = Player.Instance.transform;

    }
    protected virtual void OnEnable()
    {
        _movementTween?.Play();
        _attackRangeSqr = _attackRange * _attackRange;
        _currentHealth = _maxHealth;
    }
    protected virtual void OnDisable()
    {
        _movementTween?.Pause();
        _attackSequence?.Pause();
    }

    public virtual void Behavior()
    {
        if (_player == null) return;
        float sqr = (transform.position - _player.position).sqrMagnitude;

        if (sqr <= _attackRangeSqr)
            StartAttack();
        else
            MoveToPlayer();
    }

    protected virtual void MoveToPlayer()
    {
        _agent.isStopped = false;
        _agent.SetDestination(_player.position);
    }

    protected virtual void StartAttack()
    {
        if (_isAttacking || Time.time < _lastAttackTime + _attackCooldown) return;
        _agent.isStopped = true;
        StartCoroutine(AttackRoutine());
    }

    protected virtual IEnumerator AttackRoutine()
    {
        if (_isAttacking)
            yield break;

        _isAttacking = true;

        if (_attackSequence != null)
            _attackSequence.Restart();

        _player.GetComponent<Player>().TakeDamage(_damage);

        DamagePopupManager.Instance.ShowPopup((int)_damage, _player.position, Color.red);

        yield return new WaitForSeconds(0.3f);

        _agent.isStopped = false;
        _lastAttackTime = Time.time;
        _isAttacking = false;
    }



    public virtual void TakeDamage(float amount)
    {
        _currentHealth -= amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);

        DamagePopupManager.Instance.ShowPopup((int)amount, transform.position, Color.white);

        if (_currentHealth <= 0)
            Die();
    }
    public virtual void Heal(float amount)
    {
        _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
    }
    public virtual void Die()
    {
        ReturnToPool();
        ExperienceManager.Instance.TrySpawnCrystal((int)_maxHealth, transform.position, Vector2.one);
    }


    public override void ReturnToPool()
    {
        EnemyManager.Unregister(this);
        _healthBar.ReturnToPool();
        base.ReturnToPool();
    }
}
