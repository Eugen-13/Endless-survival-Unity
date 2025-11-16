using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;



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
    [SerializeField] private float _atackCooldawn;
    [SerializeField] private float _statsScale = 0.2f;
    [SerializeField] private float exp = 5f;

    private Transform[] _spawnPoints;
    private int _index;
    private string _enemyPoolName = "EnemyPool";
    private string _enemyHealthPoolName = "EnemyHealthPool";
    

    void Start()
    {
        _spawnPoints = GetComponentsInChildren<Transform>().Where(t => t != transform).ToArray();
        foreach (var item in spawnWeights)
        {
            PoolManager.Instance.CreatePool(_enemyPoolName + item.prefab.name, item.prefab, 50);
        }
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
        var enemy = PoolManager.Instance.Get(_enemyPoolName + getRandomEnemy(), _spawnPoints[_index % _spawnPoints.Length].position, Quaternion.identity);
        _index++;

        var healthBar = (PoolManager.Instance.Get(_enemyHealthPoolName, enemy.transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity)).GetComponent<HealthBarFollow>();
        healthBar.SetTarget(enemy.transform);

        var enemyBeh = enemy.GetComponent<BaseEnemy>();
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
        
        EnemyManager.Register(enemyBeh);
    }

    private string getRandomEnemy()
    {
       
        float sum = spawnWeights.Sum(e => e.weight);
        float rand = Random.Range(0, sum);
        foreach (var weight in spawnWeights)
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
        int level = Player.Instance.Level;
        float scale = _statsScale * (level - 1);
        return new EnemyStats { Health = _health + _health * scale, Damage = _damage + _damage * scale, Speed = _speed + _speed * scale, AtackCooldawn = _atackCooldawn - _atackCooldawn * scale, Experience = exp + exp * scale };
    }

}
