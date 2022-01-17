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
        [SerializeField] private float timeToSpawn = 0.3f;
        [SerializeField] private int heightBetweenPlatforms = 3;
        [SerializeField] private int distanceBetweenPlatforms = 4;
        [SerializeField] private float zOffsetForward = 20f;
        [SerializeField] private int yMaxRandomOffset = 5;
        [SerializeField] private int zMaxRandomOffset = 20;
        
        
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
                temporary.MoveForward(FindNewPositionForPlatform());
                queue.Enqueue(temporary);
                yield return null;
            }
            else
            {
                yield return new WaitForSeconds(timeToSpawn);
            }
        }

        private Vector3 FindNewPositionForPlatform()
        {
            int zCurrentRandomOffset = Random.Range(1, zMaxRandomOffset) * distanceBetweenPlatforms;
            int yCurrentRandomOffset = Random.Range(1, yMaxRandomOffset) * heightBetweenPlatforms;
            return new Vector3(0, yCurrentRandomOffset, PlayerZPosition + zOffsetForward + zCurrentRandomOffset);
        }

    }
}