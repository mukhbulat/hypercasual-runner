using Pool.Background_Items;
using Pool.Barriers;
using Pool.Collectables;
using Pool.Platforms;
using UnityEngine;

namespace Pool
{
    [CreateAssetMenu]
    public class LevelSegment : ScriptableObject
    {
        public CollectablesSegment Collectables;
        public PlatformsSegment Platforms;
        public EnvironmentsSegment Environments;
        public BarrierSegment Barriers;
        public int Length = 30;

        /*
        public GameObject Floor;
        public Vector3 FloorPosition;
        public Vector3 FloorRotation;
        */
    }
}