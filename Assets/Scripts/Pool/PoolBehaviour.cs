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
        
        // The Only thing I know for real about Player. 
        [SerializeField] private Transform playerTransform;
        private float PlayerZPosition => playerTransform.transform.position.z;
        
        // General fields
        [SerializeField] private float zOffsetBack = 10f;
        [SerializeField] private float zOffsetForward = 20f;

        // Platforms
        // Platforms Serialized Fields.
        [SerializeField] private List<GameObject> platformPrefabs;

        [SerializeField] private int platformsListsCapacity = 20;
        [SerializeField] private float floorToPlatformLengthRatio = 2;
        // The less - the easier.
        [SerializeField] private float timeToSpawnPlatform = 0.2f;
        [SerializeField] private int heightBetweenPlatforms = 3;
        [SerializeField] private int distanceBetweenPlatforms = 8;
        [SerializeField] private int zMaxRandomOffset = 4;
        [SerializeField] private int floorHeight = 12;
        
        // Platform private fields.
        private List<IPoolable> _platformsOnFirstFloor;
        private List<IPoolable> _platformsOnSecondFloor;
        private List<IPoolable> _platformsOnThirdFloor;
        
        // Coins
        // Coins Serialized Fields.
        [SerializeField] private GameObject coinPrefab;
        [SerializeField] private int[] coinsCapacity = {20, 30, 40};
        [SerializeField] private float timeToSpawnCoin = 0.1f;

        // Coins private fields.
        private List<IPoolable> _coinsOnFirstFloor;
        private List<IPoolable> _coinsOnSecondFloor;
        private List<IPoolable> _coinsOnThirdFloor;

        #endregion
        
        private void Awake()
        {
            // Would lag at awake. May be coroutine for three initializes?
            // Platforms
            _platformsOnFirstFloor = new List<IPoolable>(platformsListsCapacity);
            _platformsOnSecondFloor = new List<IPoolable>(platformsListsCapacity);
            _platformsOnThirdFloor = new List<IPoolable>(platformsListsCapacity);
            InitializePlatforms(_platformsOnFirstFloor, 1);
            InitializePlatforms(_platformsOnSecondFloor, 2);
            InitializePlatforms(_platformsOnThirdFloor, 3);
            // Coins
            _coinsOnFirstFloor = new List<IPoolable>(coinsCapacity[0]);
            _coinsOnSecondFloor = new List<IPoolable>(coinsCapacity[1]);
            _coinsOnThirdFloor = new List<IPoolable>(coinsCapacity[2]);
            InitializeCoins(_coinsOnFirstFloor, 1);
            InitializeCoins(_coinsOnSecondFloor, 2);
            InitializeCoins(_coinsOnThirdFloor, 3);
            
        }

        private void Update()
        {
            StartCoroutine(MovePlatformsForward(_platformsOnFirstFloor, 1));
        }

        private void InitializeCoins(List<IPoolable> listToInitialize, int floor)
        { 
            for (int i = 0; i < coinsCapacity[floor - 1]; i++)
            {
                listToInitialize.Add(coinPrefab.GetComponent<IPoolable>().Initialize().GetComponent<IPoolable>());
            }
        }

        private IEnumerator MoveCoinsForward(List<IPoolable> coins, int floor)
        {
            for (int i = 0; i < floor; i++)
            {
                yield return null;
            }

            while (true)
            {
                foreach (var coin in coins)
                {
                    if (coin.GetZPosition() < PlayerZPosition - zOffsetBack)
                    {
                        coin.MoveForward(FindNewPositionForCoin(floor));
                    }

                    yield return null;
                }

                yield return null;
            }
        }

        private Vector3 FindNewPositionForCoin(int floor)
        {
            // TODO finding new position on platform. May be easier to remake platform positioning first.
            return Vector3.zero;
        }
        
        #region Platforms
        
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

        #endregion
    }
}