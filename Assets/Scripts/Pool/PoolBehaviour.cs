using System.Collections.Generic;
using UnityEngine;

namespace Pool
{
    public class PoolBehaviour : MonoBehaviour
    {
        [SerializeField] private List<GameObject> platformPrefabs;
        private Queue<PlatformBehaviour> _platformsInGame = new Queue<PlatformBehaviour>(60);

        private void TakePlatformOutOfPool(int number)
        {
            
        }

        private void CreateQueue()
        {
            int everyPlatformTypeCount = _platformsInGame.Count / platformPrefabs.Count;
            for (int i = 0; i < platformPrefabs.Count; i++)
            {
                for (int j = 0; j < everyPlatformTypeCount; j++)
                {
                    _platformsInGame.Enqueue(platformPrefabs[i].GetComponent<PlatformBehaviour>());
                }
            }
        }
        
    }
}