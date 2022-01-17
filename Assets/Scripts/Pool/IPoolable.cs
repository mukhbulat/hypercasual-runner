using UnityEngine;

namespace Pool
{
    public interface IPoolable
    {
        public GameObject Initialize();
        public void MoveForward();
    }
}