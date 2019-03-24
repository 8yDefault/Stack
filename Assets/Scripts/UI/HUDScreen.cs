using UnityEngine;
using UnityEngine.UI;

namespace Stack
{
    public class HUDScreen : MonoBehaviour
    {
        [Header("View")]
        [SerializeField] private Text _score = null;

        private void OnEnable()
        {
            // TODO: remove logic from View component
            UpdateScore(0);

            ScoreController.Instance.ScroreUpdated += OnScoreUpdated;
        }

        private void OnDestroy()
        {
            ScoreController.Instance.ScroreUpdated -= OnScoreUpdated;
        }

        private void OnScoreUpdated(int value)
        {
            UpdateScore(value);
        }

        private void UpdateScore(int value)
        {
            _score.text = value.ToString();
        }
    }
}