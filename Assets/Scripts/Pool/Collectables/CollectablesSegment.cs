using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pool.Collectables
{
    [CreateAssetMenu]
    public class CollectablesSegment : ScriptableObject, ISegment
    {
        public List<Vector3> Coins;

        public int CollectableObjectsCapacity = 20;
        
        private List<List<Vector3>> _segmentParts;

        private void OnEnable()
        {
            _segmentParts = new List<List<Vector3>>(1)
            {
                Coins
            };
        }


        public List<List<Vector3>> GetSegmentParts => _segmentParts;
        public int GetTypesCount => _segmentParts.Count;
        public int GetObjectsCapacity => CollectableObjectsCapacity;
    }
}