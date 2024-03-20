using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace csci485.World.Planets
{
    public class PlanetUIList : MonoBehaviour
    {
        public Text [] planetList;
        public PlanetDialogueSet [] Planets;

        public void UpdateUIList(string name)
        {
            int index = GetPlanetIndex(name);
            if(index == -1)
            {
                Debug.Log("Planet not found in list");
                return;
            }
            if(planetList[index+1].text != Planets[index].planetName) planetList[index+1].text = name;
        }

        private int GetPlanetIndex(string name)
        {
            int i = 0;
            foreach(PlanetDialogueSet planet in Planets)
            {
                if(name == planet.planetName) return i;
                i++;
            }
            return -1;
        }
    }
}


