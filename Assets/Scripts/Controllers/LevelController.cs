using UnityEngine;

namespace StackGame
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private GameObject _stackPrefab = null;
        [SerializeField] private GlobalInfo _config = null;
        [SerializeField] public APool _pool = null;

        [SerializeField] public Screen _HUDScreen = null;
        [SerializeField] public Screen _gameOverScreen = null;

        private void Awake()
        {
            Debug.Log($"{this.ToString()} : Awake");

            EventAggregator.LevelStarted+= OnLevelStarted;
            EventAggregator.InaccurateStep += OnInaccurateStep;
            EventAggregator.StepPerformed += OnStepPerformed;
        }

        private void OnDestroy()
        {
            Debug.Log($"{this.ToString()} : OnDestroy");

            EventAggregator.StepPerformed -= OnStepPerformed;
            EventAggregator.InaccurateStep -= OnInaccurateStep;
            EventAggregator.LevelStarted -= OnLevelStarted;
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

        private void OnLevelStarted()
        {
            CreateStack();
        }

        private void OnInaccurateStep(Vector3 position, Vector3 scale)
        {
            _pool.GetObject(position, scale);
        }

        private void OnStepPerformed(bool success)
        {
            _gameOverScreen.gameObject.SetActive(!success);
        }
    }
}