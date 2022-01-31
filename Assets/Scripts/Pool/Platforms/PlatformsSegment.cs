using System;
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

        public int PlatformObjectsCapacity = 20;
        
        private List<List<Vector3>> _segmentParts;
        private void OnEnable()
        {
            _segmentParts = new List<List<Vector3>>(3)
            {
                X3Platforms,
                X5Platforms,
                X7Platforms
            };
            Vector3 abraCadabra = Vector3.back;
            abraCadabra.ToString();
        }

        public List<List<Vector3>> GetSegmentParts => _segmentParts;
        public int GetTypesCount => _segmentParts.Count;
        public int GetObjectsCapacity => PlatformObjectsCapacity;
    }
}