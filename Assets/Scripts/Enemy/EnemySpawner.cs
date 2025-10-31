using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float _spawnDuration;
    [SerializeField] private Transform _player;

    [SerializeField] private GameObject _enemyTrianglePrefab;
    [SerializeField] private GameObject _healthBarPrefab;

    private Transform[] _spawnPoints;
    private string _enemyPoolName = "EnemyPool1";
    private string _enemyHealthPoolName = "EnemyHealthPool";
    

    void Start()
    {
        _spawnPoints = GetComponentsInChildren<Transform>(); 
        PoolManager.Instance.CreatePool(_enemyPoolName, _enemyTrianglePrefab, 200);
        PoolManager.Instance.CreatePool(_enemyHealthPoolName, _healthBarPrefab, 200);
        StartCoroutine(SpawnEnemyes());
    }

    IEnumerator SpawnEnemyes()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnDuration);
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        int rand = Random.Range(0, _spawnPoints.Length);

        var enemy = PoolManager.Instance.Get(_enemyPoolName, _spawnPoints[rand].position, Quaternion.identity);

        var healthBar = PoolManager.Instance.Get(_enemyHealthPoolName, enemy.transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
        healthBar.GetComponent<HealthBarFollow>().SetTarget(enemy.transform);

        var enemyBeh = enemy.GetComponent<TriangleEnemyBehaviour>();
        healthBar.GetComponent<HealthBarFollow>().SetMaxHealth(enemyBeh.Health);
        enemyBeh.SetHealthBar(healthBar.GetComponent<HealthBarFollow>());

        EnemyManager.Register(enemyBeh);
        
    }

}
