using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using System.Diagnostics;
using System;
using Debug = UnityEngine.Debug;

public class LevelTwoChallenge : Challenge
{

    private List<string> shellChallenges = new List<string>();
    private List<string> genderChallenges = new List<string>();

    private string currentShellChallenge = "";
    private string currentGenderChallenge = "";

    public string CurrentShellChallenge => currentShellChallenge;
    public string CurrentGenderChallenge => currentGenderChallenge;



    public void Start()
    {

        SetChallengeText("Level 2: Special Challenge!");

        if (challengeText == null)
        {
            UnityEngine.Debug.LogError("challengeText is not assigned in the Inspector!");
            return;
        }
        UnityEngine.Debug.Log("challengeText is correctly assigned.");

        SetColourList();
        SetGenderList();
        challengeText.text = "";
        challengeText.gameObject.SetActive(false);

        PickNextChallenge();
        challengeText.gameObject.SetActive(true);

    }

    private void SetColourList()
    {
        shellChallenges.Add("Dark");
        shellChallenges.Add("Light");
        shellChallenges.Add("Light");
        shellChallenges.Add("Dark");
    }
    private void SetGenderList()
    {
        genderChallenges.Add("Female");
        genderChallenges.Add("Male");
        genderChallenges.Add("Female");
        genderChallenges.Add("Male");

    }
    public void SetChallengeText(string text)
    {
        if (challengeText != null)
        {
            challengeText.text = text;
            UnityEngine.Debug.Log("[Challenge] Showing challenge text: " + text);
        }
    }

    public void SetResult(string phenotype, string gender, Creature creature)
    {
        result = phenotype + "_" + gender; 
        currentCreature = creature;
        UnityEngine.Debug.Log("[Challenge] Received result: " + result);
        ProcessResult();
    }

    private void ProcessResult()
    {
        if (currentCreature == null)
        {
            Debug.LogWarning("No creature has been bred yet!");
            return;
        }

        string expectedResult = currentShellChallenge + "_" + currentGenderChallenge;
        Debug.LogWarning(expectedResult + " vs " + result);

        if (result.ToLower() == expectedResult.ToLower())
        {
            SetChallengeText("You Completed this challenge");

            int index = -1;
            for (int i = 0; i < shellChallenges.Count; i++)
            {
                if (shellChallenges[i] == currentShellChallenge && genderChallenges[i] == currentGenderChallenge)
                {
                    index = i;
                    break;
                }
            }

            if (index >= 0)
            {
                shellChallenges.RemoveAt(index);
                genderChallenges.RemoveAt(index);
            }

            PickNextChallenge();
        }


        // Reset to prevent multiple triggers
        currentCreature = null;
        result = "";
    }
    private void PickNextChallenge()
    {
        if (shellChallenges.Count > 0 && genderChallenges.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, shellChallenges.Count);
            currentShellChallenge = shellChallenges[index];
            currentGenderChallenge = genderChallenges[index];

            currentChallenge = "Breed a " + currentShellChallenge + " shelled " + currentGenderChallenge + " creature";
            SetChallengeText(currentChallenge);
            UnityEngine.Debug.Log("[Challenge] CurrentChallenge set to: " + currentChallenge);
        }
        else
        {
            SetChallengeText("All challenges have been completed!");
            currentChallenge = "";
        }
    }
}
