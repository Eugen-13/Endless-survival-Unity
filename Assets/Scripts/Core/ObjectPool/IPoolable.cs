namespace Core.ObjectPool
{
    public interface IPoolable
    {
        void SetPoolName(string poolName);

        void ReturnToPool();
    }
}