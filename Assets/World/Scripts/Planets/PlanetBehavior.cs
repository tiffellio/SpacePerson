using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace csci485.World.Planets
{
    [CreateAssetMenu(fileName = "planetBehavior", menuName = "Planets/Behavior/Create new Planet", order = 1)]
    public class PlanetBehavior : ScriptableObject
    {
        public PlanetDialogueSet dialogueSet;
        // need from player
        public ResourseTypes requiredResourse = ResourseTypes.crystals;
        public int requiredAmount = 10;
        // offered from planet
        public ResourseTypes offeredResourse = ResourseTypes.food;
        public int offeredAmount = 10;
    }

}

