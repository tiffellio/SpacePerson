using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace csci485.Character
{ 
    public class ResourceManagement : MonoBehaviour
    {
        [SerializeField] private int crystalCount;
        [SerializeField] private int waterCount;
        [SerializeField] private int treeCount;
        [SerializeField] private int fuelCount;

        //private static int minResource = 0;
        //[SerializeField] private int startAmount = 50; ??
       
        public delegate void OnCurrentFuelChange(float hp);
        public OnCurrentFuelChange onCurrentFuelChange;

        public delegate void OnMaxFuelChange(float hp);
        public OnMaxFuelChange onMaxFuelChange;

        public delegate void OnMinFuelChange(float hp);
        public OnMinFuelChange onMinFuelChange;


        public delegate void OnCrystalChange(int count);
        public OnCrystalChange onCrystalChange;

        public delegate void OnWaterChange(int count);
        public OnWaterChange onWaterChange;

        public delegate void OnTreeChange(int count);
        public OnTreeChange onTreeChange;

        //Set players starting resources to 0
        //Wrong place to do this? how much should they start with?
        private void Start()
        {
            crystalCount = 0;
            onCrystalChange?.Invoke(crystalCount);
            waterCount = 0;
            onWaterChange?.Invoke(waterCount);
            treeCount = 0;
            onTreeChange?.Invoke(waterCount);
            fuelCount = 100;
            onMaxFuelChange?.Invoke(fuelCount);
            onCurrentFuelChange?.Invoke(fuelCount);
        }


        public int crystal
        {
            get { return crystalCount; }
            set 
            { 
                crystalCount = value;
                onCrystalChange?.Invoke(crystalCount);
            }
        }

        public int water
        {
            get { return waterCount; }
            set 
            { 
                waterCount = value;
                onWaterChange?.Invoke(waterCount);
            }
        }

        public int tree
        {
            get { return treeCount; }
            set 
            { 
                treeCount = value;
                onTreeChange?.Invoke(treeCount);
            }
        }

        public int fuel
        {
            get { return fuelCount; }
            set 
            { 
                fuelCount = value;
                onCurrentFuelChange?.Invoke(fuelCount);
            }
            
        }


        //used to increment/decrement resource vals
        public void addCrystal(int val)
        {
            crystalCount = crystalCount + val;
            onCrystalChange?.Invoke(crystalCount);
        }

        public void addWater(int val)
        {
            waterCount = waterCount + val;
            onWaterChange?.Invoke(waterCount);
        }

        public void addTree(int val)
        {
            treeCount = treeCount + val;
            onTreeChange?.Invoke(treeCount);
        }

        public void addFuel(int val)
        {
            fuelCount = fuelCount + val;
            onCurrentFuelChange?.Invoke(fuelCount);
        }

        public string addRandom(int val)
        {
            int r = Random.Range(0,3);
            if (r == 2)
            {
                waterCount += val;
                onWaterChange?.Invoke(waterCount);
                return "water";
            }
            else if (r == 1)
            {
                treeCount += val;
                onTreeChange?.Invoke(treeCount);
                return "food items";
            }
            else
            {
                crystalCount += val;
                onCrystalChange?.Invoke(crystalCount);
                return "cystals";
            }
        } 


        //For dying/ captured by pirates???
        public void loseHalf()
        { 
           crystalCount = crystalCount/2;
           onCrystalChange?.Invoke(crystalCount);
           waterCount = waterCount/2;
           onWaterChange?.Invoke(waterCount);
           treeCount = treeCount/2;
           onTreeChange?.Invoke(treeCount);
        }
            
        //For spending crystals
        //returns true if player has enough resource, false if not
        public bool useCrystal(int val)
        {
           if(crystalCount >= val)
            {
                crystalCount = crystalCount - val;
                onCrystalChange?.Invoke(crystalCount);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool useWater(int val)
        {
            if (waterCount >= val)
            {
                waterCount = waterCount - val;
                onWaterChange?.Invoke(waterCount);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool useTree(int val)
        {
            if (treeCount >= val)
            {
                treeCount = treeCount - val;
                onTreeChange?.Invoke(treeCount);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool useFuel(int val)
        {
            if (fuelCount >= val)
            {
                fuelCount = fuelCount - val;
                onCurrentFuelChange?.Invoke(fuelCount);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Refuel(int crytsalAmount)
        {
            if(useCrystal(crytsalAmount))
            {
                fuelCount = 100;
                onCurrentFuelChange?.Invoke(fuelCount);
                return true;
            }
            else
            {
                return false;
            }
        }

        //takes resource to trade/trade for as char val (c,w, or t)
        //returns true on a successful trade
        //returns false on not enough resources or error
        //"kinda janky" Zack 11-11 2020
        public bool trade(ResourseTypes playerRes, int playerVal, ResourseTypes planetRes, int planetVal)
        {
            switch (playerRes)
            {
                case ResourseTypes.crystals:
                    if (useCrystal(playerVal))
                    { }
                    else//not enough resource
                    {return false;}
                    break;
                case ResourseTypes.water:
                    if (useWater(playerVal))
                    {}
                    else
                    {return false;}
                    break;
                case ResourseTypes.food:
                    if (useTree(playerVal))
                    {}
                    else
                    {return false;}
                    break;
                case ResourseTypes.fuel:
                    if (useTree(playerVal))
                    { }
                    else
                    { return false; }
                    break;

                default:
                    UnityEngine.Debug.Log("trade: resource 1 error");
                    return false;
            }

            switch (planetRes)
            {
                case ResourseTypes.crystals:
                    addCrystal(planetVal);
                    return true;

                case ResourseTypes.water:
                    addWater(planetVal);
                    return true;

                case ResourseTypes.food:
                    addTree(planetVal);
                    return true;

                case ResourseTypes.fuel:
                    addTree(planetVal);
                    return true;

                default:
                    UnityEngine.Debug.Log("trade: resource 2 error");
                    return false;


            }
        }
    }
}
