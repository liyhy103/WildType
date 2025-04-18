using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using System.Diagnostics;
using System;


public class Challenge : MonoBehaviour
{

    public TMP_Text challengeText;
    private List<string> challenges = new List<string>();
    public Creature currentCreature;
    private string currentChallenge = "";

    public void Start()
    {
        

        if (challengeText == null)
        {
            UnityEngine.Debug.LogError("challengeText is not assigned in the Inspector!");
            return;
        }
        UnityEngine.Debug.Log("challengeText is correctly assigned.");

        SetListItem();
        challengeText.text = "";
        challengeText.gameObject.SetActive(false);
    }

    private void SetListItem()
    {
        // Add challenges to the list
        challenges.Add("Green");
        challenges.Add("Yellow");

    }

    public void SetChallengeText(string text)
    {
        challengeText.text = text;
    }

    public void OnButtonClick()
    {
        //check challenges is not empty
        if (challenges.Count > 0)
        {
            currentChallenge = challenges[UnityEngine.Random.Range(0, challenges.Count)];
            string challenge = "Breed a " + currentChallenge + " creature";
            SetChallengeText(challenge);

        }
        else
        {
            //print message saying all challenges completed
            string completed = "All challenges have been completed";
            SetChallengeText(completed);
        }
            //displays the challenge
            challengeText.gameObject.SetActive(true);
    }
    public void Update()
    {
        if (currentCreature == null)
        {
            UnityEngine.Debug.LogWarning("No creature has been bred yet!");
            return;  // Skip the challenge check if there's no creature
        }

        //check if set challenge is blue or red
        if (currentChallenge == "Blue")
        {
            //check if creature is blue 
            bool CreatureIsBlue = CheckCreatureHasGene("Blue");
            if (CreatureIsBlue)
            {
                String Completed = "You Completed this challenge";
                SetChallengeText(Completed);
                challenges.Remove(currentChallenge);
                currentChallenge = "";
            }
        }
        else if (currentChallenge == "Red")
        {
            //check if creature is red 
            bool CreatureIsred = CheckCreatureHasGene("Red");
            if (CreatureIsred)
            {
                String Completed = "You Completed this challenge";
                SetChallengeText(Completed);
                challenges.Remove(currentChallenge);
                currentChallenge = "";
            }
        }
    }

    private bool CheckCreatureHasGene(string challengeGene)
    {
        
        if (currentCreature != null)
        {
            // Get the phenotype of the current creature
            string creaturePhenotype = currentCreature.GetPhenotype();

            // Check if the creature's phenotype matches the challenge gene
            return creaturePhenotype.Equals(challengeGene, StringComparison.OrdinalIgnoreCase);
        }
        UnityEngine.Debug.LogWarning("currentCreature is not assigned!");

        return false;
    }
}
