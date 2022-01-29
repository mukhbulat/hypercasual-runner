using System.Collections.Generic;
using UnityEngine;

namespace Pool.Platforms
{
    [CreateAssetMenu]
    public class PlatformsSegment : ScriptableObject, ISegment
    {
        public List<Vector3> X3Platforms;
        public List<Vector3> X5Platforms;
        public List<Vector3> X7Platforms;

        public int PlatformTypesCount = 3;
        public int PlatformObjectsCapacity = 20;
        
        private List<List<Vector3>> SegmentParts;
        private void OnEnable()
        {
            SegmentParts = new List<List<Vector3>>(PlatformTypesCount)
            {
                X3Platforms,
                X5Platforms,
                X7Platforms
            };
        }

        public List<List<Vector3>> GetSegmentParts => SegmentParts;
        public int GetTypesCount => PlatformTypesCount;
        public int GetObjectsCapacity => PlatformObjectsCapacity;
    }
}