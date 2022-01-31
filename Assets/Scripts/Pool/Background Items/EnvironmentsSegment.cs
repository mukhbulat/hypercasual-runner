using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pool.Background_Items
{
    [CreateAssetMenu]
    public class EnvironmentsSegment : ScriptableObject, ISegment
    {
        public List<Vector3> Buildings;

        private List<List<Vector3>> _segmentParts;

        public int ObjectsCapacity = 10;

        private void OnEnable()
        {
            _segmentParts = new List<List<Vector3>>(1)
            {
                Buildings
            };
        }


        public List<List<Vector3>> GetSegmentParts => _segmentParts;
        public int GetTypesCount => _segmentParts.Count;
        public int GetObjectsCapacity => ObjectsCapacity;
    }
}