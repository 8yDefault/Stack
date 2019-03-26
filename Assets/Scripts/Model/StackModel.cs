using UnityEngine;

namespace StackGame
{
    public struct StackModel
    {
        public float Speed { get; private set; }
        public float Size { get; private set; }
        public float DistanceMultiplier { get; private set; }
        public float ErrorThreshold { get; private set; }
        public Vector2 MaxStackBounds { get; private set; }
        public float BoundsIncrementBonus { get; private set; }
        public int BonusTriggerCount { get; private set; }

        public StackModel(float speed, float size, float distanceMultiplier, float errorThreshold, Vector2 maxStackBounds, float incrementBonus, int bonusTriggerCount)
        {
            Speed = speed;
            Size = size;
            DistanceMultiplier = distanceMultiplier;
            ErrorThreshold = errorThreshold;
            MaxStackBounds = maxStackBounds;
            BoundsIncrementBonus = incrementBonus;
            BonusTriggerCount = bonusTriggerCount;
        }
    }
}