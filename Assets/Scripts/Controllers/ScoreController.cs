using System;
using UnityEngine;

namespace Stack
{
    public class ScoreController : Singleton<ScoreController>
    {
        public Action<int> ScroreUpdated = null;

        private int _score = 0;

        protected override void Init()
        {
            base.Init();

            StackController.Instance.StepPerformed += OnStepPerformed;
        }

        protected override void DeInit()
        {
            StackController.Instance.StepPerformed -= OnStepPerformed;

            base.DeInit();
        }

        private void OnStepPerformed(bool success)
        {
            if (success)
            {
                _score++;
            }

            ScroreUpdated?.Invoke(_score);
        }
    }
}