using Managers;
using UnityEngine;
using Zenject;

namespace Core.ObjectPool
{
    public abstract class PoolableObject : MonoBehaviour, IPoolable
    {
        private string _poolName;
        [Inject] protected PoolManager _poolManager;
        
        public virtual void SetPoolName(string poolName)
        {
            _poolName = poolName;
        }

        public virtual void ReturnToPool()
        {
            if (string.IsNullOrEmpty(_poolName))
            {
                Debug.LogWarning($"{name}: Pool name is not set!");
                return;
            }

            _poolManager.Return(_poolName, gameObject);
        }
    }
}