using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace csci485.Management
{
    public class HUD_Text : MonoBehaviour
    {

        Text textField;

        void Awake()
        {
            textField = GetComponent<Text>();
            
        }

        //Update text val
        public void UpdateHUDValue(int val)
        {
            textField.text = val.ToString() + "/200";
        }

    }
}