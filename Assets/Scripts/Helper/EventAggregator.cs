using System;
using UnityEngine;

namespace StackGame
{
    internal class EventAggregator
    {
        public static Action LevelStarted = null;
        public static Action<bool> StepPerformed = null;
        public static Action<Vector3, Vector3> InaccurateStep = null;
        public static Action<int> ScroreUpdated = null;
    }
}