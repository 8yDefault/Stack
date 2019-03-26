using UnityEngine.SceneManagement;

namespace StackGame
{
    public class NavigationController : Singleton<NavigationController>
    {
        protected override void Init()
        {
            base.Init();
        }

        protected override void DeInit()
        {
            base.DeInit();
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}