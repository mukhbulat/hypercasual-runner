using System.Collections.Generic;
using UnityEngine;

namespace Pool.Collectables
{
    public class CollectableBehaviour : MonoBehaviour, IPoolable
    {
        
        [SerializeField] private List<GameObject> childPrefabs;
        private Inventory _inventory;

        private bool _isDoubleCoins = false;
        
        #region Interfaces
        
        public void Obtain()
        {
            if (_isDoubleCoins)
            {
                _inventory.Coins += 2;
            }
            else
            {
                _inventory.Coins += 1;
            }
            transform.position = Vector3.zero;
        }

        public IPoolable Initialize(int index)
        {
            GameObject instance = Instantiate(gameObject, new Vector3(0, -10, 0), Quaternion.identity);
            instance.GetComponent<CollectableBehaviour>().SetChild(index);
            return instance.GetComponent<IPoolable>();
        }

        public int GetNumberOfTypesOfThis()
        {
            return childPrefabs.Count;
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

        private void OnDoubleCoins(bool isActive)
        {
            _isDoubleCoins = isActive;
        }

        private void SetChild(int index)
        {
            Instantiate(childPrefabs[index], gameObject.transform);
        }

        private void Awake()
        {
            // Because I need reference of inventory in this script anyway, I think, it's better to use event.
            _inventory = GameObject.FindWithTag("Player").GetComponent<Inventory>();
            _inventory.DoubleCoins += OnDoubleCoins;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Inventory>() != null)
            {
                Obtain();
            }
        }
    }
}