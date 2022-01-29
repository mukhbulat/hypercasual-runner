using System;
using System.Collections.Generic;
using Pool.Collectables;
using Pool.Platforms;
using UnityEngine;

namespace Pool
{
    public class NewPoolBehaviour : MonoBehaviour
    {
        #region Fields and properties
        // Generic fields
        [SerializeField] private List<LevelSegment> levelSegments;
        [SerializeField] private int lengthOfSegment = 30;
        [SerializeField] private int offsetToMoveTheSegment = 10;
        // Player
        [SerializeField] private Transform player;
        private float PlayerZPosition => player.transform.position.z;
        #endregion
        // Platforms fields
        /*

        [SerializeField] private GameObject platformPrefab;
        
        [SerializeField] private int platformsCapacity = 50;

        private Queue<List<List<IPlatform>>> _platformsInSegments = new Queue<List<List<IPlatform>>>(3);
        private int currentSegment;
        private int nextSegment = 1;

        private int NumberOfTypesOfPlatforms => platformPrefab.GetComponent<IPlatform>().GetNumberOfTypesOfPlatforms();

        // Collectables fields
        [SerializeField] private GameObject collectablesPrefab;
        [SerializeField] private int collectablesCapacity = 20;
        private Queue<List<List<ICollectable>>> _collectablesInSegments = new Queue<List<List<ICollectable>>>(3);

        
        private int NumberOfTypesOfCollectables =>
            collectablesPrefab.GetComponent<ICollectable>().GetNumberOfTypesOfCollectables();
        
        #endregion

        /*

        private void Awake()
        {
            //Absolutely fucking same. Need generics or something else.
            // Platforms
            InitializePlatformsQueue();
            var temporaryPlatformsList = _platformsInSegments.Dequeue();
            MovePlatforms(temporaryPlatformsList, 0);
            _platformsInSegments.Enqueue(temporaryPlatformsList);
            temporaryPlatformsList = _platformsInSegments.Dequeue();
            MovePlatforms(temporaryPlatformsList, 1 * lengthOfSegment);
            _platformsInSegments.Enqueue(temporaryPlatformsList);
            
            // Collectables
            InitializeCollectableQueue();
            var temporaryCollectablesList = _collectablesInSegments.Dequeue();
            MoveCollectables(temporaryCollectablesList, 0);
            _collectablesInSegments.Enqueue(temporaryCollectablesList);
            temporaryCollectablesList = _collectablesInSegments.Dequeue();
            MoveCollectables(temporaryCollectablesList, 1 * lengthOfSegment);
            _collectablesInSegments.Enqueue(temporaryCollectablesList);
        }

        private void Update()
        {
            float modulo = PlayerZPosition % lengthOfSegment;
            currentSegment = (int) PlayerZPosition / lengthOfSegment;
            if (currentSegment == nextSegment && modulo > offsetToMoveTheSegment)
            {
                nextSegment += 1;
                var temporaryPlatformSegment = _platformsInSegments.Dequeue();
                MovePlatforms(temporaryPlatformSegment, nextSegment * lengthOfSegment);
                _platformsInSegments.Enqueue(temporaryPlatformSegment);
                var temporaryCollectablesSegment = _collectablesInSegments.Dequeue();
                MoveCollectables(temporaryCollectablesSegment, nextSegment * lengthOfSegment);
                _collectablesInSegments.Enqueue(temporaryCollectablesSegment);
            }
        }

        
        #region Platforms

        private void InitializePlatformsQueue()
        {
            for (int i = 0; i < 3; i++)
            {
                _platformsInSegments.Enqueue(new List<List<IPlatform>>(NumberOfTypesOfPlatforms));
            }

            foreach (var list in _platformsInSegments)
            {
                InitializePlatforms(list);
            }
        }
        
        private void InitializePlatforms(List<List<IPlatform>> listToInit)
        {
            // Double Nested Loop. :(
            // At least it would be in Awake. May be make it coroutine and add yield return null to make it on different
            // frames?
            for (int j = 0; j < NumberOfTypesOfPlatforms; j++)
            {
                listToInit.Add(new List<IPlatform>(platformsCapacity));
                for (int k = 0; k < platformsCapacity; k++)
                {
                    //listToInit[j].Add(platformPrefab.GetComponent<IPlatform>().Initialize(j));
                }
            }
        }

        private void MovePlatforms(List<List<IPlatform>> listToMove, int zOffset)
        {
            // Dummy. Make random after making more than one of levelSegments.
            var levelSegment = levelSegments[0];

            Vector3 offset = new Vector3(0, 0, zOffset);
            int i = 0;
            foreach (var position in levelSegment.X3Platforms)
            {
                listToMove[0][i].MoveForward(position + offset);
                i += 1;
            }
            
            i = 0;
            
            foreach (var position in levelSegment.X5Platforms)
            {
                listToMove[1][i].MoveForward(position + offset);
                i += 1;
            }
            
            i = 0;

            foreach (var position in levelSegment.X7Platforms)
            {
                listToMove[2][i].MoveForward(position + offset);
                i += 1;
            }

            i = 0;
        }

        #endregion

        #region Collectables

        private void InitializeCollectableQueue()
        // Weird. Need to make the same as platforms Queue.
        {
            for (int i = 0; i < 3; i++)
            {
                _collectablesInSegments.Enqueue(new List<List<ICollectable>>(NumberOfTypesOfCollectables));
            }

            foreach (var list in _collectablesInSegments)
            {
                InitializeCollectables(list);
            }
        }

        private void InitializeCollectables(List<List<ICollectable>> listToInit)
        // This is totally can be made with generics.
        {
            for (int j = 0; j < NumberOfTypesOfCollectables; j++)
            {
                listToInit.Add(new List<ICollectable>());
                for (int k = 0; k < collectablesCapacity; k++)
                {
                   //listToInit[j].Add(collectablesPrefab.GetComponent<ICollectable>().Initialize(j));
                }
            }
        }

        private void MoveCollectables(List<List<ICollectable>> listToMove, int zOffset)
        {
            // Dummy. Wait to make more segments
            var levelSegment = levelSegments[0];
            
            Vector3 offset = new Vector3(0, 0, zOffset);
            int i = 0;
            foreach (var position in levelSegment.Coins)
            {
                // [0] is for coins. Each list in listToMove represents list in levelSegment of certain collectables.
                listToMove[0][i].MoveForward(position + offset);
                i += 1;
            }
        }
        #endregion
        */
    }
}