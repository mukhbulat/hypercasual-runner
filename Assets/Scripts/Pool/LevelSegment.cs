using System.Collections.Generic;
using UnityEngine;

namespace Pool
{
    [CreateAssetMenu]
    public class LevelSegment : ScriptableObject
    {
        public List<Vector3> X3Platforms;
        public List<Vector3> X5Platforms;
        public List<Vector3> X7Platforms;
    }
}