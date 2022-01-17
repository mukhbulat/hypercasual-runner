using UnityEngine;

namespace Pool
{
    public class PlatformBehaviour : MonoBehaviour, IPoolable
    {
        #region Interfaces

        public void MoveForward()
        {
            
        }

        public GameObject Initialize()
        {
            return Instantiate(gameObject);
        }
        
        #endregion

    }
}