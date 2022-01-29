using System.Collections.Generic;
using UnityEngine;

namespace Pool
{
    [CreateAssetMenu]
    public class NewLevelSegment : ScriptableObject
    {
        public List<List<Vector3>> ObjectsPositions;
    }
}