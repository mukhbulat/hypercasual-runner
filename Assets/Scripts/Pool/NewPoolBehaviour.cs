using System;
using System.Collections.Generic;
using Pool.Platforms;
using UnityEngine;

namespace Pool
{
    public class NewPoolBehaviour : MonoBehaviour
    {
        #region Fields and properties
        // Player
        [SerializeField] private Transform player;
        private float PlayerZPosition => player.transform.position.z;
        
        // Platforms fields

        [SerializeField] private GameObject platformPrefab;
        [SerializeField] private List<LevelSegment> levelSegments;
        
        [SerializeField] private int lengthOfSegment = 30;
        [SerializeField] private int offsetToMoveTheSegment = 10;
        [SerializeField] private int platformsCapacity = 50;

        private Queue<List<List<IPlatform>>> _platformsInSegments = new Queue<List<List<IPlatform>>>(3);
        private int currentSegment;
        private int nextSegment = 1;

        private int NumberOfTypesOfPlatforms => platformPrefab.GetComponent<IPlatform>().GetNumberOfTypesOfPlatforms();

        #endregion

        //

        private void Awake()
        {
            InitializeQueue();
            var temporaryList = _platformsInSegments.Dequeue();
            MovePlatforms(temporaryList, 0);
            _platformsInSegments.Enqueue(temporaryList);
            temporaryList = _platformsInSegments.Dequeue();
            MovePlatforms(temporaryList, 1 * lengthOfSegment);
            _platformsInSegments.Enqueue(temporaryList);
        }

        private void Update()
        {
            float modulo = PlayerZPosition % lengthOfSegment;
            currentSegment = (int) PlayerZPosition / lengthOfSegment;
            if (currentSegment == nextSegment && modulo > offsetToMoveTheSegment)
            {
                nextSegment += 1;
                var temporarySegment = _platformsInSegments.Dequeue();
                MovePlatforms(temporarySegment, nextSegment * lengthOfSegment);
                _platformsInSegments.Enqueue(temporarySegment);
            }
        }

        #region Platforms

        private void InitializeQueue()
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
                    listToInit[j].Add(platformPrefab.GetComponent<IPlatform>().Initialize(j));
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
    }
}