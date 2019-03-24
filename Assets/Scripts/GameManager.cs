using UnityEngine;

namespace Stack
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject _gameOverScreen = null;

        private void Awake()
        {
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