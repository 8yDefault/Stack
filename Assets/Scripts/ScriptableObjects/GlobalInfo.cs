using UnityEngine;

namespace StackGame
{
    public class GlobalInfo : ScriptableObject
    {
        [Header("Balance")]
        public float TileSpeed = 1.0f;
        public float TileSize = 1.0f;
        public float DistanceMultiplier = 1.0f;
        public float ErrorThreshold = 0.1f;
        public Vector2 MaxStackBounds = Vector2.one;
        public float BoundsIncrementBonus = 0.1f;
        public int BonusTriggerCount = 5;
    }
}