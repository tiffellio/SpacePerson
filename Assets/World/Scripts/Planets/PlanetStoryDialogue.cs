using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace csci485.World.Planets
{
    [CreateAssetMenu(fileName = "planetBehavior", menuName = "Planets/Behavior/Create new Story Dialouge", order = 1)]
    public class PlanetStoryDialogue : ScriptableObject
    {
        public string planetName;

        [TextArea]
        public string [] dialogue;

        [TextArea]
        public string defaultIntro;
    }

}

