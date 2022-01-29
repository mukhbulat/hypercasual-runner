using UnityEngine;

namespace Pool
{
    public interface IPoolable
    {
        public void MoveForward(Vector3 newPosition);
        public float GetZPosition();
        public IPoolable Initialize(int index);
        public int GetNumberOfTypesOfThis();
    }
}