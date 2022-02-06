using System.Collections.Generic;
using UnityEngine;

namespace Pool.Collectables
{
    public class CollectableBehaviour : MonoBehaviour, IPoolable
    {
        
        [SerializeField] private List<GameObject> childPrefabs;
        private PlayerStats _playerStats;

        private bool _isDoubleCoins;
        
        #region Interfaces
        
        private void Obtain()
        {
            if (_isDoubleCoins)
            {
                _playerStats.Coins += 2;
            }
            else
            {
                _playerStats.Coins += 1;
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
            _playerStats = GameObject.Find("PlayerStats").GetComponent<PlayerStats>();
            _playerStats.DoubleCoins += OnDoubleCoins;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerMovement>() != null)
            {
                Obtain();
            }
            else
            {
                Debug.Log($"PlayerMovement is not found on this {other}");
            }
        }
    }
}