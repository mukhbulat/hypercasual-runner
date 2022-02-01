using System.Collections.Generic;
using UnityEngine;

namespace Pool.Background_Items
{
    public class EnvironmentBehaviour : MonoBehaviour, IPoolable
    {
        [SerializeField] private List<GameObject> childPrefabs;
        [SerializeField] private Vector3 positionOffset;
        [SerializeField] private Quaternion rotationOffset;
        [SerializeField] private GameObject groundPrefab;
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
            GameObject instance = Instantiate(gameObject, new Vector3(0, -10, 0), Quaternion.identity);
            instance.GetComponent<EnvironmentBehaviour>().SetChild(index);
            return instance.GetComponent<IPoolable>();
        }

        public int GetNumberOfTypesOfThis()
        {
            return childPrefabs.Count;
        }
        
        #endregion
        
        private void SetChild(int index)
        {
            if (index == -1)
            {
                Instantiate(groundPrefab, gameObject.transform);
            }
            else
            {
                Instantiate(childPrefabs[index], gameObject.transform);
            }
        }
    }
}