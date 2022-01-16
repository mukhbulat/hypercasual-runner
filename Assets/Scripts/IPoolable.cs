using UnityEngine;

public interface IPoolable
{
    public void ReturnToPool();
    public void GetFromPool(Vector3 position);
}