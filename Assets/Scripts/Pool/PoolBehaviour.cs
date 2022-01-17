using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pool
{
    public class PoolBehaviour : MonoBehaviour
    {
        [SerializeField] private List<GameObject> platformPrefabs;
        private Queue<IPoolable> _platformsInGame = new Queue<IPoolable>(60);

        private void TakePlatformOutOfPool(int number)
        {
            
        }

        private void Awake()
        {
            CreateQueue();
        }

        private void CreateQueue()
        {
            //60 is the capacity of _platformsInGame
            int everyPlatformTypeCount = 60 / platformPrefabs.Count;
            foreach (var platform in platformPrefabs)
            {
                for (int j = 0; j < everyPlatformTypeCount; j++)
                {
                    IPoolable instance = platform.GetComponent<IPoolable>().Initialize().GetComponent<IPoolable>();
                    _platformsInGame.Enqueue(instance);
                }
            }
        }
        
    }
}