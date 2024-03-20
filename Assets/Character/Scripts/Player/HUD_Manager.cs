using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

namespace csci485.Management
{

    public class HUD_Manager : MonoBehaviour

    {
        
        [SerializeField] private ProgressBar healthBar = null;
        [SerializeField] private ProgressBar fuelBar = null;

        [SerializeField] private HUD_Text crystalText = null;
        [SerializeField] private HUD_Text waterText = null;
        [SerializeField] private HUD_Text treeText = null;
        void Start()
        {
            
            if (healthBar)
            {
                csci485.Character.PlayerController manager = GameObject.FindGameObjectWithTag("Player").GetComponent<csci485.Character.PlayerController>();
                manager.onCurrentHealthChange += UpdateCurrentHealth;
                manager.onMaxHealthChange += UpdateMaxHealth;
                manager.onMinHealthChange += UpdateMinHealth;
            }
            if (fuelBar)
            {
                csci485.Character.ResourceManagement manager = GameObject.FindGameObjectWithTag("Player").GetComponent<csci485.Character.ResourceManagement>();
                manager.onCurrentFuelChange += UpdateCurrentFuel;
                manager.onMaxFuelChange += UpdateMaxFuel;
                manager.onMinFuelChange += UpdateMinFuel;
            }

            if (crystalText)
            {
                csci485.Character.ResourceManagement manager = GameObject.FindGameObjectWithTag("Player").GetComponent<csci485.Character.ResourceManagement>();
                manager.onCrystalChange+= UpdateCrystal;
            }

            if(waterText)
            {
                csci485.Character.ResourceManagement manager = GameObject.FindGameObjectWithTag("Player").GetComponent<csci485.Character.ResourceManagement>();
                manager.onWaterChange += UpdateWater;

            }
            if (treeText)
            {
                csci485.Character.ResourceManagement manager = GameObject.FindGameObjectWithTag("Player").GetComponent<csci485.Character.ResourceManagement>();
                manager.onTreeChange += UpdateTree;
            }



        }

        public void UpdateCurrentHealth(float hp)
        {
            healthBar.UpdateCurrentValue(hp);
        }

        
        public void UpdateMaxHealth(float maxHp)
        {
            healthBar.UpdateMaxValue(maxHp);
        }

        public void UpdateMinHealth(float minHp)
        {
            healthBar.UpdateMinValue(minHp);
        }


        public void UpdateCurrentFuel(float fuelLevel)
        {
            fuelBar.UpdateCurrentValue(fuelLevel);
        }


        public void UpdateMaxFuel(float maxFuel)
        {
            fuelBar.UpdateMaxValue(maxFuel);
        }

        public void UpdateMinFuel(float minFuel)
        {
            fuelBar.UpdateMinValue(minFuel);
        }

        public void UpdateCrystal(int count)
        {
            crystalText.UpdateHUDValue(count);
        }

        public void UpdateWater(int count)
        {
            waterText.UpdateHUDValue(count);
        }

        public void UpdateTree(int count)
        {
            treeText.UpdateHUDValue(count);
        }


    }
}