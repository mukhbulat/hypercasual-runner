using UnityEngine;

public class PlatformBehaviour : MonoBehaviour, IPoolable
{
    private PlatformData _data;
    
    public void ReturnToPool()
    {
        gameObject.SetActive(false);
    }

    public void GetFromPool(Vector3 position)
    {
        if (gameObject.activeInHierarchy) return;
        gameObject.transform.position = position;
        gameObject.SetActive(true);
    }
}