using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.HealthBar;
using EnemyTypes;
using Managers;
using PlayerSystem;
using UnityEngine;
using Zenject;


public class EnemySpawner : MonoBehaviour
{

    [System.Serializable]
    public struct EnemySpawnWeight
    {
        public GameObject prefab;
        public float weight;
    }
    
    [SerializeField] private List<EnemySpawnWeight> spawnWeights;
    [SerializeField] private float _spawnDuration;
    [SerializeField] private GameObject _healthBarPrefab ;

    [Header("Enemy stats")]
    [SerializeField] private int _health;
    [SerializeField] private float _speed;
    [SerializeField] private int _damage;
    [SerializeField] private float _attackCooldown;
    [SerializeField] private float _statsScale = 0.2f;
    [SerializeField] private float exp = 5f;
    
    private EnemyManager _enemyManager;
    private PoolManager _poolManager;
    private Player _player;

    [Inject]
    private void Construct(EnemyManager enemyManager, PoolManager poolManager, Player player)
    {
        _enemyManager = enemyManager;
        _poolManager = poolManager;
        _player = player;
    }
    private Transform[] _spawnPoints;
    private int _index;
    private readonly string _enemyPoolName = "EnemyPool";
    private readonly string _enemyHealthPoolName = "EnemyHealthPool";


    private void Start()
    {
        _spawnPoints = GetComponentsInChildren<Transform>().Where(t => t != transform).ToArray();
        foreach (EnemySpawnWeight item in spawnWeights)
        {
            _poolManager.CreatePool(_enemyPoolName + item.prefab.name, item.prefab, 50);
        }
        _poolManager.CreatePool(_enemyHealthPoolName, _healthBarPrefab, 200);
        StartCoroutine(SpawnEnemyes());
    }

    IEnumerator SpawnEnemyes()
    {
        while (_player)
        {
            SpawnEnemy(GetEnemyStats());
            yield return new WaitForSeconds(_spawnDuration);
        }
        
    }

    private void SpawnEnemy(EnemyStats enemyStats)
    {
        string enemyPoolName = _enemyPoolName + GetRandomEnemy();
        Vector3 spawnPoint = _spawnPoints[_index % _spawnPoints.Length].position;

        GameObject enemy = _poolManager.Get(_enemyPoolName, spawnPoint, Quaternion.identity);
        
        HealthBarFollow healthBar = (_poolManager.Get(_enemyHealthPoolName, enemy.transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity)).GetComponent<HealthBarFollow>();
        healthBar.SetTarget(enemy.transform);

        BaseEnemy enemyBeh = enemy.GetComponent<BaseEnemy>();
        enemyBeh.InitaliceStats(enemyStats);
        switch (enemyBeh)
        {
            case SquareEnemy square:
                square.SetHealthBarSource(healthBar, 1.5f);
                break;


            default:
                enemyBeh.SetHealthBarSource(healthBar);
                break;
        }
        
        _enemyManager.Register(enemyBeh);
        
        _index++;
    }

    private string GetRandomEnemy()
    {
       
        float sum = spawnWeights.Sum(e => e.weight);
        float rand = Random.Range(0, sum);
        foreach (EnemySpawnWeight weight in spawnWeights)
        {
            rand -= weight.weight;
            if (rand <= 0)
            {
                return weight.prefab.name;
            }
        }

        return null;
    }

    private EnemyStats GetEnemyStats()
    {
        int level = _player.Level;
        float scale = _statsScale * (level - 1);
        return new EnemyStats { Health = _health + _health * scale, Damage = _damage + _damage * scale, Speed = _speed + _speed * scale, AttackCooldown = _attackCooldown - _attackCooldown * scale, Experience = exp + exp * scale };
    }

}
