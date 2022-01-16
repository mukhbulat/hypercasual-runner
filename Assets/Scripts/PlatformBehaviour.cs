using System;
using UnityEngine;

public class PlatformBehaviour : MonoBehaviour
{
    [SerializeField] private PlatformData data;
    
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

    public void InitializePoolObject(Vector3 startingPosition)
    {
        Instantiate(data.prefab, startingPosition, Quaternion.identity);
    }

    private void Awake()
    {
        Instantiate(data.prefab, transform);
    }
}