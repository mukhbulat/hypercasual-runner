using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pool.Collectables
{
    public class CollectableBehaviour : MonoBehaviour, IPoolable
    {
        
        [SerializeField] private List<GameObject> childPrefabs;
        private PlayerStats _playerStats;

        [SerializeField] private float particleLifetime = 0.5f;
        private ParticleSystem _particleSystem;
        private MeshRenderer _childMeshRenderer;
        
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
            StartCoroutine(ParticleEffectAndMove());
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
            _childMeshRenderer = Instantiate(childPrefabs[index], gameObject.transform).GetComponent<MeshRenderer>();
        }

        private void Awake()
        {
            _playerStats = GameObject.Find("PlayerStats").GetComponent<PlayerStats>();
            _playerStats.DoubleCoins += OnDoubleCoins;
            _particleSystem = GetComponent<ParticleSystem>();
        }

        private IEnumerator ParticleEffectAndMove()
        {
            _particleSystem.Play();
            _childMeshRenderer.enabled = false;
            yield return new WaitForSeconds(particleLifetime);
            _particleSystem.Stop();
            transform.position = new Vector3(0, -10, 0);
            _childMeshRenderer.enabled = true;
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