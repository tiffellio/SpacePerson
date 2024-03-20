using UnityEngine;
using UnityEngine.SceneManagement;

namespace csci485.Management
{
    public class LevelManager : MonoBehaviour
    {
        public void LoadLevel(string name)
        {
            Debug.Log("level load requested for: " + name);
            SceneManager.LoadScene(name);
        }

        public void QuitRequest()
        {
            Debug.Log("quit request");
            Application.Quit();
        }

    }
}


