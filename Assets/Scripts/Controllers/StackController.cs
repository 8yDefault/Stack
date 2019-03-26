using System;
using UnityEngine;

namespace Stack
{
    public class StackController : MonoBehaviour
    {
        public Action<bool> StepPerformed = null;

        // TEMP
        private static StackController _instance = null;
        public static StackController Instance
        {
            get
            {
                if (_instance == null)
                {
                    InitInstance();
                }
                return _instance;
            }

            set
            {
                if (value == null)
                {
                    if (_instance != null)
                    {
                        Debug.Log($"StackController : DeInit.");
                        _instance = null;
                    }
                }
                else
                {
                    throw new InvalidOperationException("Create your singleton in other way.");
                }
            }
        }

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

        private static void InitInstance()
        {
            if (_instance == null)
            {
                _instance = (StackController)FindObjectOfType(typeof(StackController));
                if (_instance == null)
                {
                    var newSingleton = new GameObject();
                    _instance = newSingleton.AddComponent<StackController>();
                }
            }
        }

        public void Init(StackModel model)
        {
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

            ColorMesh(_stackTiles[_stackIndex].GetComponent<MeshFilter>().mesh);
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

                    // TODO: возможно, здесь бага currentTile.localPosition.x > 0, провести тесты. Повторить в axis Z
                    //var nextX = currentTile.localPosition.x > 0 ? currentTile.localPosition.x + currentTile.localScale.x / 2 : currentTile.localPosition.x - currentTile.localScale.x / 2;
                    var nextX = currentTile.localPosition.x > _lastTilePosition.x ? currentTile.localPosition.x + currentTile.localScale.x / 2 : currentTile.localPosition.x - currentTile.localScale.x / 2;
                    CreateCube(new Vector3(nextX, currentTile.localPosition.y, currentTile.localPosition.z), new Vector3(deltaX, 1, _stackBounds.y));

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

                    //var nextZ = currentTile.localPosition.z > 0 ? currentTile.localPosition.z + currentTile.localScale.z / 2 : currentTile.localPosition.z - currentTile.localScale.z / 2;
                    var nextZ = currentTile.localPosition.z > _lastTilePosition.z ? currentTile.localPosition.z + currentTile.localScale.z / 2 : currentTile.localPosition.z - currentTile.localScale.z / 2;
                    CreateCube(new Vector3(currentTile.localPosition.x, currentTile.localPosition.y, nextZ), new Vector3(_stackBounds.x, 1, deltaY));

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

            //StepPerformed?.Invoke(true);
            EventAggregator.StepPerformed?.Invoke(true);

            return true;
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

            go.GetComponent<MeshRenderer>().material = _model.Material;
            ColorMesh(go.GetComponent<MeshFilter>().mesh);
        }

        private void ColorMesh(Mesh mesh)
        {
            var vertices = mesh.vertices;
            var colors = new Color32[vertices.Length];

            float f = Mathf.Sin(_stepIndex) * 0.25f;
            for (int i = 0; i < vertices.Length; i++)
            {
                colors[i] = Lerp4(_model.Colors[0], _model.Colors[1], _model.Colors[2], _model.Colors[3], f);
            }
            mesh.colors32 = colors;
        }

        private Color32 Lerp4(Color32 a, Color32 b, Color32 c, Color32 d, float time)
        {
            if (time < 0.33f)
            {
                return Color.Lerp(a, b, time / 0.33f);
            }
            else if(time < 0.66f)
            {
                return Color.Lerp(b, c, (time - 0.33f) / 0.33f);
            }
            else
            {
                return Color.Lerp(c, d, (time - 0.66f) / 0.66f);
            }
        }

        private void GameOver()
        {
            Debug.Log(this + " : GameOver");

            //StepPerformed?.Invoke(false);
            EventAggregator.StepPerformed?.Invoke(false);

            _isGameOver = true;
            _stackTiles[_stackIndex].AddComponent<Rigidbody>();
        }
    }
}