using UnityEngine;

namespace Stack
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject _gameOverScreen = null;
        [SerializeField] private GlobalInfo _config = null;

        public static GlobalInfo Config { get; private set; }

        private void Awake()
        {
            Config = _config;

            CreateStack();

            ScoreController.InitInstance();

            //StackController.Instance.StepPerformed += OnStepPerformed;
            EventAggregator.StepPerformed += OnStepPerformed;
        }

        private void OnDestroy()
        {
            //StackController.Instance.StepPerformed -= OnStepPerformed;
            EventAggregator.StepPerformed -= OnStepPerformed;
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

        private void CreateStack()
        {
            var stackModel = new StackModel(_config.TileSpeed, _config.TileSize, _config.DistanceMultiplier, _config.ErrorThreshold, _config.MaxStackBounds,
                _config.BoundsIncrementBonus, _config.BonusTriggerCount, _config.Colors, _config.StackMaterial);

            StackController.Instance.Init(stackModel);
        }
    }
}