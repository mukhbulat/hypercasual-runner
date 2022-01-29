using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pool.Collectables
{
    [CreateAssetMenu]
    public class CollectablesSegment : ScriptableObject, ISegment
    {
        public List<Vector3> Coins;

        public int CollectableTypesCount = 1;
        public int CollectableObjectsCapacity = 20;
            
        private List<List<Vector3>> SegmentParts;

        private void OnEnable()
        {
            SegmentParts = new List<List<Vector3>>(1)
            {
                Coins
            };
        }


        public List<List<Vector3>> GetSegmentParts => SegmentParts;
        public int GetTypesCount => CollectableTypesCount;
        public int GetObjectsCapacity => CollectableObjectsCapacity;
    }
}