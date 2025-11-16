using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : PoolableObject
{

    [SerializeField] private float _lifetime = 2f;

    private float _speed;
    private float _damage;

    private Transform _target;
    private Coroutine _returnCoroutine;
    private Rigidbody2D _rb;
    private string _hitEffectPool;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!_target.gameObject.activeInHierarchy)
            ReturnToPool();

        Vector2 dir = (_target.position - transform.position).normalized;
        _rb.velocity = dir * _speed;

    }

    public void Initialize(Transform target, float speed, float damage, string hitEffectPoolName)
    {
        _target = target;
        _speed = speed;
        _damage = damage;
        _hitEffectPool = hitEffectPoolName;

        if (_returnCoroutine != null)
            StopCoroutine(_returnCoroutine);

        _returnCoroutine = StartCoroutine(ReturnAfterTime());
    }

    private IEnumerator ReturnAfterTime()
    {
        yield return new WaitForSeconds(_lifetime);
        ReturnToPool();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<BaseEnemy>(out BaseEnemy enemy))
        {
            enemy.TakeDamage(_damage);
            ReturnToPool();
            PoolManager.Instance.Get(_hitEffectPool, transform.position, transform.rotation);
        }
    }

}