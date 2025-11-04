using System.Collections;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float _spawnDuration;
    [SerializeField] private GameObject _enemyTrianglePrefab;
    [SerializeField] private GameObject _healthBarPrefab;

    [Header("Enemy stats")]
    [SerializeField] private int _health;
    [SerializeField] private float _speed;
    [SerializeField] private int _damage;
    [SerializeField] private float _atackCooldawn;
    [SerializeField] private float _statsScale = 0.2f;

    private Transform[] _spawnPoints;
    private string _enemyPoolName = "EnemyPool1";
    private string _enemyHealthPoolName = "EnemyHealthPool";
    

    void Start()
    {
        _spawnPoints = GetComponentsInChildren<Transform>().Where(t => t != transform).ToArray();
        PoolManager.Instance.CreatePool(_enemyPoolName, _enemyTrianglePrefab, 200);
        PoolManager.Instance.CreatePool(_enemyHealthPoolName, _healthBarPrefab, 200);
        StartCoroutine(SpawnEnemyes());
    }

    IEnumerator SpawnEnemyes()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnDuration);
            SpawnEnemy(GetEnemyStats());
        }
    }

    private void SpawnEnemy(EnemyStats enemyStats)
    {
        int rand = Random.Range(0, _spawnPoints.Length);

        var enemy = PoolManager.Instance.Get(_enemyPoolName, _spawnPoints[rand].position, Quaternion.identity);

        var healthBar = (PoolManager.Instance.Get(_enemyHealthPoolName, enemy.transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity)).GetComponent<HealthBarFollow>();
        healthBar.SetTarget(enemy.transform);

        var enemyBeh = enemy.GetComponent<TriangleEnemyBehaviour>();
        enemyBeh.SetHealthBarSource(healthBar);
        enemyBeh.Initalice(enemyStats);

        EnemyManager.Register(enemyBeh);
        
    }

    private EnemyStats GetEnemyStats()
    {
        int level = Player.Instance.Level;
        float scale = _statsScale * (level - 1);
        return new EnemyStats { Health = _health + _health * scale, Damage = _damage + _damage * scale, Speed = _speed + _speed * scale, AtackCooldawn = _atackCooldawn - _atackCooldawn * scale };
    }

}
