using UnityEngine;

namespace StackGame
{
    public class CameraController : MonoBehaviour
    {
        [Header("View")]
        [SerializeField] private float _movingSpeed = 1.0f;

        private float _height = 0;

        private void OnEnable()
        {
            EventAggregator.StepPerformed += OnStepPerformed;

            _height = transform.position.y;
        }

        private void OnDisable()
        {
            EventAggregator.StepPerformed -= OnStepPerformed;
        }

        private void OnStepPerformed(bool success)
        {
            if (success)
            {
                _height++;
            }
        }

        private void Update()
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, _height, transform.position.z), _movingSpeed);
        }
    }
}