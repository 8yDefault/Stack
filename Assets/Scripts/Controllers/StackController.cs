using System;
using UnityEngine;

namespace StackGame
{
    public class StackController : MonoBehaviour
    {
        private StackModel _model;
        private GameObject[] _stackTiles = null;
        private Vector2 _stackBounds = Vector2.zero;
        private int _stackAmount = 0;

        private float _tileTransition = 1.0f;
        private int _stepIndex = 0;
        private int _stackIndex = 0;
        private int _comboIndex = 0;

        private bool _isAlongWithAxisX = false;
        private float _placedTilePosition = 0.0f;
        private Vector3 _lastTilePosition = Vector3.zero;

        private bool _isInited = false;
        private bool _isGameOver = false;

        public void Init(StackModel model)
        {
            // TODO: move to level controller
            EventAggregator.LevelStarted?.Invoke();

            _model = model;
            _stackAmount = transform.childCount;

            _stackTiles = new GameObject[_stackAmount];
            for (int i = 0; i < _stackAmount; i++)
            {
                _stackTiles[i] = transform.GetChild(i).gameObject;
                transform.GetChild(i).localScale = new Vector3(_model.Size, 1, _model.Size);
            }
            _stackIndex = _stackAmount - 1;
            _stackBounds = new Vector2(_model.Size, _model.Size);
            ReserTilePosition();

            _isInited = true;
        }

        private void Update()
        {
            if (!_isInited)
            {
                return;
            }

            if (_isGameOver)
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
                    GameOver();
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
            ReserTilePosition();
            _lastTilePosition = _stackTiles[_stackIndex].transform.localPosition;

            _stackIndex--;
            if (_stackIndex < 0)
            {
                _stackIndex = _stackAmount - 1;
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
                float deltaX = _lastTilePosition.x - currentTile.localPosition.x;
                if (Mathf.Abs(deltaX) > _model.ErrorThreshold)
                {
                    var cacheBound = _stackBounds.x;
                    _comboIndex = 0;
                    _stackBounds.x -= Mathf.Abs(deltaX);
                    if (_stackBounds.x <= 0)
                    {
                        return false;
                    }

                    float middlePos = (_lastTilePosition.x + currentTile.localPosition.x) / 2;
                    currentTile.localScale = new Vector3(_stackBounds.x, 1, _stackBounds.y);

                    var nextX = currentTile.localPosition.x > _lastTilePosition.x ? currentTile.localPosition.x + currentTile.localScale.x / 2 : currentTile.localPosition.x - currentTile.localScale.x / 2;
                    EventAggregator.InaccurateStep?.Invoke(new Vector3(nextX, currentTile.localPosition.y, currentTile.localPosition.z), new Vector3(deltaX, 1, _stackBounds.y));

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
                float deltaY = _lastTilePosition.z - currentTile.localPosition.z;
                if (Mathf.Abs(deltaY) > _model.ErrorThreshold)
                {
                    var cacheBound = _stackBounds.y;

                    _comboIndex = 0;
                    _stackBounds.y -= Mathf.Abs(deltaY);
                    if (_stackBounds.y <= 0)
                    {
                        return false;
                    }

                    float middlePos = (_lastTilePosition.z + currentTile.localPosition.z) / 2;
                    currentTile.localScale = new Vector3(_stackBounds.x, 1, _stackBounds.y);

                    var nextZ = currentTile.localPosition.z > _lastTilePosition.z ? currentTile.localPosition.z + currentTile.localScale.z / 2 : currentTile.localPosition.z - currentTile.localScale.z / 2;
                    EventAggregator.InaccurateStep?.Invoke(new Vector3(currentTile.localPosition.x, currentTile.localPosition.y, nextZ), new Vector3(_stackBounds.x, 1, deltaY));

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

        private void ReserTilePosition()
        {
            _tileTransition = Mathf.PI / 2;
        }

        private void GameOver()
        {
            EventAggregator.StepPerformed?.Invoke(false);

            _isGameOver = true;
            _stackTiles[_stackIndex].AddComponent<Rigidbody>();
        }
    }
}