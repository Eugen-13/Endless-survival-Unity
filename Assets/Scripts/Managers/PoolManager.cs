using System.Collections.Generic;
using UnityEngine;
public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    private Dictionary<string, ObjectPool> _pools = new();

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void CreatePool(string key, GameObject prefab, int count)
    {
        if (_pools.ContainsKey(key))
        {
            Debug.LogWarning($"Pool with key '{key}' already exists!");
            return;
        }

        var poolParent = new GameObject(key);
        poolParent.transform.SetParent(transform);

        _pools[key] = new ObjectPool(prefab, count, key, poolParent.transform);
    }

    public GameObject Get(string key, Vector2 position, Quaternion rotation)
    {
        if (!_pools.TryGetValue(key, out var pool))
        {
            Debug.LogError($"No pool with key '{key}'");
            return null;
        }

        return pool.Get(position, rotation);
    }

    public void Return(string key, GameObject obj)
    {
        if (_pools.TryGetValue(key, out var pool))
            pool.Return(obj);
    }
}