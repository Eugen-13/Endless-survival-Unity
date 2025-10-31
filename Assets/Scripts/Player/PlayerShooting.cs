using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    private InputSystem _inputSystem;


    [SerializeField] private Transform tower;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _detectionRadius = 9f;


    [SerializeField] private float _fireRate = 0.3f;
    [SerializeField] private float _bulletSpeed = 25f;
    [SerializeField] private int _damage = 5;


    private float _nextFireTime;
    // private bool _isShooting;
    private Transform _currentTarget;

    private string _poolName = "BulletPool";

    void Start()
    {
        _inputSystem = new InputSystem();
        _inputSystem.Enable();

        //_inputSystem.Player.Shoot.performed += ctx => _isShooting = true;
        //_inputSystem.Player.Shoot.canceled += ctx => _isShooting = false;


        PoolManager.Instance.CreatePool(_poolName, _projectilePrefab, 100);
    }

    void Update()
    {
        FindClosestVisibleEnemy();
        RotateTowardsTarget();
    }

    void FindClosestVisibleEnemy()
    {
        float closestDistance = Mathf.Infinity;
        Transform nearest = null;
        foreach (var enemy in EnemyManager.Enemies)
        {
            if (enemy == null) continue;

            float distance = Vector2.Distance(transform.position, enemy.position);
            if (distance < _detectionRadius && distance < closestDistance)
            {
                Vector2 dir = (enemy.position - transform.position).normalized;

                int layerMask = ~LayerMask.GetMask("Player");

                RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, distance, layerMask);

                
                if (hit && hit.collider.transform == enemy)
                {
                    closestDistance = distance;
                    nearest = enemy;
                }
            }
        }

        _currentTarget = nearest;
    }

    void RotateTowardsTarget()
    {
        if (_currentTarget != null)
        {
            var dir = _currentTarget.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            tower.rotation = Quaternion.Euler(0, 0, angle);
            if (Time.time >= _nextFireTime)
            {
                Shoot();
                _nextFireTime = Time.time + _fireRate;
            }
        }
        else
            tower.rotation = Quaternion.Lerp(tower.rotation, transform.rotation, 10f * Time.deltaTime);
    }

    private void Shoot()
    {
        var projectile = PoolManager.Instance.Get(_poolName, _firePoint.position, Quaternion.identity);
        var bullet = projectile.GetComponent<Projectile>();
        bullet.Initialize(_currentTarget, _bulletSpeed, _damage);
    }

}
