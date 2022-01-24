using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Pool
{
    public class CoinBehaviour : MonoBehaviour, ICollectable
    {
        
        [SerializeField] private Inventory inventory;
        [SerializeField] private List<GameObject> prefabs;
        
        

        private bool _isDoubleCoins = false;
        
        #region Interfaces
        
        public void Obtain()
        {
            if (_isDoubleCoins)
            {
                inventory.Coins += 2;
            }
            else
            {
                inventory.Coins += 1;
            }
            transform.position = Vector3.zero;
        }

        public ICollectable Initialize(int index)
        {
            GameObject instance = Instantiate(gameObject);
            instance.GetComponent<CoinBehaviour>().SetChild(index);
            return instance.GetComponent<ICollectable>();
        }

        public int GetNumberOfTypesOfCollectables()
        {
            return prefabs.Count;
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
            Instantiate(prefabs[index], gameObject.transform);
        }

        private void Awake()
        {
            inventory = GameObject.FindWithTag("Player").GetComponent<Inventory>();
            inventory.DoubleCoins += OnDoubleCoins;
        }

        private void Update()
        {
            //double transformX = transform.position.x;
            //transform.DORotate(new Vector3(0, 360, 0), 3);
            //DOTween.To(() => transformX, x => transformX = x, 1.5, 1);
        }
    }
}