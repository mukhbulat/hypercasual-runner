using System.Collections.Generic;
using UnityEngine;

namespace Pool.Platforms
{
    public class PlatformBehaviour : MonoBehaviour, IPlatform
    {
        [SerializeField] private List<GameObject> childPrefabs;
        private int _platformTypeIndex = -1;
            
        #region Interfaces

        public void MoveForward(Vector3 newPosition)
        {
            transform.position = newPosition;
        }

        public float GetZPosition()
        {
            return transform.position.z;
        }

        public IPlatform Initialize(int index)
        {
            GameObject instance = Instantiate(gameObject);
            instance.GetComponent<PlatformBehaviour>().SetChild(index);
            return instance.GetComponent<IPlatform>();
        }

        public int GetPlatformTypeIndex()
        {
            return _platformTypeIndex;
        }

        public int GetNumberOfTypesOfPlatforms()
        {
            return childPrefabs.Count;
        }

        #endregion

        private void SetChild(int index)
        {
            Instantiate(childPrefabs[index], gameObject.transform);
            _platformTypeIndex = index;
        }
    }
}