using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace csci485.Character
{
    //Handles playing sound effects attached to the Player
    public class PlayerSoundHandler : MonoBehaviour
    {

        private AudioSource[] mysounds;

        private AudioSource engineSound;
        private AudioSource laserShot;

        // Start is called before the first frame update
        void Start()
        {
            mysounds = GetComponents<AudioSource>();
            engineSound = mysounds[0];
            laserShot = mysounds[1];
        }

        
        public void PlayEngineSound()
        {
            UnityEngine.Debug.Log("playing engine sound");
            engineSound.Play();
        }

        public void PlayLaserShot()
        {
            laserShot.Play();
        }
    
        public void StopAll()
        {
            laserShot.Stop();
            engineSound.Stop();
        }
    }
}