using System.Collections.Generic;
using UnityEngine;

namespace Pool
{
    public interface ISegment
    {
        public List<List<Vector3>> GetSegmentParts { get; }
        public int GetTypesCount { get; }
        public int GetObjectsCapacity { get; }
    }
}