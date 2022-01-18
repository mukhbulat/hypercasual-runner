using UnityEngine;

namespace Pool
{
    public class CoinBehaviour : MonoBehaviour, ICollectable, IPoolable
    {
        
        [SerializeField] private Inventory inventory;

        private bool isX2 = false;
        
        #region Interfaces
        
        public void Obtain()
        {
            inventory.Coins += 1;
        }

        public GameObject Initialize()
        {
            return Instantiate(gameObject);
        }

        public void MoveForward(Vector3 newPosition)
        {
            transform.position = newPosition;
        }

        public float GetZPosition()
        {
            return transform.position.z;
        }
        
        #endregion

        private void OnX2Get()
        {
            isX2 = true;
        }

        private void OnX2Lose()
        {
            isX2 = false;
        }
    }
}