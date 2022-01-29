using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pool
{
    [CreateAssetMenu]
    public class LevelSegment : ScriptableObject
    {
        public ISegment Collectables;
        public ISegment Platforms;

        public int Length = 30;
    }
}