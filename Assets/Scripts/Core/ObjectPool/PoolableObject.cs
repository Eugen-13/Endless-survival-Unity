using UnityEngine;

public abstract class PoolableObject : MonoBehaviour, IPoolable
{
    protected string _poolName;

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

        PoolManager.Instance.Return(_poolName, gameObject);
    }
}