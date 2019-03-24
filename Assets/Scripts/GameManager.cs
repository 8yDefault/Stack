using UnityEngine;
using UnityEngine.SceneManagement;

namespace Stack
{
    public class GameManager : MonoBehaviour
    {
        private void Awake()
        {
            ScoreController.InitInstance();
            StackController.Instance.Init();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}