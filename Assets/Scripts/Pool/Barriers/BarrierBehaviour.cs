using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pool.Barriers
{
    public class BarrierBehaviour : MonoBehaviour, IPoolable
    {
        [SerializeField] private List<GameObject> childPrefabs;

        private PlayerMovement _playerMovement;

        private void Awake()
        {
            _playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        }

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
            GameObject instance = Instantiate(gameObject, new Vector3(0, -30, 0), Quaternion.identity);
            instance.GetComponent<BarrierBehaviour>().SetChild(index);
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
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_playerMovement.IsEnabled)
            {
                _playerMovement.BarrierHit();
            }
        }

    }
}