using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace csci485.World.Planets
{
    [CreateAssetMenu(fileName = "planetBehavior", menuName = "Planets/Behavior/Create new Planet Dialouge Set", order = 1)]
    public class PlanetDialogueSet : ScriptableObject
    {
        public string planetName = "Bob";
        
        //[TextArea]
        //public string planet_intro;

        [TextArea]
        public string [] dialogue_firstVisit;

        [TextArea]
        public string[] dialogue_repeatVisit;

        //[TextArea]
        //public string talk_normal;
        //[TextArea]
        //public string talk_thriving;
        //[TextArea]
        //public string talk_dying;

    }
}
