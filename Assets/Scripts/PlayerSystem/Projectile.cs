using System.Collections;
using Core.ObjectPool;
using EnemyTypes;
using Managers;
using UnityEngine;

namespace PlayerSystem
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : PoolableObject
    {
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private float _lifetime = 2f;
        
        private float _speed;
        private float _damage;

        private Transform _target;
        private Coroutine _returnCoroutine;
        
        private string _hitEffectPoolName;
        
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
            _hitEffectPoolName = hitEffectPoolName;

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
                _poolManager.Get(_hitEffectPoolName, transform.position, transform.rotation);
            }
        }

    }
}