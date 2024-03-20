using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using csci485.Character;

namespace csci485.World.Planets
{
    public class Planet_Friendly : Planet
    {
        public const string planetNormal = "This planet appears to be doing well..";
        public const string planetThriving= "Wow! This planet is thriving!";
        public const string planetDying = "Oh no! This poor planet is struggling...";
        public const string planetDead= "Oops! This planet is dead!";

        public Queue<string> dialogueQueue = new Queue<string>();
        public int refuelCost = 50;

        [SerializeField] PlanetBehavior behavior = null; // how the planet reacts
        private PlanetDialogueCanvas dialogueCanvas = null;
        private PlanetUI planetUI = null;
        private ResourceManagement playerResourses = null;

        private float lastSuccessfulAction = -9999;
        private float resetTime = 60f; // will have to be saved to a static class
        private bool visited;// has the player visited this planet before

        PlanetData.planet thisPlanet;

        // handles the planet functionality when the planet interacts with it
        public override void Interact()
        {
            ResolveInteract();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag != "Player") return;

            hideText();
        }

        private void Awake()
        {
            
            if (PlanetData.planets.TryGetValue(behavior.name, out thisPlanet))
            {
                lastSuccessfulAction = thisPlanet.lastSuccessfulActionTime;
                currentPlanetState = thisPlanet.currentState;
                visited = true;
            }
            else
            {
                thisPlanet.lastSuccessfulActionTime = lastSuccessfulAction;
                thisPlanet.currentState = currentPlanetState;
                PlanetData.planets.Add(behavior.name,thisPlanet);
                visited = false; // first time visiting planet

            }
            
        }
        private void OnEnable()
        {
            planetUI = PlanetData.planetUI;
            dialogueCanvas = PlanetData.planetDialogueCanvas;
            hideText();
        }

        public void showText()
        {
            planetUI.gameObject.SetActive(true);
        }

        public void hideText()
        {
            if(!planetUI) planetUI = FindObjectOfType<PlanetUI>();
            planetUI.gameObject.SetActive(false);
            dialogueCanvas.gameObject.SetActive(false);
        }

        public void SetTextTo(string text)
        {
            planetUI.changeText(text);
        }

        public void ResolveInteract()
        {
            PlanetUIList uiList = FindObjectOfType<PlanetUIList>();
            if (uiList != null)
            {
                uiList.UpdateUIList(behavior.dialogueSet.planetName);
            }

            planetUI.changeName(behavior.dialogueSet.planetName);
            if ((lastSuccessfulAction + resetTime) > Time.time)
            {
                SetTextTo("This planet is busy... Come back Later.");
                showText();
                return;
            }
            if (currentPlanetState == PlanetStates.dead)
            {
                SetTextTo(planetDead);
                showText();
            }
            else
            {
                switch (currentPlanetState)
                {
                    case PlanetStates.normal:
                    SetTextTo(planetNormal);
                    break;
                    case PlanetStates.dying:
                    SetTextTo(planetDying);
                    break;
                    case PlanetStates.thriving:
                    SetTextTo(planetThriving);
                    break;
                }
                dialogueCanvas.SetResponseText("What would you like to do?");
                showText(); 
                DisplayPlanetOptions();
            }
        }

        // Set up trade
        public void Talk()
        {
            dialogueCanvas.ResetButtons();
            dialogueQueue.Clear();
            string [] dialogue;
            if (visited)
            {
                dialogue = behavior.dialogueSet.dialogue_repeatVisit;
            }
            else
            {
                dialogue = behavior.dialogueSet.dialogue_firstVisit;
            }
            foreach (string section in dialogue) dialogueQueue.Enqueue(section);

            //set up canvas
            dialogueCanvas.SetResponseText("");
            dialogueCanvas.hideButtonOne();
            dialogueCanvas.hideButtonThree();
            dialogueCanvas.SetButtonTwo("Continue", DisplayNextSection);
            dialogueCanvas.gameObject.SetActive(true);

            DisplayNextSection();
        }

