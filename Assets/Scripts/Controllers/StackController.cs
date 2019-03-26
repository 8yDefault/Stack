using System.Collections.Generic;
using UnityEngine;

namespace StackGame
{
    public class StackController : MonoBehaviour
    {
        [SerializeField] private GameObject _tilePrefab = null;
        [SerializeField] private int _tilesMaxAmount = 64;

        private StackModel _model;
        private List<GameObject> _stackTiles = null;
        private Vector2 _stackBounds = Vector2.zero;

        private float _tileTransition = 1.0f;
        private int _stepIndex = 0;
        private int _stackIndex = 0;
        private int _comboIndex = 0;

        private bool _isAlongWithAxisX = false;
        private float _placedTilePosition = 0.0f;
        private Vector3 _lastTilePosition = Vector3.zero;

        private bool _isEnabled = false;

        public void Init(StackModel model)
        {
            _model = model;

            // TODO: create external pool or create new tile on demand?
            _stackTiles = new List<GameObject>(_tilesMaxAmount);
            for (int i = 0; i < _tilesMaxAmount; i++)
            {
                var go = Instantiate(_tilePrefab);
                go.transform.SetParent(this.transform);
                go.transform.localPosition = Vector3.down * i;
                go.transform.localScale = new Vector3(_model.Size, 1, _model.Size);
                _stackTiles.Add(go);
            }

            _stepIndex++;
            _stackIndex = _tilesMaxAmount - 1;
            _stackBounds = new Vector2(_model.Size, _model.Size);
            ResetTilePosition();

            _isEnabled = true;
        }

        private void Update()
        {
            if (!_isEnabled)
            {
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (PlaceTile())
                {
                    SpawnTile();
                }
                else
                {
                    EventAggregator.StepPerformed?.Invoke(false);

                    _isEnabled = false;
                    _stackTiles[_stackIndex].AddComponent<Rigidbody>();
                }
            }

            MoveTile();
        }

        private void MoveTile()
        {
            _tileTransition += Time.deltaTime * _model.Speed;
            var axisPos = Mathf.Sin(_tileTransition) * _model.Size * _model.DistanceMultiplier;
            var nextPos = _isAlongWithAxisX ? new Vector3(axisPos, _stepIndex, _placedTilePosition) : new Vector3(_placedTilePosition, _stepIndex, axisPos);

            _stackTiles[_stackIndex].transform.localPosition = nextPos;
        }

        private void SpawnTile()
        {
            ResetTilePosition();
            _lastTilePosition = _stackTiles[_stackIndex].transform.localPosition;

            _stackIndex--;
            if (_stackIndex < 0)
            {
                // TODO: create new tile or reuse existing ones?
                _stackIndex = _tilesMaxAmount - 1;
            }

            _stackTiles[_stackIndex].transform.localPosition = new Vector3(0, _stepIndex, 0);
            _stackTiles[_stackIndex].transform.localScale = new Vector3(_stackBounds.x, 1, _stackBounds.y);
            _stepIndex++;
        }

        private bool PlaceTile()
        {
            Transform currentTile = _stackTiles[_stackIndex].transform;

            if (_isAlongWithAxisX)
            {
                float delta = Mathf.Abs(_lastTilePosition.x - currentTile.localPosition.x);
                if (delta > _model.ErrorThreshold)
                {
                    _comboIndex = 0;
                    _stackBounds.x -= delta;
                    if (_stackBounds.x <= 0)
                    {
                        return false;
                    }

                    float middlePos = (_lastTilePosition.x + currentTile.localPosition.x) / 2;
                    currentTile.localScale = new Vector3(_stackBounds.x, 1, _stackBounds.y);

                    var next = currentTile.localPosition.x > _lastTilePosition.x ? currentTile.localPosition.x + currentTile.localScale.x / 2 : currentTile.localPosition.x - currentTile.localScale.x / 2;
                    EventAggregator.InaccurateStep?.Invoke(new Vector3(next, currentTile.localPosition.y, currentTile.localPosition.z), new Vector3(delta, 1, _stackBounds.y));

                    currentTile.localPosition = new Vector3(middlePos, _stepIndex, _lastTilePosition.z);
                }
                else
                {
                    if (_comboIndex > _model.BonusTriggerCount)
                    {
                        _stackBounds.x += _model.BoundsIncrementBonus;
                        if (_stackBounds.x > _model.MaxStackBounds.x)
                        {
                            _stackBounds.x = _model.MaxStackBounds.x;
                        }
                    }
                    _comboIndex++;
                    currentTile.localPosition = new Vector3(_lastTilePosition.x , _stepIndex, _lastTilePosition.z);
                }
            }
            else
            {
                float delta = Mathf.Abs(_lastTilePosition.z - currentTile.localPosition.z);
                if (delta > _model.ErrorThreshold)
                {
                    _comboIndex = 0;
                    _stackBounds.y -= delta;
                    if (_stackBounds.y <= 0)
                    {
                        return false;
                    }

                    float middlePos = (_lastTilePosition.z + currentTile.localPosition.z) / 2;
                    currentTile.localScale = new Vector3(_stackBounds.x, 1, _stackBounds.y);

                    var next = currentTile.localPosition.z > _lastTilePosition.z ? currentTile.localPosition.z + currentTile.localScale.z / 2 : currentTile.localPosition.z - currentTile.localScale.z / 2;
                    EventAggregator.InaccurateStep?.Invoke(new Vector3(currentTile.localPosition.x, currentTile.localPosition.y, next), new Vector3(_stackBounds.x, 1, delta));

                    currentTile.localPosition = new Vector3(_lastTilePosition.x, _stepIndex, middlePos);
                }
                else
                {
                    if (_comboIndex > _model.BonusTriggerCount)
                    {
                        _stackBounds.y += _model.BoundsIncrementBonus;
                        if (_stackBounds.y > _model.MaxStackBounds.y)
                        {
                            _stackBounds.y = _model.MaxStackBounds.y;
                        }
                    }
                    _comboIndex++;
                    currentTile.localPosition = new Vector3(_lastTilePosition.x, _stepIndex, _lastTilePosition.z);
                }
            }

            _placedTilePosition = _isAlongWithAxisX ? currentTile.localPosition.x : currentTile.localPosition.z;
            _isAlongWithAxisX = !_isAlongWithAxisX;

            EventAggregator.StepPerformed?.Invoke(true);

            return true;
        }

        private void ResetTilePosition()
        {
            _tileTransition = Mathf.PI / 2;
        }
    }
}