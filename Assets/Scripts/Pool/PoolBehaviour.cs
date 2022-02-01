using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Pool
{
    public class PoolBehaviour : MonoBehaviour
    {
        #region FieldsAndProperties

        // Generic Fields
        [SerializeField] private List<LevelSegment> levelSegments;
        private int LevelSegmentsCount => levelSegments.Count;

        private int _currentSegmentNumber = 0;
        private int _nextSegmentNumber = 1;

        private LevelSegment _currentSegment;
        private LevelSegment _nextSegment;
        
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

        // Environment
        [SerializeField] private GameObject environmentBehaviourPrefab;
        
        // This should be more than max in EnvironmentSegments.
        [SerializeField] private int environmentsCapacity = 8;

        private Queue<List<List<IPoolable>>> _environmentsQueue = new Queue<List<List<IPoolable>>>(3);

        #endregion
        #region UnityFuncs

        private void Awake()
        {
            // Getting two random segments to make first two segments.
            int i = Random.Range(0, LevelSegmentsCount);
            var segmentI = levelSegments[i];
            int j = Random.Range(0, LevelSegmentsCount);
            var segmentJ = levelSegments[j];

            _currentSegment = segmentI;
            _nextSegment = segmentJ;
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
            StartCoroutine(MoveObjectsLoop());
        }

        #endregion
        
        private IEnumerator MoveObjectsLoop()
        {
            while (PlayerZPosition < 0) yield return null;
            while (true)
            {
                float currentSegmentLength = _currentSegment.Length;
                _currentSegmentNumber = (int) (PlayerZPosition / currentSegmentLength);
                float modulo = PlayerZPosition % currentSegmentLength;
                // If player passed 1/3 of current segment, dequeue previous segment and create new one in front.
                if (_currentSegmentNumber == _nextSegmentNumber && modulo > currentSegmentLength / 3)
                {
                    int nextSegmentIndex = Random.Range(0, LevelSegmentsCount);
                    _nextSegment = levelSegments[nextSegmentIndex];
                    _nextSegmentNumber += 1;
                    
                    QueueMixing(_platformsQueue, nextSegmentIndex, _nextSegmentNumber, _nextSegment.Platforms);
                    yield return null;
                    QueueMixing(_collectablesQueue, nextSegmentIndex, _nextSegmentNumber, _nextSegment.Collectables);
                    yield return null;
                    QueueMixing(_environmentsQueue, nextSegmentIndex, _nextSegmentNumber, _nextSegment.Environments);
                    yield return null;
                }
                
                yield return new WaitForSeconds(0.1f);
            }
        }
        
        #region PoolableLists

        private void QueueMixing(Queue<List<List<IPoolable>>> initialQueue, int segmentIndex,
            int segmentNumber, ISegment objectsSegment)
        // Takes list of objects from queue, changes positions of objects and returns the list in queue.
        {
            var temporaryList = initialQueue.Dequeue();
            MoveObjects(temporaryList, segmentNumber, objectsSegment, segmentIndex);
            initialQueue.Enqueue(temporaryList);
        }
        
        private void InitializeQueue(Queue<List<List<IPoolable>>> queue, int capacityOfTypes,
            int capacityOfObjects, GameObject behaviourPrefab)
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

        private void InitializeObjects(List<List<IPoolable>> listToInit, int capacityOfTypes, 
            int capacityOfObjects, GameObject behaviourPrefab)
        {
            // Double nested loop.
            // At least it would be in Awake. May be make it coroutine and add yield return null to make it on different
            // frames?
            for (int j = 0; j < capacityOfTypes; j++)
            {
                listToInit.Add(new List<IPoolable>(capacityOfObjects));
                for (int k = 0; k < capacityOfObjects; k++)
                {
                    Debug.Log(j);
                    listToInit[j].Add(behaviourPrefab.GetComponent<IPoolable>().Initialize(j));
                }
            }
        } 
        
        private void MoveObjects(List<List<IPoolable>> listToMove, int zOffset, ISegment objectsSegment, int index)
        {
            Vector3 offset = new Vector3(0, 0,levelSegments[index].Length * zOffset);
            int i = 0;
            int j = 0;
            
            // Nested loop. SegmentParts is small array, but still, need to think on it.
            
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