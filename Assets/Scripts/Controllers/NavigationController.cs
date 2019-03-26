using UnityEngine.SceneManagement;

namespace StackGame
{
    public class NavigationController : Singleton<NavigationController>
    {
        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}