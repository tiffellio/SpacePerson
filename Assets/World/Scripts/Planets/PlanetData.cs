using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace csci485.World.Planets
{
    public static class PlanetData 
    {
        public static Dictionary<string,planet> planets = new Dictionary<string, planet>();

        public struct planet
        {
            private string id;
            public PlanetStates currentState;
            public float lastSuccessfulActionTime;
        }

        public static PlanetUI planetUI;
        public static PlanetDialogueCanvas planetDialogueCanvas;


        public static Dictionary<Vector2Int, CrytalClump> CrystalClumps = new Dictionary<Vector2Int, CrytalClump>();

        public struct CrytalClump
        {
            public float lastPickupTime;
        }
    }
}


