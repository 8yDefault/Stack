using UnityEngine;
using UnityEngine.UI;

namespace StackGame
{
    public class HUDScreen : Screen
    {
        [Header("View")]
        [SerializeField] private Text _score = null;

        private void OnEnable()
        {
            EventAggregator.ScroreUpdated += OnScoreUpdated;
        }

        private void OnDestroy()
        {
            EventAggregator.ScroreUpdated -= OnScoreUpdated;
        }

        private void OnScoreUpdated(int value)
        {
            _score.text = value.ToString();
        }
    }
}