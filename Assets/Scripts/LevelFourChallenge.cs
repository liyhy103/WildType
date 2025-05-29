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

    private List<string> TailChallenges = new List<string>();
    private List<string> colourChallenges = new List<string>();

    private string currentCollourChallenge = "";
    private string currentTailChallenge = "";

  //  public string currentCollourChallenge => currentCollourChallenge;
    //public string currentTailChallenge => currentTailChallenge;



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
        TailChallenges.Add("Green");
        TailChallenges.Add("Green");
        TailChallenges.Add("Yellow");
        TailChallenges.Add("Yellow");
    }
    private void SetGenderList()
    {
        colourChallenges.Add("Long");
        colourChallenges.Add("Short");
        colourChallenges.Add("Long");
        colourChallenges.Add("Short");

    }
    public void SetChallengeText(string text)
    {
        if (challengeText != null)
        {
            challengeText.text = text;
            UnityEngine.Debug.Log("[Challenge] Showing challenge text: " + text);
        }
    }

    public void SetResult(string colour, string tail, Creature creature)
    {
        result = tail + "_" + colour; 
        currentCreature = creature;
        UnityEngine.Debug.Log("[Challenge] Received result: " + result);
        ProcessResult();
    }

    private void ProcessResult()
    {

        UnityEngine.Debug.Log("result being processed");
        if (currentCreature == null)
        {
            Debug.LogWarning("No creature has been bred yet!");
            return;
        }

        string expectedResult = currentCollourChallenge + "_" + currentTailChallenge;
        UnityEngine.Debug.Log(expectedResult + " vs " + result);

        if (result.ToLower() == expectedResult.ToLower())
        {
            SetChallengeText("You Completed this challenge");

            int index = -1;
            for (int i = 0; i < TailChallenges.Count; i++)
            {
                if (TailChallenges[i] == currentCollourChallenge && colourChallenges[i] == currentTailChallenge)
                {
                    index = i;
                    break;
                }
            }

            if (index >= 0)
            {
                TailChallenges.RemoveAt(index);
                colourChallenges.RemoveAt(index);
            }

            PickNextChallenge();
        }


        // Reset to prevent multiple triggers
        currentCreature = null;
        result = "";
    }
    private void PickNextChallenge()
    {
        if (TailChallenges.Count > 0 && colourChallenges.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, TailChallenges.Count);
            currentCollourChallenge = TailChallenges[index];
            currentTailChallenge = colourChallenges[index];

            currentChallenge = "Breed a " + currentCollourChallenge + " coloured creature with a  " + currentTailChallenge + " tail";
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
