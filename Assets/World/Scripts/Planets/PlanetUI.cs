using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace csci485.World.Planets
{
    public class PlanetUI : MonoBehaviour
    {
        [SerializeField] TMP_Text dialogueField = null;
        [SerializeField] TMP_Text planetName = null;

        private void Start()
        {
            PlanetData.planetUI =  this;
        }

        public void changeText(string text)
        {
            dialogueField.text = text;
        }

        public void changeName(string name)
        {
            planetName.text = name;
        }
    }
}

