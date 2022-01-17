using UnityEngine;

namespace Pool
{
    public interface IPoolable
    {
        public GameObject Initialize();
        public void MoveForward(Vector3 newPosition);
        public float GetPosition();
    }
}