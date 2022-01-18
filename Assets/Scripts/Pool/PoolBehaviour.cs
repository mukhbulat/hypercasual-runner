using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Pool
{
    public class PoolBehaviour : MonoBehaviour
    {
        // Platform private fields.
        private List<IPoolable> _platformsOnFirstFloor;
        private List<IPoolable> _platformsOnSecondFloor;
        private List<IPoolable> _platformsOnThirdFloor;
        
        // The Only thing I know for real about Player. 
        [SerializeField] private Transform playerTransform;
        private float PlayerZPosition => playerTransform.transform.position.z; 
        
        // Platforms Serialized Fields.
        [SerializeField] private List<GameObject> platformPrefabs;

        [SerializeField] private int platformsListsCapacity = 20;
        [SerializeField] private float floorToPlatformLengthRatio = 2;
        // The less - the easier.
        [SerializeField] private float zOffsetBack = 10f;
        [SerializeField] private float timeToSpawnPlatform = 0.2f;
        [SerializeField] private int heightBetweenPlatforms = 3;
        [SerializeField] private int distanceBetweenPlatforms = 8;
        [SerializeField] private float zOffsetForward = 20f;
        [SerializeField] private int zMaxRandomOffset = 4;
        [SerializeField] private int floorHeight = 12;
        //[SerializeField] private int heightOfPlatform = 1;
        
        private void Awake()
        {
            // Would lag at awake. May be coroutine for three initializes?
            _platformsOnFirstFloor = new List<IPoolable>(platformsListsCapacity);
            _platformsOnSecondFloor = new List<IPoolable>(platformsListsCapacity);
            _platformsOnThirdFloor = new List<IPoolable>(platformsListsCapacity);
            InitializePlatforms(_platformsOnFirstFloor, 1);
            InitializePlatforms(_platformsOnSecondFloor, 2);
            InitializePlatforms(_platformsOnThirdFloor, 3);
        }

        private void Update()
        {
            StartCoroutine(MovePlatformsForward(_platformsOnFirstFloor, 1));
        }

        private void InitializePlatforms(List<IPoolable> listToInitialize, int floor)
        {
            float randomLengthRatio = 1 / (floor * floorToPlatformLengthRatio);
            for (int i = 0; i < platformsListsCapacity; i ++)
            {
                // Bad! Could be worse.
                // At least this is not in update.
                if (Random.value < randomLengthRatio)
                {
                    listToInitialize.Add(platformPrefabs[0].GetComponent<IPoolable>().Initialize().GetComponent<IPoolable>());
                }
                else if (Random.value < randomLengthRatio)
                {
                    listToInitialize.Add(platformPrefabs[1].GetComponent<IPoolable>().Initialize().GetComponent<IPoolable>());
                }
                else 
                {
                    listToInitialize.Add(platformPrefabs[2].GetComponent<IPoolable>().Initialize().GetComponent<IPoolable>());
                }
            }
        }

        private IEnumerator MovePlatformsForward(List<IPoolable> platforms, int floor)
        {
            while (true)
            {
                foreach (var platform in platforms)
                {
                    if (platform.GetZPosition() < PlayerZPosition - zOffsetBack)
                    {
                        platform.MoveForward(FindNewPositionForPlatform(floor));
                    }

                    yield return null;
                }
                yield return new WaitForSeconds(timeToSpawnPlatform);
            }
        }

        private Vector3 FindNewPositionForPlatform(int floor)
        // The whole system shouldn't be random. I need to draw like 10 variations for each floor
        // and randomly select one of them for the next sector of the gameplay.
        // So, the whole method is dummy for the later system. Event the whole class.
        {
            int maxY = floor * floorHeight;
            int minY = (floor - 1) * floorHeight;
            int yMaxRandomOffset = maxY / heightBetweenPlatforms - 1;
            int zCurrentRandomOffset = Random.Range(1, zMaxRandomOffset) * distanceBetweenPlatforms;
            int yCurrentRandomOffset = Random.Range(minY + 1, yMaxRandomOffset + 1);
            yCurrentRandomOffset = yCurrentRandomOffset * heightBetweenPlatforms + minY + yCurrentRandomOffset - 1;
            if (yCurrentRandomOffset > maxY) throw new Exception($"What the fuck {yCurrentRandomOffset}");
            return new Vector3(0, yCurrentRandomOffset, PlayerZPosition + zOffsetForward + zCurrentRandomOffset);
        }
        
    }
}