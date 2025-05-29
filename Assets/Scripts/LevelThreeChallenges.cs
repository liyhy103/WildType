using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using System.Diagnostics;
using System;
using Debug = UnityEngine.Debug;


public class LevelThreeChallenges : Challenge
{



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

        PickNextChallenge();
        challengeText.gameObject.SetActive(true);

    }

    private void SetListItem()
    {
        // Add challenges to the list
        challenges.Add("Long");
        challenges.Add("Short");
        challenges.Add("No");

    }

    public void SetChallengeText(string text)
    {
        if (challengeText != null)
        {
            challengeText.text = text;
            UnityEngine.Debug.Log("[Challenge] Showing challenge text: " + text);
        }
    }

    public void SetResult(string phenotype, Creature creature)
    {
        result = phenotype;
        currentCreature = creature;
        Debug.Log("[Challenge] Received result: " + phenotype);
        ProcessResult();
    }

    private void ProcessResult()
    {
        if (currentCreature == null)
        {
            Debug.LogWarning("No creature has been bred yet!");
            return;
        }

        Debug.LogWarning(currentChallenge + " vs " + result);
        if (result.ToLower() == currentChallenge.ToLower())
        {
            SetChallengeText("You Completed this challenge");
            challenges.Remove(currentChallenge);
            PickNextChallenge();

        }

        // Reset to prevent multiple triggers
        currentCreature = null;
        result = "";
    }

    private void PickNextChallenge()
    {
        if (challenges.Count > 0)
        {
            currentChallenge = challenges[0];
            string challenge = "Breed a " + currentChallenge + " horned creature";
            SetChallengeText(challenge);
            Debug.Log("[Challenge] CurrentChallenge set to: " + currentChallenge);
        }
        else
        {
            string completed = "All challenges have been completed";
            SetChallengeText(completed);
            currentChallenge = "";
        }
    }
}
