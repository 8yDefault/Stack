﻿using System;
using UnityEngine;

namespace Stack
{
    public class StackController : MonoBehaviour
    {
        public Action<bool> StepPerformed = null;

        [Header("Balance")]
        [SerializeField] private float _tileTransition = 1.0f;
        [SerializeField] private float _tileSpeed = 1.0f;
        [SerializeField] private float _distanceMultiplier = 1.0f;
        [SerializeField] private float _errorThreshold = 0.1f;
        [SerializeField] private Vector2 _maxStackBounds = Vector2.one;
        [SerializeField] private float _boundsIncrementBonus = 0.1f;
        [SerializeField] private int _bonusTriggerCount = 5;

        [Header("View")]
        [SerializeField] private float _tileSize = 1.0f;
        [SerializeField] private float _stackMovingSpeed = 1.0f;

        private GameObject[] _stackTiles = null;
        private Vector2 _stackBounds = Vector2.zero;
        private int _stackAmount = 0;

        private int _stepIndex = 0;
        private int _stackIndex = 0;
        private int _comboIndex = 0;

        private bool _isAlongWithAxisX = false;
        private float _placedTilePosition = 0.0f;
        private Vector3 _lastTilePosition = Vector3.zero;

        private bool _isGameOver = false;

        private void Start()
        {
            _stackAmount = transform.childCount;

            _stackTiles = new GameObject[_stackAmount];
            for (int i = 0; i < _stackAmount; i++)
            {
                _stackTiles[i] = transform.GetChild(i).gameObject;
                transform.GetChild(i).localScale = new Vector3(_tileSize, 1, _tileSize);
            }
            _stackIndex = _stackAmount - 1;
            _stackBounds = new Vector2(_tileSize, _tileSize);
            ReserTilePosition();
        }

        private void Update()
        {
            if(Input.GetMouseButtonDown(0))
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
            //LowerStack();
        }

        private void MoveTile()
        {
            if (_isGameOver)
            {
                return;
            }

            _tileTransition += Time.deltaTime * _tileSpeed;
            var axisPos = Mathf.Sin(_tileTransition) * _tileSize * _distanceMultiplier;
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
            Debug.Log("# PlaceTile : " + currentTile.name + " ::: currentTile.pos = " + currentTile.position);

            if (_isAlongWithAxisX)
            {
                float deltaX = _lastTilePosition.x - currentTile.localPosition.x;
                if (Mathf.Abs(deltaX) > _errorThreshold)
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

                    // TODO: возможно, здесь бага currentTile.localPosition.x > 0, провести тесты. Повторить в axis Z
                    //var nextX = currentTile.localPosition.x > 0 ? currentTile.localPosition.x + currentTile.localScale.x / 2 : currentTile.localPosition.x - currentTile.localScale.x / 2;
                    var nextX = currentTile.localPosition.x > _lastTilePosition.x ? currentTile.localPosition.x + currentTile.localScale.x / 2 : currentTile.localPosition.x - currentTile.localScale.x / 2;
                    CreateCube(new Vector3(nextX, currentTile.localPosition.y, currentTile.localPosition.z), new Vector3(deltaX, 1, _stackBounds.y));

                    currentTile.localPosition = new Vector3(middlePos, _stepIndex, _lastTilePosition.z);
                    Debug.Log("# currentTile.localPosition X : " + (middlePos - (_lastTilePosition.x / 2)));

                }
                else
                {
                    if (_comboIndex > _bonusTriggerCount)
                    {
                        _stackBounds.x += _boundsIncrementBonus;
                        if (_stackBounds.x > _maxStackBounds.x)
                        {
                            _stackBounds.x = _maxStackBounds.x;
                        }
                    }
                    _comboIndex++;
                    currentTile.localPosition = new Vector3(_lastTilePosition.x , _stepIndex, _lastTilePosition.z);
                }
            }
            else
            {
                float deltaY = _lastTilePosition.z - currentTile.localPosition.z;
                if (Mathf.Abs(deltaY) > _errorThreshold)
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

                    //var nextZ = currentTile.localPosition.z > 0 ? currentTile.localPosition.z + currentTile.localScale.z / 2 : currentTile.localPosition.z - currentTile.localScale.z / 2;
                    var nextZ = currentTile.localPosition.z > _lastTilePosition.z ? currentTile.localPosition.z + currentTile.localScale.z / 2 : currentTile.localPosition.z - currentTile.localScale.z / 2;
                    CreateCube(new Vector3(currentTile.localPosition.x, currentTile.localPosition.y, nextZ), new Vector3(_stackBounds.x, 1, deltaY));

                    currentTile.localPosition = new Vector3(_lastTilePosition.x, _stepIndex, middlePos);
                    Debug.Log("# currentTile.localPosition Z : " + (middlePos - (_lastTilePosition.z / 2)));

                }
                else
                {
                    if (_comboIndex > _bonusTriggerCount)
                    {
                        _stackBounds.y += _boundsIncrementBonus;
                        if (_stackBounds.y > _maxStackBounds.y)
                        {
                            _stackBounds.y = _maxStackBounds.y;
                        }
                    }

                    _comboIndex++;
                    currentTile.localPosition = new Vector3(_lastTilePosition.x, _stepIndex, _lastTilePosition.z);
                }
            }

            _placedTilePosition = _isAlongWithAxisX ? currentTile.localPosition.x : currentTile.localPosition.z;
            _isAlongWithAxisX = !_isAlongWithAxisX;

            if (StepPerformed != null)
            {
                StepPerformed.Invoke(true);
            }

            return true;
        }

        private void LowerStack()
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(0, -_stepIndex, 0), _stackMovingSpeed);
        }

        private void ReserTilePosition()
        {
            _tileTransition = Mathf.PI / 2;
        }

        public void CreateCube(Vector3 position, Vector3 scale)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);

            go.transform.position = transform.TransformPoint(position);
            go.transform.localScale = scale;
            go.AddComponent<Rigidbody>();
        }

        private void GameOver()
        {
            Debug.LogWarning(this + " : GameOver");

            _isGameOver = true;
            _stackTiles[_stackIndex].AddComponent<Rigidbody>();
        }
    }
}