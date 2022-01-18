using UnityEngine;

namespace Pool
{
    public class CoinBehaviour : MonoBehaviour, ICollectable, IPoolable
    {
        #region Interfaces
        
        public void Obtain()
        {
            throw new System.NotImplementedException();
        }

        public GameObject Initialize()
        {
            throw new System.NotImplementedException();
        }

        public void MoveForward(Vector3 newPosition)
        {
            throw new System.NotImplementedException();
        }

        public float GetZPosition()
        {
            throw new System.NotImplementedException();
        }
        
        #endregion
    }
}