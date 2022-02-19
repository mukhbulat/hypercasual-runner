using System.Collections.Generic;
using UnityEngine;

namespace Pool.Barriers
{
    [CreateAssetMenu]
    public class BarrierSegment : ScriptableObject, ISegment
    {
        public List<Vector3> SmallBarrier;

        private List<List<Vector3>> _segmentParts;

        public int ObjectsCapacity = 3;

        private void OnEnable()
        {
            _segmentParts = new List<List<Vector3>>(1)
            {
                SmallBarrier
            };
        }

        public List<List<Vector3>> GetSegmentParts => _segmentParts;
        public int GetTypesCount => _segmentParts.Count;
        public int GetObjectsCapacity => ObjectsCapacity;


    }
}