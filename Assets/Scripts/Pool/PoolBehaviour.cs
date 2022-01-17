using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using Random = UnityEngine.Random;

namespace Pool
{
    public class PoolBehaviour : MonoBehaviour
    {
        [SerializeField] private List<GameObject> platformPrefabs;
        private Queue<IPoolable> _platformsInGame = new Queue<IPoolable>(60);


        [SerializeField] private Transform playerTransform;
        [SerializeField] private float zOffsetBack = 10f;
        [SerializeField] private float timeToSpawn = 1f;
        [SerializeField] private float zOffsetForward = 20f;

        private enum PooledObject
        {
            Platform,
            Coin
        }
        
        private float PlayerZPosition => playerTransform.transform.position.z; 
        private void Awake()
        {
            CreateQueue();
        }

        private void Update()
        {
            StartCoroutine(SwapPlatforms(_platformsInGame));
        }

        private void CreateQueue()
        {
            // 60 is the capacity of _platformsInGame.
            int everyPlatformTypeCount = 60 / platformPrefabs.Count;
            // Nested loop.
            foreach (var platform in platformPrefabs)
            {
                for (int j = 0; j < everyPlatformTypeCount; j++)
                {
                    IPoolable instance = platform.GetComponent<IPoolable>().Initialize().GetComponent<IPoolable>();
                    _platformsInGame.Enqueue(instance);
                }
            }
        }

        private IEnumerator SwapPlatforms(Queue<IPoolable> queue)
        {
            if (queue.Peek().GetPosition() < PlayerZPosition - zOffsetBack)
            {
                IPoolable temporary = queue.Dequeue();
                temporary.MoveForward(FindNewPosition(PooledObject.Platform));
                queue.Enqueue(temporary);
                yield return null;
            }
            else
            {
                yield return new WaitForSeconds(timeToSpawn);
            }
        }

        private Vector3 FindNewPosition(PooledObject objectType)
        {
            switch (objectType)
            {
                case PooledObject.Platform:
                {
                    return new Vector3(0, 3, PlayerZPosition + zOffsetForward);
                }
            }

            throw new Exception("Unknown objectType");
        }

    }
}