using UnityEngine;

namespace Stack
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject _gameOverScreen = null;
        [SerializeField] private GlobalInfo _config = null;

        public static GlobalInfo Config = null;

        private void Awake()
        {
            Config = _config;

            ScoreController.InitInstance();
            StackController.Instance.Init();

            StackController.Instance.StepPerformed += OnStepPerformed;
        }

        private void OnDestroy()
        {
            StackController.Instance.StepPerformed -= OnStepPerformed;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                NavigationController.Instance.RestartLevel();
            }
        }

        private void OnStepPerformed(bool success)
        {
            _gameOverScreen.SetActive(!success);
        }
    }
}