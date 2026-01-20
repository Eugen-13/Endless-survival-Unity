using System;
using System.Collections.Generic;
using System.Linq;
using Managers;
using UnityEngine;
using Zenject;

namespace PlayerSystem
{
    public class PlayerShooting : IInitializable, ITickable
    {
        private InputSystem _inputSystem;

        private GameObject _hitEffect;
        private Transform _firePoint;
        private Projectile _projectilePrefab;
        private EnemyManager _enemyManager;
        private PoolManager _poolManager;
        private Player _player;

        [Inject]
        private void Construct(
            [Inject(Id = "HitEffectPrefab")]GameObject hitEffectPrefab, 
            [Inject(Id = "FirePoint")]Transform firePoint,
            Projectile projectilePrefab, 
            EnemyManager enemyManager,
            PoolManager poolManager,
            Player player)
        {
            _projectilePrefab = projectilePrefab;
            _hitEffect = hitEffectPrefab;
            _firePoint = firePoint;
            _enemyManager = enemyManager;
            _poolManager = poolManager;
            _player = player;
        }

        private float _nextFireTime;
        // private bool _isShooting;
        private Transform[] _currentTargets;

        private readonly string _poolName = "BulletPool";
        private readonly string _hitEffectPool = "hitEffectPool1";

        public void Initialize()
        {
            _inputSystem = new InputSystem();
            _inputSystem.Enable();

            //_inputSystem.Player.Shoot.performed += ctx => _isShooting = true;
            //_inputSystem.Player.Shoot.canceled += ctx => _isShooting = false;
            _poolManager.CreatePool(_poolName, _projectilePrefab.gameObject, 100);
            _poolManager.CreatePool(_hitEffectPool, _hitEffect, 100);
        }

        public void Tick()
        {
            FindClosestVisibleEnemy();
            ShootToTargets();
        }


        private void FindClosestVisibleEnemy()
        {
            Vector3 playerPos = _player.transform.position;
            Dictionary<Transform, float> targets = new();
            foreach (Transform enemy in _enemyManager.Enemies)
            {
                if (!enemy) continue;

                float distance = (playerPos - enemy.position).sqrMagnitude;

                if (distance < (_player.DetectionRadius * _player.DetectionRadius))
                {
                    targets.Add(enemy, distance);
                }
            }

            int min = Math.Min(targets.Count, _player.ProjectileCount);
            _currentTargets = new Transform[min];


            for (int i = 0; i < _currentTargets.Length; i++)
            {
                float closestDistance = Mathf.Infinity;
                foreach (KeyValuePair<Transform, float> enemy in targets)
                {
                    if (!enemy.Key) continue;

                    float distance = enemy.Value;
                    if (distance < closestDistance)
                    {
                        Vector2 dir = (enemy.Key.position - playerPos).normalized;

                        int layerMask = ~LayerMask.GetMask("Player");

                        RaycastHit2D hit = Physics2D.Raycast(playerPos, dir, distance, layerMask);

                        if (hit && _currentTargets.Contains(hit.collider.transform))
                            continue;

                        if (hit && hit.collider.transform == enemy.Key)
                        {
                            closestDistance = distance;
                            _currentTargets[i] = enemy.Key;
                        }
                    }
                }
            }

        }

        private void ShootToTargets()
        {
            if (Time.time >= _nextFireTime)
            {
                for (int i = 0; i < _currentTargets.Length; i++)
                {
                    if (_currentTargets[i])
                    {
                        Shoot(_currentTargets[i]);
                        _nextFireTime = Time.time + _player.FireRate;

                    }
                }
            }   
        }
        private void Shoot(Transform target)
        {
            GameObject projectile = _poolManager.Get(_poolName, _firePoint.position, Quaternion.identity);
            Projectile bullet = projectile.GetComponent<Projectile>();
            bullet.Initialize(target, _player.BulletSpeed, _player.Damage, _hitEffectPool);
        }
        
    }
}
