using UnityEngine;

namespace StackGame
{
    public class GameController : MonoBehaviour
    {
        private void Awake()
        {
            Debug.Log($"{this.ToString()} : Awake");

            NavigationController.InitInstance();
            ScoreController.InitInstance();

            // TODO: create UpdateController, InputController
        }

        private void Start()
        {
            Debug.Log($"{this.ToString()} : Start");

            EventAggregator.LevelStarted?.Invoke();
        }

        private void OnDestroy()
        {
            Debug.Log($"{this.ToString()} : OnDestroy");

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
    }
}