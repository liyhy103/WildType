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
    private List<string> colourChallenges = new List<string>();
    private List<string> tailChallenges = new List<string>();

    private string currentColourChallenge = "";
    private string currentTailChallenge = "";

    public string CurrentColourChallenge => currentColourChallenge;
    public string CurrentTailChallenge => currentTailChallenge;

    protected override void SetListItem()
    {
        colourChallenges.AddRange(new[] { "Green", "Green", "Yellow", "Yellow" });
        tailChallenges.AddRange(new[] { "Long", "Short", "Long", "Short" });
    }

    protected override void PickNextChallenge()
    {
        if (colourChallenges.Count > 0 && tailChallenges.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, colourChallenges.Count);
            currentColourChallenge = colourChallenges[index];
            currentTailChallenge = tailChallenges[index];

            expectedTraits = new List<string> { currentColourChallenge, currentTailChallenge };

            currentChallenge = $"Breed a {currentColourChallenge} coloured creature with a {currentTailChallenge} tail";
            SetChallengeText(currentChallenge);
            Debug.Log("[Challenge] CurrentChallenge set to: " + currentChallenge);
        }
        else
        {
            SetChallengeText("All challenges have been completed!");
            currentChallenge = "";
        }
    }
    public void SetResult(string colour, string tail, Creature creature)
    {
        List<string> traits = new() { colour, tail };
        SetResult(traits, creature);
    }

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
    }
}
