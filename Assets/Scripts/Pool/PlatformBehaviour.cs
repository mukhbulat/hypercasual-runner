using UnityEngine;

namespace Pool
{
    public class PlatformBehaviour : MonoBehaviour, IPoolable
    {
        #region Interfaces

        public bool IsActive()
        {
            return gameObject.activeInHierarchy;
        }

        public GameObject Initialize()
        {
            return null;
        }
        
        #endregion

    }
}