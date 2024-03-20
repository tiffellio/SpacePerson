using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace csci485.Management
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] string startLevel = "";
        private LevelManager levelManager;

        // Start is called before the first frame update
        void Start()
        {
            levelManager = FindObjectOfType<LevelManager>();
        }


        public void GoToStart()
        {
            Time.timeScale = 1.0f;
            levelManager.LoadLevel(startLevel);
        }

        public void Resume()
        {
            Time.timeScale = 1.0f;
            gameObject.SetActive(false);
        }

        public void Pause()
        {
            Time.timeScale = 0f;
            gameObject.SetActive(true);
        }
    }

}

