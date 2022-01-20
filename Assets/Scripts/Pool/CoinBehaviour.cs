using System;
using UnityEngine;
using DG.Tweening;

namespace Pool
{
    public class CoinBehaviour : MonoBehaviour, ICollectable, IPoolable
    {
        
        [SerializeField] private Inventory inventory;

        private bool isX2 = false;
        
        #region Interfaces
        
        public void Obtain()
        {
            if (isX2)
            {
                inventory.Coins += 2;
            }
            else
            {
                inventory.Coins += 1;
            }
            transform.position = Vector3.zero;
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

        private void Awake()
        {
            inventory = GameObject.FindWithTag("Player").GetComponent<Inventory>();
        }

        private void Update()
        {
            //double transformX = transform.position.x;
            //transform.DORotate(new Vector3(0, 360, 0), 3);
            //DOTween.To(() => transformX, x => transformX = x, 1.5, 1);
        }
    }
}