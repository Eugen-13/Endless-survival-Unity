using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class PlayerShooting : MonoBehaviour
{
    private InputSystem _inputSystem;

    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private GameObject _hitEffect;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _detectionRadius = 9f;


    private float _nextFireTime;
    // private bool _isShooting;
    private Transform[] _currentTargets;
    private Player _player;

    private string _poolName = "BulletPool";
    private string _hitEffectPool = "hitEffectPool1";

    void Start()
    {
        _inputSystem = new InputSystem();
        _inputSystem.Enable();

        //_inputSystem.Player.Shoot.performed += ctx => _isShooting = true;
        //_inputSystem.Player.Shoot.canceled += ctx => _isShooting = false;


        PoolManager.Instance.CreatePool(_poolName, _projectilePrefab, 100);
        PoolManager.Instance.CreatePool(_hitEffectPool, _hitEffect, 30);
        _player = Player.Instance;
    }

    void Update()
    {
        FindClosestVisibleEnemy();
        ShootToTargets();
    }

    void FindClosestVisibleEnemy()
    {

        Dictionary<Transform, float> targets = new Dictionary<Transform, float>();
        foreach (var enemy in EnemyManager.Enemies)
        {
            if (enemy == null) continue;

            float distance = (transform.position - enemy.position).sqrMagnitude;

            if (distance < (_detectionRadius * _detectionRadius))
            {
                targets.Add(enemy, distance);
            }
        }

        int min = Math.Min(targets.Count, Player.Instance.ProjectileCount);
        _currentTargets = new Transform[min];


        for (int i = 0; i < _currentTargets.Length; i++)
        {
            float closestDistance = Mathf.Infinity;
            foreach (var enemy in targets)
            {
                if (enemy.Key == null) continue;

                float distance = enemy.Value;
                if (distance < closestDistance)
                {
                    Vector2 dir = (enemy.Key.position - transform.position).normalized;

                    int layerMask = ~LayerMask.GetMask("Player");

                    RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, distance, layerMask);

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

    void ShootToTargets()
    {
        if (Time.time >= _nextFireTime)
        {
            for (int i = 0; i < _currentTargets.Length; i++)
            {
                if (_currentTargets[i] != null)
                {
                    Shoot(_currentTargets[i]);
                    _nextFireTime = Time.time + _player.FireRate;

                }
            }
        }   
    }
    private void Shoot(Transform target)
    {
        var projectile = PoolManager.Instance.Get(_poolName, _firePoint.position, Quaternion.identity);
        var bullet = projectile.GetComponent<Projectile>();
        bullet.Initialize(target, _player.BulletSpeed, _player.Damage, _hitEffectPool);
    }

}
