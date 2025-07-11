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
    //List of shell color challenges (Light or Dark)
    private List<string> shellChallenges = new();
    //List of gender challenges (Male or Female)
    private List<string> genderChallenges = new();

    //Current shell color to be matched
    private string currentShellChallenge = "";
    //Current gender to be matched
    private string currentGenderChallenge = "";

    //Public getters for external access
    public string CurrentShellChallenge => currentShellChallenge;
    public string CurrentGenderChallenge => currentGenderChallenge;

    //Method to initialize the challenge lists
    protected override void SetListItem()
    {
        shellChallenges.AddRange(new[] { "Light", "Light", "Dark", "Dark" });
        genderChallenges.AddRange(new[] { "Female", "Male", "Female", "Male" });
    }

    //Picks a new shell + gender combination as the active challenge
    protected override void PickNextChallenge()
    {
        if (shellChallenges.Count > 0 && genderChallenges.Count > 0)
        {
            try
            {
                int index = UnityEngine.Random.Range(0, shellChallenges.Count);

                //Assign the current challenge pair
                currentShellChallenge = shellChallenges[index];
                currentGenderChallenge = genderChallenges[index];

                //Set expected traits used for validation
                expectedTraits = new List<string> { currentShellChallenge, currentGenderChallenge };

                //Update UI text
                currentChallenge = $"Breed a {currentShellChallenge} shelled {currentGenderChallenge} creature";
                SetChallengeText(currentChallenge);

                //Trigger turtle visual hint
                ShowVisualCue();
            }
            catch (Exception ex)
            {
                Debug.LogError($"[LevelTwoChallenge] Exception in PickNextChallenge: {ex.Message}");
                SetChallengeText("Error setting new challenge.");
            }
        }
        else
        {
            SetChallengeText("All challenges completed!");
            currentChallenge = "";
        }
    }

    //Overload to support string-based result input
    public void SetResult(string shellPhenotype, string gender, Creature creature)
    {
        List<string> traits = new() { shellPhenotype, gender };
        SetResult(traits, creature);
    }

    //Removes the current shell-gender pair from the challenge pool
    protected override void RemoveCurrentChallenge()
    {
        int index = -1;
        for (int i = 0; i < shellChallenges.Count; i++)
        {
            if (shellChallenges[i] == currentShellChallenge &&
                genderChallenges[i] == currentGenderChallenge)
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
        else
        {
            Debug.LogWarning("[LevelTwoChallenge] Tried to remove challenge that doesn't exist in list.");
        }
    }

    //Displays visual cue (Turtle image) based on challenge type
    protected override void ShowVisualCue()
    {
        //Hide all turtles first
        TurtleOne?.gameObject.SetActive(false);
        TurtleTwo?.gameObject.SetActive(false);
        TurtleThree?.gameObject.SetActive(false);
        TurtleFour?.gameObject.SetActive(false);

        //Get current shell and gender challenge values
        string shell = currentShellChallenge;
        string gender = currentGenderChallenge;

        Debug.Log($"[LevelTwoChallenge] Visual cue: shell='{shell}' gender='{gender}'");

        try
        {
            if (shell == "Light" && gender == "Female")
            {
                TurtleOne?.gameObject.SetActive(true);
                Debug.Log("[LevelTwoChallenge] Showing TurtleOne");
            }
            else if (shell == "Light" && gender == "Male")
            {
                TurtleTwo?.gameObject.SetActive(true);
                Debug.Log("[LevelTwoChallenge] Showing TurtleTwo");
            }
            else if (shell == "Dark" && gender == "Female")
            {
                TurtleThree?.gameObject.SetActive(true);
                Debug.Log("[LevelTwoChallenge] Showing TurtleThree");
            }
            else if (shell == "Dark" && gender == "Male")
            {
                TurtleFour?.gameObject.SetActive(true);
                Debug.Log("[LevelTwoChallenge] Showing TurtleFour");
            }
            else
            {
                Debug.LogWarning($"[LevelTwoChallenge] No matching turtle for shell='{shell}' and gender='{gender}'");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"[LevelTwoChallenge] Error in ShowVisualCue: {ex.Message}");
        }
    }
}
