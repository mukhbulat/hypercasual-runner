using System.Collections.Generic;
using UnityEngine;

namespace Pool.Platforms
{
    public class PlatformBehaviour : MonoBehaviour, IPoolable
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

        public IPoolable Initialize(int index)
        {
            GameObject instance = Instantiate(gameObject);
            instance.GetComponent<PlatformBehaviour>().SetChild(index);
            return instance.GetComponent<IPoolable>();
        }
        
        
        public int GetNumberOfTypesOfThis()
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