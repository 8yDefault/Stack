using UnityEngine;
using UnityEngine.UI;

namespace StackGame
{
    public class GameOverScreen : Screen
    {
        [Header("Navigation")]
        [SerializeField] private Button _restartButton = null;

        private void OnEnable()
        {
            _restartButton.onClick.AddListener(() => NavigationController.Instance.RestartLevel());
        }

        private void OnDisable()
        {
            _restartButton.onClick.RemoveAllListeners();
        }
    }
}