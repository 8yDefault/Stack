using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackGame
{
    public class LevelController : MonoBehaviour
    {
        public APool _pool = null;

        private void Start()
        {
            EventAggregator.InaccurateStep += OnInaccurateStep;
        }

        private void OnDestroy()
        {
            EventAggregator.InaccurateStep -= OnInaccurateStep;
            _pool = null;
        }

        private void OnInaccurateStep(Vector3 position, Vector3 scale)
        {
            CreateCube(position, scale);
        }

        public void CreateCube(Vector3 position, Vector3 scale)
        {
            _pool.GetObject(position, scale);
        }
    }
}