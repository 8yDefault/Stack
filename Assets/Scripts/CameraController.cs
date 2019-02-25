using UnityEngine;

namespace Stack
{
    public class CameraController : MonoBehaviour
    {
        [Header("Referencies")]
        [SerializeField] private StackController _stackController = null;

        [Header("View")]
        [SerializeField] private float _movingSpeed = 1.0f;

        private float _height = 0;

        private void Awake()
        {
            _stackController.StepPerformed += OnStepPerformed;

            _height = transform.position.y;
        }

        private void OnDestroy()
        {
            _stackController.StepPerformed -= OnStepPerformed;
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