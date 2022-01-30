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

        private int _currentSegmentNumber = 0;
        private int _nextSegmentNumber = 1;

        private LevelSegment _currentSegment;
        
        //Player
        [SerializeField] private Transform player;
        private float PlayerZPosition => player.transform.position.z;


        // Platforms
        [SerializeField] private GameObject platformBehaviourPrefab;
        
        // This should be more than max in PlatformsSegments.
        [SerializeField] private int platformsCapacity = 50;
        
        private Queue<List<List<IPoolable>>> _platformsQueue = new Queue<List<List<IPoolable>>>(3);

        private int NumberOfTypesOfPlatforms =>
            platformBehaviourPrefab.GetComponent<IPoolable>().GetNumberOfTypesOfThis();
        
        // Collectables
        [SerializeField] private GameObject collectableBehaviourPrefab;
        
        // This should be more than max in CollectablesSegments.
        [SerializeField] private int collectablesCapacity = 20;
        private Queue<List<List<IPoolable>>> _collectablesQueue = new Queue<List<List<IPoolable>>>(3);

        private int NumberOfTypesOfCollectables =>
            collectableBehaviourPrefab.GetComponent<IPoolable>().GetNumberOfTypesOfThis();


        #region UnityFuncs

        private void Awake()
        {
            // Getting two random segments to make first two segments.
            int i = Random.Range(0, LevelSegmentsCount);
            var segmentI = levelSegments[i];
            int j = Random.Range(0, LevelSegmentsCount);
            var segmentJ = levelSegments[j];
            
            // Platforms
            int platformTypesCount = levelSegments[0].Platforms.GetTypesCount;
            InitializeQueue(_platformsQueue, platformTypesCount,
                platformsCapacity, platformBehaviourPrefab);
            var temporaryPlatformsList = _platformsQueue.Dequeue();
            MoveObjects(temporaryPlatformsList, 0, segmentI.Platforms, i);
            _platformsQueue.Enqueue(temporaryPlatformsList);
            temporaryPlatformsList = _platformsQueue.Dequeue();
            MoveObjects(temporaryPlatformsList, 1, segmentJ.Platforms, j);
            
            // Collectables
            int collectableTypesCount = levelSegments[0].Collectables.GetTypesCount;
            InitializeQueue(_collectablesQueue, collectableTypesCount,
                collectablesCapacity, collectableBehaviourPrefab);
            var temporaryCollectablesList = _collectablesQueue.Dequeue();
            MoveObjects(temporaryCollectablesList, 0, segmentI.Collectables, i);
            _collectablesQueue.Enqueue(temporaryCollectablesList);
            temporaryCollectablesList = _collectablesQueue.Dequeue();
            MoveObjects(temporaryCollectablesList, 1, segmentJ.Collectables, j);
            _collectablesQueue.Enqueue(temporaryCollectablesList);
            
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
            // Double nested loop.
            // At least it would be in Awake. May be make it coroutine and add yield return null to make it on different
            // frames?
            for (int j = 0; j < capacityOfTypes; j++)
            {
                listToInit.Add(new List<IPoolable>(capacityOfObjects));
                for (int k = 0; k < capacityOfObjects; k++)
                {
                    listToInit[j].Add(behaviourPrefab.GetComponent<IPoolable>().Initialize(j));
                }
            }
        } 
        
        private void MoveObjects(List<List<IPoolable>> listToMove, int zOffset, ISegment objectsSegment, int index)
        {
            Vector3 offset = new Vector3(0, 0,levelSegments[index].Length * zOffset);
            int i = 0;
            int j = 0;
            
            // Nested loop. GetSegmentParts is not a huge array, but still, need to think on it.
            
            foreach (var listOfPositions in objectsSegment.GetSegmentParts)
            {
                foreach (var position in listOfPositions)
                {
                    listToMove[i][j].MoveForward(position + offset);
                    j += 1;
                }

                j = 0;
                i += 1;
            }
        }

        #endregion
    }
    
}