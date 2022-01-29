using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Pool
{
    public class PoolBehaviour : MonoBehaviour
    {
        
        // Generic Fields
        [SerializeField] private List<LevelSegment> levelSegments;
        private int LevelSegmentsCount => levelSegments.Count;

        private int _currentSegment = 0;
        private int _nextSegment = 1;
        
        //Player
        [SerializeField] private Transform player;
        private float PlayerZPosition => player.transform.position.z;


        // Platforms
        [SerializeField] private GameObject platformBehaviourPrefab;
        [SerializeField] private int platformsCapacity = 50;
        
        private Queue<List<List<IPoolable>>> _platformsInSegments = new Queue<List<List<IPoolable>>>(3);

        private int NumberOfTypesOfPlatforms =>
            platformBehaviourPrefab.GetComponent<IPoolable>().GetNumberOfTypesOfThis();
        
        // Collectables
        [SerializeField] private GameObject collectableBehaviourPrefab;
        [SerializeField] private int collectablesCapacity = 20;
        
        private Queue<List<List<IPoolable>>> _collectablesInSegments = new Queue<List<List<IPoolable>>>(3);

        private int NumberOfTypesOfCollectables =>
            collectableBehaviourPrefab.GetComponent<IPoolable>().GetNumberOfTypesOfThis();


        #region UnityFuncs

        private void Awake()
        {
            
        }

        private void Update()
        {
            
        }

        #endregion
        
        #region IPoolableLists
        
        private void InitializeQueue(Queue<List<List<IPoolable>>> queue, int capacityOfTypes, int capacityOfObjects, GameObject behaviourPrefab)
        {
            for (int i = 0; i < 3; i++)
            {
                queue.Enqueue(new List<List<IPoolable>>(capacityOfTypes));
            }

            foreach (var list in queue)
            {
                InitializeObjects(list, capacityOfTypes, capacityOfObjects, behaviourPrefab);
            }
        }

        private void InitializeObjects(List<List<IPoolable>> listToInit, int capacityOfTypes, int capacityOfObjects, GameObject behaviourPrefab)
        {
            for (int j = 0; j < capacityOfTypes; j++)
            {
                listToInit.Add(new List<IPoolable>(capacityOfObjects));
                for (int k = 0; k < capacityOfObjects; k++)
                {
                    listToInit[j].Add(behaviourPrefab.GetComponent<IPoolable>().Initialize(j));
                }
            }
        } 
        
        private void MoveObjects(List<List<IPoolable>> listToMove, int zOffset, ISegment objectsSegment)
        {
            
        }

        #endregion
    }
    
}