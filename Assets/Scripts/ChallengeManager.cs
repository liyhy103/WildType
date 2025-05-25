using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using System.Diagnostics;
using System;
using Debug = UnityEngine.Debug;


public class Challenge : MonoBehaviour
{

    public TMP_Text challengeText;
    private List<string> challenges = new List<string>();
    public Creature currentCreature;
    private string currentChallenge = "";
    public string CurrentChallenge => currentChallenge;
    private string result = "";

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
        challenges.Add("Green");
        challenges.Add("Yellow");

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
    }

    public void Update()
    {
        if (currentCreature == null)
        {
            Debug.LogWarning("No creature has been bred yet!");
            return;
        }

        if (result.ToLower() == currentChallenge.ToLower())
        {
            SetChallengeText("You Completed this challenge");
            challenges.Remove(currentChallenge);
            PickNextChallenge();

            // Optionally reset currentCreature & result so it only triggers once per challenge
            currentCreature = null;
            result = "";
        }
    }
    private void PickNextChallenge()
    {
        if (challenges.Count > 0)
        {
            currentChallenge = challenges[0];
            string challenge = "Breed a " + currentChallenge + " creature";
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
