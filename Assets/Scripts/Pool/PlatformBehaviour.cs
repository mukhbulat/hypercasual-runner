using UnityEngine;

namespace Pool
{
    public class PlatformBehaviour : MonoBehaviour, IPoolable
    {
        #region Interfaces

        public void MoveForward(Vector3 newPosition)
        {
            transform.position = newPosition;
        }

        public GameObject Initialize()
        {
            return Instantiate(gameObject);
        }

        public float GetZPosition()
        {
            return transform.position.z;
        }

        #endregion

    }
}