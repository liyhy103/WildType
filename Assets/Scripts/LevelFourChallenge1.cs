using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using System.Diagnostics;
using System;
using Debug = UnityEngine.Debug;

public class LevelFourChallenge : Challenge
{

    private List<string> colourcoatChallenge = new List<string>();
    private List<string> tailChallenge = new List<string>();

    private string currentCoatColourChallenge = "";
    private string currentTailChallenge = "";

   // public string currentCoatColourChallenge => currentCoatColourChallenge;
   // public string currentTailChallenge => currentTailChallenge;



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
        colourcoatChallenge.Add("Dark");
        colourcoatChallenge.Add("Light");
        colourcoatChallenge.Add("Light");
        colourcoatChallenge.Add("Dark");
    }
    private void SetGenderList()
    {
        tailChallenge.Add("Female");
        tailChallenge.Add("Male");
        tailChallenge.Add("Female");
        tailChallenge.Add("Male");

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

        string expectedResult = currentCoatColourChallenge + "_" + currentTailChallenge;
        Debug.LogWarning(expectedResult + " vs " + result);

        if (result.ToLower() == expectedResult.ToLower())
        {
            SetChallengeText("You Completed this challenge");

            int index = -1;
            for (int i = 0; i < colourcoatChallenge.Count; i++)
            {
                if (colourcoatChallenge[i] == currentCoatColourChallenge && tailChallenge[i] == currentTailChallenge)
                {
                    index = i;
                    break;
                }
            }

            if (index >= 0)
            {
                colourcoatChallenge.RemoveAt(index);
                tailChallenge.RemoveAt(index);
            }

            PickNextChallenge();
        }


        // Reset to prevent multiple triggers
        currentCreature = null;
        result = "";
    }
    private void PickNextChallenge()
    {
        if (colourcoatChallenge.Count > 0 && tailChallenge.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, colourcoatChallenge.Count);
            currentCoatColourChallenge = colourcoatChallenge[index];
            currentTailChallenge = tailChallenge[index];

            currentChallenge = "Breed a " + currentCoatColourChallenge + " shelled " + currentTailChallenge + " creature";
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
