using UnityEngine;

namespace Pool.Platforms
{
    public interface IPlatform : IPoolable
    {
        public IPlatform Initialize(int index);

        public int GetPlatformTypeIndex();
    }
}