using System.Collections.Generic;
using Core.ObjectPool;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class PoolManager
    {
        private readonly Dictionary<string, ObjectPool> _pools = new();
        
        private readonly DiContainer _diContainer;
        private readonly Transform _poolsParentTransform;

        public PoolManager(DiContainer diContainer,
            [Inject(Id = "PoolsParent")]Transform poolsParentTransform)
        {
            _diContainer = diContainer;
            _poolsParentTransform = poolsParentTransform;
        }
        
        public void CreatePool(string key, GameObject prefab, int count)
        {
            if (_pools.ContainsKey(key))
            {
                Debug.LogWarning($"Pool with key '{key}' already exists!");
                return;
            }

            GameObject poolParent = new(key);
            poolParent.transform.SetParent(_poolsParentTransform);

            _pools[key] = new ObjectPool(_diContainer, prefab, count, key, poolParent.transform);
        }

        public GameObject Get(string key, Vector2 position, Quaternion rotation)
        {
            if (!_pools.TryGetValue(key, out ObjectPool pool))
            {
                Debug.LogError($"No pool with key '{key}'");
                return null;
            }

            return pool.Get(position, rotation);
        }

        public void Return(string key, GameObject obj)
        {
            if (_pools.TryGetValue(key, out ObjectPool pool))
                pool.Return(obj);
        }
    }
}