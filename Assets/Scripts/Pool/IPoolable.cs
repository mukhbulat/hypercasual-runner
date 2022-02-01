using UnityEngine;

namespace Pool
{
    public interface IPoolable
    {
        public void MoveForward(Vector3 newPosition);
        public float GetZPosition();
        // Not used, left it if I need to use it. :(
        public IPoolable Initialize(int index);
        // Making instance at awake in PoolBehaviour, object is far down
        // index is for prefabs index, at the moment, all prefabs have the same number of objects.
        // Instancing occurs under the floor.
        public int GetNumberOfTypesOfThis();
    }
}