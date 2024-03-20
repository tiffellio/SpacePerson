using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace csci485.World
{
    public class MapBehavior : MonoBehaviour
    {
        public int AREA_SIZE = 10;
        public int SECTOR_SIZE = 10; //group of planets based of area size (eg sector size 10 of 500x500 areas will be 5000x5000)
 

        public int mapWidth; //in sectors
        public int mapHeight; //in sectors
        public GameObject player;
        public GameObject homePlanet;

        private GameObject[,] map;
        private Vector2Int[,] currentActiveCoordinates;
        private GameObject[,] currentActiveAreas;
        private Vector2Int homePlanetCoordinates;
            
        [SerializeField] private GameObject emptyArea = null; 
        [SerializeField] private GameObject[] areas = null;
        //[SerializeField] private GameObject[] planets = null;

        [System.Serializable]
        public struct PlanetSet
        {
            public GameObject[] planets; //array of planets for each sector
        }
        public PlanetSet[] planetSet; //equal to number of sectors

        //[SerializeField] private Vector2Int playerStartCoordinates;
        private Vector2Int currentPlayerCoordinates;
        private Vector2Int playerStartCoordinates;

        public int AreaSize
        {
            get {return AREA_SIZE;}
        }

        private void Start()
        {
            Planets.PlanetData.planets.Clear();  //clear all planet data from other sessions
            Planets.PlanetData.CrystalClumps.Clear();
            InitializeMap();
        }

        // update new coordinates and active areas on map
        public void UpdatePlayerCoordinates(Vector2Int centerPosition)
        {
            //print(centerPosition.x + " : " + centerPosition.y);

            if (centerPosition.x != currentPlayerCoordinates.x) UpdateActiveCoordinatesX(centerPosition.x);
            if (centerPosition.y != currentPlayerCoordinates.y) UpdateActiveCoordinatesY(centerPosition.y);

            currentPlayerCoordinates = centerPosition;
        }

        // Call functions to itiialize map and player at game start
        public void InitializeMap()
        {
            BuildMap();

            BuildAreaAroundPlayer();
        }

        // Fills out the map array
        private void BuildMap()
        {
            bool homePlanet = false; //has the home planet been placed?
            map = new GameObject[mapWidth * SECTOR_SIZE, mapHeight * SECTOR_SIZE];
            
            int sectorIndex = 0; //keeps track of which planet set to use
            //place planets
            for(int y=0; y<mapHeight;y++)
            {
                for(int x=0; x<mapWidth; x++)
                {
                    Vector2Int[] planetCoorinates = GetPlanetCoordinates();

                    placePlanetsInSector(sectorIndex, x, y, planetCoorinates, !homePlanet);
                    homePlanet = true;

                    sectorIndex++;
                }
            }
            //put empty areas around home planet
            for (int i = homePlanetCoordinates.x - 1; i <= homePlanetCoordinates.x + 1; i++)
            {
                for (int j = homePlanetCoordinates.y - 1; j <= homePlanetCoordinates.y + 1; j++)
                {
                    Vector2Int trueCoordinates = new Vector2Int(i, j);

                    if (trueCoordinates.x < 0) trueCoordinates.x = mapWidth * SECTOR_SIZE - 1;
                    if (trueCoordinates.y < 0) trueCoordinates.y = mapHeight * SECTOR_SIZE - 1;
                    trueCoordinates.x = Mod(trueCoordinates.x, mapWidth * SECTOR_SIZE);
                    trueCoordinates.y = Mod(trueCoordinates.y, mapWidth * SECTOR_SIZE);

                    if (map[trueCoordinates.x, trueCoordinates.y] == null)
                    {
                        map[trueCoordinates.x, trueCoordinates.y] = emptyArea;
                    }
                }
            }
            // place everything else;
            for (int y = 0; y < mapHeight * SECTOR_SIZE; y++)
            { 
                for (int x = 0; x < mapWidth * SECTOR_SIZE; x++)
                {
                    if(map[x,y] == null) // no planets
                        AddNewArea(x,y);
                }
            }
        }

        public void ResetCoordinates()
        {
            foreach(GameObject area in currentActiveAreas)
            {
                Destroy(area);
            }

            player.transform.position = new Vector3(playerStartCoordinates.x * AREA_SIZE, playerStartCoordinates.y * AREA_SIZE);
            currentPlayerCoordinates = playerStartCoordinates;

            int xCoord = currentPlayerCoordinates.x - 1;
            int yCoord = currentPlayerCoordinates.y - 1;

            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    currentActiveCoordinates[x, y] = new Vector2Int(xCoord + x, yCoord + y);

                    currentActiveAreas[x, y] = InstantiateArea(currentActiveCoordinates[x, y]);
                }
            }
        }

        //Adds a random area to map during initialization
        private void AddNewArea(int xCoord, int yCoord)
        {
            map[xCoord,yCoord] = areas[Random.Range(0,areas.Length)]; //temp no randomness      
        }

        //initialize the player to the start position
        private void InitializeHomePlanetandPlayerPosition(int xPos, int yPos)
        {
            Vector3 position = new Vector3(xPos*AREA_SIZE, yPos*AREA_SIZE);

            currentPlayerCoordinates.x = xPos;
            currentPlayerCoordinates.y = yPos;

            playerStartCoordinates = new Vector2Int(xPos, yPos);

            player.transform.position = position;
        }

        // gets the coodinates from the map and loads the 9 areas around the player
        private void BuildAreaAroundPlayer()
        {
            currentActiveCoordinates = new Vector2Int[3,3];
            currentActiveAreas = new GameObject[3,3];

            int xCoord = currentPlayerCoordinates.x - 1;
            int yCoord = currentPlayerCoordinates.y - 1;

            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    currentActiveCoordinates[x,y] = new Vector2Int(xCoord+x,yCoord+y);

                    currentActiveAreas[x,y] = InstantiateArea(currentActiveCoordinates[x, y]);
                }
            }
        }

        //instantiates area at given coordinates
        private GameObject InstantiateArea(Vector2Int coordinates)
        {
            int xPos = AREA_SIZE * coordinates.x;
            int yPos = AREA_SIZE * coordinates.y; //* -1; // flip y plane
            Vector3 position = new Vector3(xPos, yPos);

            int x = Mod(coordinates.x,mapWidth * SECTOR_SIZE);
            int y = Mod(coordinates.y,mapHeight * SECTOR_SIZE);

            return Instantiate(map[x, y], position, Quaternion.identity);        
        }

        //remove farthest column and insert new one
        private void UpdateActiveCoordinatesX(int newX)
        {
            int diff = newX - currentPlayerCoordinates.x;
            if (diff == 1)
            {
                for (int i = 0; i < 3; i++)
                {
                    Destroy(currentActiveAreas[0, i]);
                    
                    currentActiveCoordinates[0, i] = currentActiveCoordinates[1, i];
                    currentActiveCoordinates[1, i] = currentActiveCoordinates[2, i];
                    currentActiveCoordinates[2, i] = currentActiveCoordinates[2, i] + new Vector2Int(1, 0);

                    //print(currentActiveCoordinates[i, 2]);
                    currentActiveAreas[0, i] = currentActiveAreas[1, i];
                    currentActiveAreas[1, i] = currentActiveAreas[2, i];
                    currentActiveAreas[2, i] = InstantiateArea(currentActiveCoordinates[2, i]);
                }
            }
            else if (diff == -1)
            {
                for (int i = 0; i < 3; i++)
                {
                    Destroy(currentActiveAreas[2, i]);

                    currentActiveCoordinates[2, i] = currentActiveCoordinates[1, i];
                    currentActiveCoordinates[1, i] = currentActiveCoordinates[0, i];
                    currentActiveCoordinates[0, i] = currentActiveCoordinates[0, i] + new Vector2Int(-1, 0);

                    //print(currentActiveCoordinates[i, 2]);
                    currentActiveAreas[2, i] = currentActiveAreas[1, i];
                    currentActiveAreas[1, i] = currentActiveAreas[0, i];
                    currentActiveAreas[0, i] = InstantiateArea(currentActiveCoordinates[0, i]);
                }
            }
        }

        //remove farthest row and insert new one
        private void UpdateActiveCoordinatesY(int newY)
        {
            int diff = newY - currentPlayerCoordinates.y;

            if (diff == 1)
            {
                for(int i = 0; i<3; i++)
                {
                    Destroy(currentActiveAreas[i, 0]);
                    

                    currentActiveCoordinates[i, 0] = currentActiveCoordinates[i, 1];
                    currentActiveCoordinates[i, 1] = currentActiveCoordinates[i, 2];
                    currentActiveCoordinates[i, 2] = currentActiveCoordinates[i, 2] + new Vector2Int(0,1);

                    //print(currentActiveCoordinates[i, 2]);
                    currentActiveAreas[i, 0] = currentActiveAreas[i, 1];
                    currentActiveAreas[i, 1] = currentActiveAreas[i, 2];
                    currentActiveAreas[i, 2] = InstantiateArea(currentActiveCoordinates[i, 2]);
                }
            }
            else if(diff == -1)
            {
                for (int i = 0; i < 3; i++)
                {
                    Destroy(currentActiveAreas[i, 2]);

                    currentActiveCoordinates[i, 2] = currentActiveCoordinates[i, 1];
                    currentActiveCoordinates[i, 1] = currentActiveCoordinates[i, 0];
                    currentActiveCoordinates[i, 0] = currentActiveCoordinates[i, 0] + new Vector2Int(0, -1);

                    //print(currentActiveCoordinates[i, 2]);
                    currentActiveAreas[i, 2] = currentActiveAreas[i, 1];
                    currentActiveAreas[i, 1] = currentActiveAreas[i, 0];
                    currentActiveAreas[i, 0] = InstantiateArea(currentActiveCoordinates[i, 0]);
                }
            }
        }

        //Get Random Unique X value over iterated y value
        private Vector2Int[] GetPlanetCoordinates()
        {       
            Vector2Int[] planetCoordinates = new Vector2Int[SECTOR_SIZE];

            int[] tmpArr = new int [SECTOR_SIZE];
            for (int i = 0; i < SECTOR_SIZE; i++)
            {
                tmpArr[i] = i;
            }
            for (int i = 0; i < SECTOR_SIZE; i++)
            {
                int tmp = tmpArr[i];
                int rnd = Random.Range(i, SECTOR_SIZE);
                tmpArr[i] = tmpArr[rnd];
                tmpArr[rnd] = tmp;

                planetCoordinates[i] = new Vector2Int(tmpArr[i], i);
            }
            
            return planetCoordinates;
        }

        //Adds a random area to map during initialization
        private void placePlanetsInSector(int planetSet, int xSect, int ySect, Vector2Int[] planetCoords, bool placeHomePlanet)
        {
            for(int i = 0; i < SECTOR_SIZE; i++)
            {
                int x = planetCoords[i].x + xSect * SECTOR_SIZE;
                int y = planetCoords[i].y + ySect * SECTOR_SIZE;

                if (i == (SECTOR_SIZE -1) && placeHomePlanet) //last element
                {
                    InitializeHomePlanetandPlayerPosition(x,y);
                    map[x, y] = homePlanet;
                    homePlanetCoordinates = new Vector2Int(x,y);

                    placeHomePlanet = false;
                }
                else
                {
                    PlacePlanet(x, y, planetSet, i);
                }
            }
        }

        //Adds a random area to map during initialization
        private void PlacePlanet(int xCoord, int yCoord, int set, int index)
        {
            map[xCoord, yCoord] = planetSet[set].planets[index];
        }



        // Get the mod of an int
        int Mod(int a, int b)
        {
            return a - b * (int) Mathf.Floor((float) a / (float) b);
        }
    }
}


