using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assembly.IBX.Main.UtilityComponents
{
    internal class LoadNextSceneImediate : MonoBehaviour
    {
        private void Start()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