        public void DisplayNextSection()
        {
            if (dialogueQueue.Count == 0)
            {
                EndDialogueSuquence();
                return;
            }
            string next = dialogueQueue.Dequeue();
            SetTextTo(next);
        }

        private void EndDialogueSuquence()
        {
            SetTextTo("Would you like to trade?");
            dialogueCanvas.SetResponseText("Trade " + behavior.requiredAmount + " " + behavior.requiredResourse 
                                       + " for " + behavior.offeredAmount + " " + behavior.offeredResourse + "?");
            DisplayTradeOptions();
        }

        public void Steal()
        {
            playerResourses = FindObjectOfType<ResourceManagement>();
            string item = playerResourses.addRandom(15);
            SetTextTo("Hey! You stole 15 "+ item +" from me! Now I'm not going to survive...");
            currentPlanetState--;
            thisPlanet.currentState = currentPlanetState;
            lastSuccessfulAction = Time.time;
            thisPlanet.lastSuccessfulActionTime = lastSuccessfulAction;
            PlanetData.planets[behavior.name] = thisPlanet;
            //dialogueCanvas.gameObject.SetActive(false);

            dialogueCanvas.SetResponseText("");
            dialogueCanvas.hideButtonOne();
            dialogueCanvas.hideButtonThree();
            dialogueCanvas.SetButtonTwo("Continue", CloseConversation);
            dialogueCanvas.gameObject.SetActive(true);
        }

        private void DisplayPlanetOptions()
        {
            dialogueCanvas.ResetButtons();
            dialogueCanvas.SetButtonOne("Talk", Talk);
            dialogueCanvas.SetButtonTwo("Steal Resourse", Steal);
            dialogueCanvas.SetButtonThree("Refuel (50 Crystals)", Refuel);
            dialogueCanvas.gameObject.SetActive(true);
        }

        private void DisplayTradeOptions()
        {
            dialogueCanvas.ResetButtons();
            dialogueCanvas.hideButtonThree();
            dialogueCanvas.SetButtonOne("Accept", AttemptTrade);
            dialogueCanvas.SetButtonTwo("Reject", RejectTrade);
            dialogueCanvas.gameObject.SetActive(true);
        }

        public void AttemptTrade()
        {
            playerResourses = FindObjectOfType<ResourceManagement>();
            if(playerResourses.trade(behavior.requiredResourse, behavior.requiredAmount, behavior.offeredResourse, behavior.offeredAmount))
            {
                SetTextTo("Thanks for the  " + behavior.requiredResourse + "! This will help my inhabitants..");
                if(currentPlanetState != PlanetStates.thriving){ currentPlanetState++; thisPlanet.currentState = currentPlanetState;}
                lastSuccessfulAction = Time.time;
                thisPlanet.lastSuccessfulActionTime = lastSuccessfulAction;
                PlanetData.planets[behavior.name] = thisPlanet;
            }
            else
            {
                SetTextTo("It doesn't look like you have enough " +behavior.requiredResourse + "...");
            }
            dialogueCanvas.gameObject.SetActive(false);
        }

        public void RejectTrade()
        {
            SetTextTo("Goodbye, human.");
            dialogueCanvas.SetResponseText("");
            dialogueCanvas.hideButtonOne();
            dialogueCanvas.SetButtonTwo("Continue", CloseConversation);
            dialogueCanvas.gameObject.SetActive(true);
        }

        public void Refuel()
        {
            playerResourses = FindObjectOfType<ResourceManagement>();
            if (playerResourses.Refuel(refuelCost))
            {
                SetTextTo("You're all topped up!");
            }
            else
            {
                SetTextTo("You're a little short on crystals...");
            }
            dialogueCanvas.SetResponseText("");
            dialogueCanvas.hideButtonOne();
            dialogueCanvas.hideButtonThree();
            dialogueCanvas.SetButtonTwo("Continue", CloseConversation);
            dialogueCanvas.gameObject.SetActive(true);
        }

        public void CloseConversation()
        {
            hideText();
            dialogueCanvas.gameObject.SetActive(false);
        }
    }
}


