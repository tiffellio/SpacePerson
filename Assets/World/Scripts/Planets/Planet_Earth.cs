using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using csci485.Character;

namespace csci485.World.Planets
{
    public class Planet_Earth: Planet
    {
        [SerializeField] private int requiredFood = 150;
        [SerializeField] private int requiredWater = 150;
        [SerializeField] private int requiredCystals = 0;

        
        [SerializeField] PlanetStoryDialogue storyDialogue = null;
        private PlanetDialogueCanvas dialogueCanvas = null;
        private PlanetUI planetUI = null;
        private ResourceManagement playerResourses = null;

        public Queue<string> introDialogueQueue = new Queue<string>();

        public static bool introCompleted = false;

        // handles the planet functionality when the planet interacts with it
        public override void Interact()
        {
            planetUI.changeName(storyDialogue.planetName);
            if (!introCompleted) return;

            SetTextTo(storyDialogue.defaultIntro);
            showText();
            print(Time.time);
            dialogueCanvas.SetResponseText("What would you like to do?");
            DisplayPlanetOptions();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if(!introCompleted) return;

            if (collision.tag != "Player") return;
            hideText();
        }

        private void OnEnable()
        {
            planetUI = PlanetData.planetUI;
            dialogueCanvas = PlanetData.planetDialogueCanvas;
            hideText();
            if (!introCompleted) IntroSequence(); //run intro sequence 
        }

        public void showText()
        {
            planetUI.gameObject.SetActive(true);
        }

        public void hideText()
        {
            planetUI.gameObject.SetActive(false);
            dialogueCanvas.gameObject.SetActive(false);
        }

        public void SetTextTo(string text)
        {
            planetUI.changeText(text);
        }

        public void PositiveResponce()
        {
            playerResourses = FindObjectOfType<ResourceManagement>();
            if(playerResourses.water >= requiredWater && playerResourses.tree >= requiredFood && playerResourses.crystal >= requiredCystals)
            {
                SetTextTo("Yay! You saved us!");
            }
            else
            {
                FindObjectOfType<PlayerController>().Heal();
                SetTextTo("Come back when you have everything! We fixed your ship up in the meantime...");
            }
            dialogueCanvas.gameObject.SetActive(false);
        }

        public void NegativeResponce()
        {
            SetTextTo("Awwee, Okay.. Please Hurry! We fixed your ship up in the meantime...");
            dialogueCanvas.gameObject.SetActive(false);
            FindObjectOfType<PlayerController>().Heal();
        }

        private void DisplayPlanetOptions()
        {
            //reset buttons
            dialogueCanvas.ResetButtons();
            dialogueCanvas.hideButtonThree();
            dialogueCanvas.SetButtonOne("Yes", PositiveResponce);
            dialogueCanvas.SetButtonTwo("Not yet..", NegativeResponce);
            dialogueCanvas.gameObject.SetActive(true);
        }

        public void IntroSequence()
        {
            dialogueCanvas.ResetButtons();
            introDialogueQueue.Clear(); 
            foreach (string section in storyDialogue.dialogue) introDialogueQueue.Enqueue(section);

            dialogueCanvas.SetResponseText("");
            dialogueCanvas.hideButtonOne();
            dialogueCanvas.hideButtonThree();
            dialogueCanvas.SetButtonTwo("Continue", DisplayNextSection);
            dialogueCanvas.gameObject.SetActive(true);

            planetUI.changeName(storyDialogue.planetName);
            DisplayNextSection();

            showText();
        }

        public void DisplayNextSection()
        {
            if(introDialogueQueue.Count == 0)
            {
                EndIntroSuquence();
                return;
            }
            string next = introDialogueQueue.Dequeue();
            SetTextTo(next);
        }

        private void EndIntroSuquence()
        {
            hideText();
            introCompleted = true;
        }

    }
}


