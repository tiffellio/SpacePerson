using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace csci485.Management
{

    public class Timer : MonoBehaviour
    {

        [SerializeField] private float timeRemaining;
        private bool timerExpired;

        TMP_Text Text;

        private LevelManager levelManager; 

        private void Start()
        {
            timerExpired = false;
            Text = GetComponent<TMP_Text>();
        }

        void Update()
        {
            if (!timerExpired)
            {


                if (timeRemaining > 0)
                {
                    timeRemaining -= Time.deltaTime;
                    DisplayTime(timeRemaining);
                   
                }
                else
                {
                    timeRemaining = 0;
                    timerExpired = true;
                    GameOver();
                    //GameOver();
                    //What happens??
                    //DisplayTime(timeRemaining);
                }
            }
        }

        public void DisplayTime(float time)
        {
            time += 1;
            //mins for days?
            float days = Mathf.FloorToInt(time/60);
            //24 hours in a day (minute);
            float hours = Mathf.FloorToInt((time/2.5f)% 24);
            
            Text.text = string.Format("Time Remaining: {0:00} Days {1:00} Hours", days, hours );

        }


        public void GameOver()
        {
            levelManager = FindObjectOfType<LevelManager>();
            levelManager.LoadLevel("0c_GameOver");

        }
    }
}