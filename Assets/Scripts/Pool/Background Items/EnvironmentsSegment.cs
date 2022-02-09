using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pool.Background_Items
{
    [CreateAssetMenu]
    public class EnvironmentsSegment : ScriptableObject, ISegment
    {
        public List<Vector3> Building2Small;
        public List<Vector3> Building3Small;
        public List<Vector3> Building3Big;
        public List<Vector3> Building4;
        public List<Vector3> Ground;

        private List<List<Vector3>> _segmentParts;

        public int ObjectsCapacity = 3;

        private void OnEnable()
        {
            _segmentParts = new List<List<Vector3>>(5)
            {
                Building2Small,
                Building3Small,
                Building3Big,
                Building4,
                Ground
            };
        }


        public List<List<Vector3>> GetSegmentParts => _segmentParts;
        public int GetTypesCount => _segmentParts.Count;
        public int GetObjectsCapacity => ObjectsCapacity;
    }
}