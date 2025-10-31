using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class TriangleEnemyBehaviour : PoolableObject
{

    [Header("Attack Settings")]
    [SerializeField] private float _attackRange = 1f;
    [SerializeField] private float _attackCooldown = 1f;
    [SerializeField] private int _damage = 10;

    [Header("Movement")]
    [SerializeField] private float _agentSpeed = 5f;
    [SerializeField] private float _repelRadius = 1f;
    [SerializeField] private float _repelForce = 1.5f;

    [SerializeField] private int _health = 5;

    [SerializeField] private LayerMask _enemyLayer;

    private float _lastAttackTime;
    private bool _isAttacking = false;
    private float _attackRangeSqr;

    private Transform _player;
    private NavMeshAgent _agent;
    private HealthBarFollow _healthBar;
    private Sequence _sequence;
    private Tween _rotationTween;

    public NavMeshAgent Agent => _agent;
    public float RepelRadius => _repelRadius;
    public float RepelForce => _repelForce;
    public float Health => _health;
    public void SetHealth(int health) => _health = health;
    public void SetHealthBar(HealthBarFollow healthBar) => _healthBar = healthBar;


    void Start()
    {

        _agent = GetComponent<NavMeshAgent>();

        _agent.updateRotation = false;
        _agent.updateUpAxis = false;

        _agent.radius = 0.5f;
        _agent.speed = _agentSpeed;
        _agent.angularSpeed = 1000f;
        _agent.acceleration = 8f;
        _agent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;

        _attackRangeSqr = _attackRange * _attackRange;

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

    public void TakeDamage(int amount)
    {
        DamagePopupManager.Instance.ShowPopup(amount, transform.position, Color.white);
        if (_healthBar.TakeDamageToDie(amount))
        {
            ReturnToPool();
            ExperienceManager.Instance.TrySpawnCrystal(_health, transform.position, Vector2.one);
        }
    }


    public override void ReturnToPool()  
    {
        EnemyManager.Unregister(this);
        _healthBar.ReturnToPool();
        base.ReturnToPool();
    } 
}
