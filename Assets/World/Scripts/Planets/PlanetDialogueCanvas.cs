using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace csci485.World.Planets
{
    public class PlanetDialogueCanvas : MonoBehaviour
    {
        [SerializeField] private Button btn1;
        [SerializeField] private Button btn2;
        [SerializeField] private Button btn3;
        [SerializeField] private TMPro.TMP_Text reponseText;

        private void Start()
        {
            PlanetData.planetDialogueCanvas = this;
        }

        public void SetButtonOne(string title, UnityEngine.Events.UnityAction action)
        {
            btn1.GetComponentInChildren<Text>().text = title;
            btn1.onClick.AddListener(action);
        }

        public void SetButtonTwo(string title, UnityEngine.Events.UnityAction action)
        {
            btn2.GetComponentInChildren<Text>().text = title;
            btn2.onClick.AddListener(action);
        }

        public void SetButtonThree(string title, UnityEngine.Events.UnityAction action)
        {
            btn3.GetComponentInChildren<Text>().text = title;
            btn3.onClick.AddListener(action);
        }

        public void ResetButtons()
        {
            btn1.onClick.RemoveAllListeners();
            btn2.onClick.RemoveAllListeners();
            btn3.onClick.RemoveAllListeners();
            btn1.gameObject.SetActive(true);
            btn2.gameObject.SetActive(true);
            btn3.gameObject.SetActive(true);
        }

        public void SetResponseText(string text)
        {
            reponseText.text = text;
        }

        public void hideButtonOne()
        {
            btn1.gameObject.SetActive(false);
        }

        public void hideButtonTwo()
        {
            btn2.gameObject.SetActive(false);
        }

        public void hideButtonThree()
        {
            btn3.gameObject.SetActive(false);
        }

    }
}


