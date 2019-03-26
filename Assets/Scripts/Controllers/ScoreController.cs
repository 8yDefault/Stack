using System;

namespace StackGame
{
    public class ScoreController : Singleton<ScoreController>
    {
        public Action<int> ScroreUpdated = null;

        private int _score = 0;

        protected override void Init()
        {
            base.Init();

            EventAggregator.LevelStarted += OnLevelStarted;
            EventAggregator.StepPerformed += OnStepPerformed;
        }

        protected override void DeInit()
        {
            EventAggregator.LevelStarted -= OnLevelStarted;
            EventAggregator.StepPerformed -= OnStepPerformed;


            base.DeInit();
        }

        private void OnLevelStarted()
        {
            _score = 0;

            ScroreUpdated?.Invoke(_score);
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