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
    //Stores the available colour and tail challenges
    private List<string> colourChallenges = new List<string>();
    private List<string> tailChallenges = new List<string>();

    //Current challenge values
    private string currentColourChallenge = "";
    private string currentTailChallenge = "";

    //Public accessors for external scripts
    public string CurrentColourChallenge => currentColourChallenge;
    public string CurrentTailChallenge => currentTailChallenge;

    //Method to populate the initial list of challenges
    protected override void SetListItem()
    {
        colourChallenges.AddRange(new[] { "Green", "Green", "Yellow", "Yellow" });
        tailChallenges.AddRange(new[] { "Long", "Short", "Long", "Short" });
    }

    //Method to select and set the next challenge
    protected override void PickNextChallenge()
    {
        if (colourChallenges.Count > 0 && tailChallenges.Count > 0)
        {
            try
            {
                int index = UnityEngine.Random.Range(0, colourChallenges.Count);
                currentColourChallenge = colourChallenges[index];
                currentTailChallenge = tailChallenges[index];

                expectedTraits = new List<string> { currentColourChallenge, currentTailChallenge };

                currentChallenge = $"Breed a {currentColourChallenge} coloured creature with a {currentTailChallenge} tail";
                SetChallengeText(currentChallenge);
                Debug.Log("[LevelFourChallenge] CurrentChallenge set to: " + currentChallenge);

                ShowVisualCue();
            }
            catch (Exception ex)
            {
                Debug.LogError("[LevelFourChallenge] Error in PickNextChallenge: " + ex.Message);
                SetChallengeText("Error setting challenge.");
            }
        }
        else
        {
            SetChallengeText("All challenges have been completed!");
            currentChallenge = "";
        }
    }

    //Method to validate the player's result with two phenotype inputs
    public void SetResult(string colour, string tail, Creature creature)
    {
        List<string> traits = new() { colour, tail };
        SetResult(traits, creature);
    }

    //Method to remove the completed challenge combination
    protected override void RemoveCurrentChallenge()
    {
        int index = -1;
        for (int i = 0; i < colourChallenges.Count; i++)
        {
            if (colourChallenges[i] == currentColourChallenge && tailChallenges[i] == currentTailChallenge)
            {
                index = i;
                break;
            }
        }

        if (index >= 0)
        {
            colourChallenges.RemoveAt(index);
            tailChallenges.RemoveAt(index);
        }
        else
        {
            Debug.LogWarning("[LevelFourChallenge] Tried to remove a challenge pair that was not found.");
        }
    }

    //Method to show the correct turtle based on current colour and tail challenge
    protected override void ShowVisualCue()
    {
        try
        {
            //Hide all turtles
            TurtleOne?.gameObject.SetActive(false);
            TurtleTwo?.gameObject.SetActive(false);
            TurtleThree?.gameObject.SetActive(false);
            TurtleFour?.gameObject.SetActive(false);

            string colour = currentColourChallenge;
            string tail = currentTailChallenge;

            if (colour == "Green" && tail == "Long")
            {
                TurtleOne?.gameObject.SetActive(true);
            }
            else if (colour == "Green" && tail == "Short")
            {
                TurtleTwo?.gameObject.SetActive(true);
            }
            else if (colour == "Yellow" && tail == "Long")
            {
                TurtleThree?.gameObject.SetActive(true);
            }
            else if (colour == "Yellow" && tail == "Short")
            {
                TurtleFour?.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning($"[LevelFourChallenge] No visual match for: colour={colour}, tail={tail}");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("[LevelFourChallenge] Error in ShowVisualCue: " + ex.Message);
        }
    }
}