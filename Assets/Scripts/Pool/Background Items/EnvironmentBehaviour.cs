using System.Collections.Generic;
using UnityEngine;

namespace Pool.Background_Items
{
    public class EnvironmentBehaviour : MonoBehaviour, IPoolable
    {
        [SerializeField] private List<GameObject> childPrefabs;
        [SerializeField] private List<Vector3> positionOffset;
        [SerializeField] private List<Quaternion> rotationOffset;

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
            throw new System.NotImplementedException();
        }

        public int GetNumberOfTypesOfThis()
        {
            throw new System.NotImplementedException();
        }
        
        #endregion
        
        
        private void SetChild(int index)
        {
            Instantiate(childPrefabs[index], positionOffset[index], rotationOffset[index], gameObject.transform);
        }

    }
}