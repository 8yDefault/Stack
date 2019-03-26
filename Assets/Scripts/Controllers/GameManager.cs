using UnityEngine;

namespace StackGame
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject _gameOverScreen = null;
        [SerializeField] private GlobalInfo _config = null;
        [SerializeField] private GameObject _stackPrefab = null;

        private void Awake()
        {
            CreateStack();

            NavigationController.InitInstance();
            ScoreController.InitInstance();

            EventAggregator.StepPerformed += OnStepPerformed;
        }

        private void OnDestroy()
        {
            EventAggregator.StepPerformed -= OnStepPerformed;

            ScoreController.RemoveInstance();
            NavigationController.RemoveInstance();
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
                _config.BoundsIncrementBonus, _config.BonusTriggerCount);

            var go = Instantiate(_stackPrefab);
            go.transform.position = Vector3.zero;
            var ctrl = go.GetComponent<StackController>();
            ctrl.Init(stackModel);
        }
    }
}