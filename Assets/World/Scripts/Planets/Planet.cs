using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace csci485.World.Planets
{
    public enum PlanetStates {dead, dying, normal, thriving}
  
    public abstract class Planet : MonoBehaviour
    {
        
        public PlanetStates currentPlanetState = PlanetStates.normal;

        public abstract void Interact();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag != "Player") return;

            Interact();
        }


    }
}


