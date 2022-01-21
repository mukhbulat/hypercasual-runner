using System;
using System.Collections;
using System.Collections.Generic;
using Pool.Platforms;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Pool
{
    public class PoolBehaviour : MonoBehaviour
    {
        #region FieldsAndProperties
        
        // The Only thing I know for real about Player. 
        [SerializeField] private Transform playerTransform;
        private float PlayerZPosition => playerTransform.transform.position.z;
        
        // General fields
        [SerializeField] private float zOffsetBack = 10f;
        [SerializeField] private float zOffsetForward = 20f;

        // Platforms
        // Platforms Serialized Fields.
        [SerializeField] private GameObject platformPrefab;
        [SerializeField] private List<LevelSegment> levelSegments;

        // Need to change capacity after adding new segment with more platforms than in capacity.
        // May be first take all the LevelSegments, count platforms and make capacity to fill all the platforms.
        [SerializeField] private int numberOfTypesOfPlatforms = 3;
        [SerializeField] private int platformsCapacity = 40;
        
        // Platform private fields.
        // Queue consists of three lists: previous segment of platforms, current and next.
        private Queue<List<IPlatform>> _platforms = new Queue<List<IPlatform>>(3);

        // Coins
        // Coins Serialized Fields.
        [SerializeField] private GameObject coinPrefab;
        [SerializeField] private int[] coinsCapacity = {20, 30, 40};
        [SerializeField] private float timeToSpawnCoin = 0.1f;

        // Coins private fields.
        #endregion
        
        private void Awake()
        {
            // Would lag at awake. May be coroutine for three initializes?
            // Platforms
            for (int i = 0; i < 3; i++)
            {
                _platforms.Enqueue(new List<IPlatform>(platformsCapacity));
                InitializePlatforms(_platforms.Peek());
            }
            PositionPlatforms(_platforms.Peek());
        }

        #region Platforms
        
        /*
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
            for (int i = 0; i < floor; i++)
            {
                yield return null;
            }
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
        */

        private void InitializePlatforms(List<IPlatform> listToInitialize)
        {
            // Dummy. Change, when add new segments.
            var levelSegment = levelSegments[0];
            // platformPrefabs: 0 - X3platform prefab, 1 - X5platform prefab, 2 - X7 platform prefab.
            foreach (var position in levelSegment.X7Platforms)
            {
                listToInitialize.Add(platformPrefab.GetComponent<IPlatform>().Initialize(0));
            }

            foreach (var position in levelSegment.X5Platforms)
            {
                listToInitialize.Add(platformPrefab.GetComponent<IPlatform>().Initialize(1));
            }

            foreach (var position in levelSegment.X3Platforms)
            {
                listToInitialize.Add(platformPrefab.GetComponent<IPlatform>().Initialize(2));
            }
        }
        private void PositionPlatforms(List<IPlatform> listWithPlatforms)
        {
            // Dummy. Make random after couple of segments.
            var levelSegment = levelSegments[0];
            int numberOfX3 = 0;
            int numberOfX5 = 0;
            int numberOfX7 = 0;
            
            // Piece of shit.
            // Takes a platform and moves it while the list of this type platforms in levelSegment is not ended.
            foreach (var platform in listWithPlatforms)
            {
                switch (platform.GetPlatformTypeIndex())
                {
                    case 0:
                    {
                        if (numberOfX3 < levelSegment.X3Platforms.Count)
                        {
                            platform.MoveForward(levelSegment.X3Platforms[numberOfX3]);
                            numberOfX3 += 1;
                        }
                        break;
                    }
                    case 1:
                    {
                        if (numberOfX5 < levelSegment.X5Platforms.Count)
                        {
                            platform.MoveForward(levelSegment.X5Platforms[numberOfX5]);
                            numberOfX5 += 1;
                        }
                        break;
                    }
                    case 2:
                    {
                        if (numberOfX7 < levelSegment.X7Platforms.Count)
                        {
                            platform.MoveForward(levelSegment.X7Platforms[numberOfX7]);
                            numberOfX7 += 1;
                        }
                        break;
                    }
                    case -1:
                    {
                        throw new Exception("Platform is not initialized");
                    }
                }
            }

            foreach (var x3Platform in levelSegment.X3Platforms)
            {
                
            }
            
        }

        #endregion
    }
}