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
    //Method to define the list of horn-related challenges
    protected override void SetListItem()
    {
        challenges.Add("Long");
        challenges.Add("Short");
        challenges.Add("No");
    }

    //Method to submit a phenotype string and creature for result evaluation
    public void SetResult(string phenotype, Creature creature)
    {
        SetResult(new List<string> { phenotype }, creature);
    }

    //Method to display the correct turtle visual based on current challenge
    protected override void ShowVisualCue()
    {
        try
        {
            string challenge = currentChallenge;

            //Hide all turtle visuals first
            TurtleOne?.gameObject.SetActive(false);
            TurtleTwo?.gameObject.SetActive(false);
            TurtleThree?.gameObject.SetActive(false);

            if (string.IsNullOrEmpty(challenge))
            {
                Debug.LogWarning("[LevelThreeChallenge] currentChallenge is null or empty.");
                return;
            }

            if (challenge == "Long")
            {
                TurtleOne?.gameObject.SetActive(true);
                Debug.Log("[LevelThreeChallenge] Showing TurtleOne (Long Horn)");
            }
            else if (challenge == "Short")
            {
                TurtleTwo?.gameObject.SetActive(true);
                Debug.Log("[LevelThreeChallenge] Showing TurtleTwo (Short Horn)");
            }
            else if (challenge == "No")
            {
                TurtleThree?.gameObject.SetActive(true);
                Debug.Log("[LevelThreeChallenge] Showing TurtleThree (No Horn)");
            }
            else
            {
                Debug.LogWarning($"[LevelThreeChallenge] No matching visual for challenge: {challenge}");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"[LevelThreeChallenge] Error in ShowVisualCue: {ex.Message}");
        }
    }

    //Method to select and display the next challenge
    protected override void PickNextChallenge()
    {
        if (challenges.Count > 0)
        {
            try
            {
                currentChallenge = challenges[0];
                expectedTraits = new List<string> { currentChallenge };

                SetChallengeText($"Breed a {currentChallenge} horned creature");
                ShowVisualCue();
            }
            catch (Exception ex)
            {
                Debug.LogError($"[LevelThreeChallenge] Error in PickNextChallenge: {ex.Message}");
                SetChallengeText("Error loading challenge.");
            }
        }
        else
        {
            SetChallengeText("All challenges completed!");
            currentChallenge = "";
        }
    }
}