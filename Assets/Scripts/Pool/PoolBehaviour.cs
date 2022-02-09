﻿using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Pool
{
    public class PoolBehaviour : MonoBehaviour, IRestartable
    {
        #region FieldsAndProperties

        // Generic Fields
        [SerializeField] private List<LevelSegment> levelSegments;
        private int LevelSegmentsCount => levelSegments.Count;

        private int _currentSegmentNumber = 0;
        private int _nextSegmentNumber = 1;

        private LevelSegment _currentSegment;
        private LevelSegment _nextSegment;

        private bool _inMovingObjectsLoop;
        
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
        
        // Barriers
        [SerializeField] private GameObject barrierBehaviourPrefab;
        // This should be more than max in EnvironmentSegments.
        [SerializeField] private int barriersCapacity = 8;

        private Queue<List<List<IPoolable>>> _barriersQueue = new Queue<List<List<IPoolable>>>(3);


        #endregion
        #region UnityFuncs

        private void Awake()
        {
            // Getting two random segments to make first two segments.
            int i = Random.Range(0, LevelSegmentsCount);
            var firstSegment = levelSegments[i];
            int j = Random.Range(0, LevelSegmentsCount);
            var secondSegment = levelSegments[j];

            // Initializing queues ana making first two segments to appear.
            _currentSegment = firstSegment;
            _nextSegment = secondSegment;
            
            int platformTypesCount = levelSegments[0].Platforms.GetTypesCount;
            InitializeQueue(_platformsQueue, platformTypesCount,
                platformsCapacity, platformBehaviourPrefab);
            
            int collectableTypesCount = levelSegments[0].Collectables.GetTypesCount;
            InitializeQueue(_collectablesQueue, collectableTypesCount,
                collectablesCapacity, collectableBehaviourPrefab);
            
            int environmentTypesCount = levelSegments[0].Environments.GetTypesCount;
            InitializeQueue(_environmentsQueue, environmentTypesCount,
                environmentsCapacity, environmentBehaviourPrefab);

            int barriersTypesCount = levelSegments[0].Barriers.GetTypesCount;
            InitializeQueue(_barriersQueue, barriersTypesCount, 
                barriersCapacity, barrierBehaviourPrefab);
            
            StartCoroutine(RestartingPool(i, j, firstSegment, secondSegment));
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
                // If player passed 1/20 of current segment, dequeue previous segment and create new one in front.
                if (_currentSegmentNumber == _nextSegmentNumber && modulo > currentSegmentLength / 20)
                {
                    int nextSegmentIndex = Random.Range(0, LevelSegmentsCount);
                    _nextSegment = levelSegments[nextSegmentIndex];
                    _nextSegmentNumber += 1;
                    
                    _inMovingObjectsLoop = true;
                    
                    QueueMixing(_platformsQueue, nextSegmentIndex, _nextSegmentNumber, _nextSegment.Platforms);
                    yield return null;
                    QueueMixing(_collectablesQueue, nextSegmentIndex, _nextSegmentNumber, _nextSegment.Collectables);
                    yield return null;
                    QueueMixing(_environmentsQueue, nextSegmentIndex, _nextSegmentNumber, _nextSegment.Environments);
                    yield return null;
                }

                _inMovingObjectsLoop = false;
                yield return null;
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

        private IEnumerator WaitForLoopExitAndRestart()
        {
            while (_inMovingObjectsLoop)
            {
                yield return null;
            }
            ThirdPartOfRestart();
        }

        private void ThirdPartOfRestart()
        {
            StopAllCoroutines();

            // Getting two random segments to make first two segments.
            int i = Random.Range(0, LevelSegmentsCount);
            var firstSegment = levelSegments[i];
            int j = Random.Range(0, LevelSegmentsCount);
            var secondSegment = levelSegments[j];
            
            StartCoroutine(RestartingPool(i, j, firstSegment, secondSegment));
        }

        public void Restart()
        // Restart here in three parts. First: this one, called by interface. Second - coroutine, which waits for all 
        // queues (platforms, collectables, etc.) to be in one condition
        {
            StartCoroutine(WaitForLoopExitAndRestart());
        }

        private IEnumerator RestartingPool(int i, int j, LevelSegment firstSegment, LevelSegment secondSegment)
        {
            
            yield return null;
            // Initializing queues and making first two segments to appear.
            _currentSegment = firstSegment;
            _nextSegment = secondSegment;

            _currentSegmentNumber = 0;
            _nextSegmentNumber = 1;
            
            QueueMixing(_platformsQueue, i, 0, firstSegment.Platforms);
            QueueMixing(_platformsQueue, j, 1, secondSegment.Platforms);
            yield return null;
            
            QueueMixing(_collectablesQueue, i, 0, firstSegment.Collectables);
            QueueMixing(_collectablesQueue, j, 1, secondSegment.Collectables);
            yield return null;
            
            QueueMixing(_environmentsQueue, i, 0, firstSegment.Environments);
            QueueMixing(_environmentsQueue, j, 1, secondSegment.Environments);
            yield return null;
            
            QueueMixing(_barriersQueue, i, 0, firstSegment.Barriers);
            QueueMixing(_barriersQueue, j, 1, secondSegment.Barriers);
            yield return null;
            
            StartCoroutine(MoveObjectsLoop());
        }
    }
}