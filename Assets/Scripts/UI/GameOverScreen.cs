using UnityEngine;
using UnityEngine.UI;

namespace Stack
{
    public class GameOverScreen : MonoBehaviour
    {
        [Header("View")]
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