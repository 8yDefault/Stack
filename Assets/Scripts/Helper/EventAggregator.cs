using System;
using UnityEngine;

namespace StackGame
{
    public class EventAggregator
    {
        public static Action LevelStarted = null;
        public static Action<bool> StepPerformed = null;
        public static Action<Vector3, Vector3> InaccurateStep = null;
    }
}