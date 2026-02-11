using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Core.ObjectPool
{
    public class ObjectPool
    {
        private readonly DiContainer _container;
        private readonly GameObject _prefab;
        private readonly Transform _parent;
        private readonly Queue<GameObject> _pool = new Queue<GameObject>();

        public ObjectPool(DiContainer container, GameObject prefab, int initialSize, string key, Transform parent = null)
        {
            _container = container;
            _prefab = prefab;
            _parent = parent;
            for (int i = 0; i < initialSize; i++)
            {
                GameObject obj = _container.InstantiatePrefab(_prefab, _parent);
                obj.SetActive(false);

                IPoolable poolable = obj.GetComponent<IPoolable>();
                poolable?.SetPoolName(key);

                _pool.Enqueue(obj);
            }
        }

        public GameObject Get(Vector2 position, Quaternion rotation, Transform parent = null)
        {
            Transform parentTransform = parent ?? _parent;

            GameObject obj = _pool.Count > 0 ? _pool.Dequeue() : _container.InstantiatePrefab(_prefab);

            if (parentTransform)
            {
                obj.transform.SetParent(parentTransform);
            }

            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.SetActive(true);
            return obj;
        }

        public void Return(GameObject obj)
        {
            obj.SetActive(false);
            _pool.Enqueue(obj);
        }
    }
}
