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

    private List<string> shellChallenges = new();
    private List<string> genderChallenges = new();

    private string currentShellChallenge = "";
    private string currentGenderChallenge = "";

    public string CurrentShellChallenge => currentShellChallenge;
    public string CurrentGenderChallenge => currentGenderChallenge;

    protected override void SetListItem()
    {
        shellChallenges.AddRange(new[] { "Light", "Light", "Dark", "Dark" });
        genderChallenges.AddRange(new[] { "Female", "Male", "Female", "Male" });
    }

    protected override void PickNextChallenge()
    {
        if (shellChallenges.Count > 0 && genderChallenges.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, shellChallenges.Count);
            currentShellChallenge = shellChallenges[index];
            currentGenderChallenge = genderChallenges[index];

            expectedTraits = new List<string> { currentShellChallenge, currentGenderChallenge };

            currentChallenge = $"Breed a {currentShellChallenge} shelled {currentGenderChallenge} creature";
            SetChallengeText(currentChallenge);
        }
        else
        {
            SetChallengeText("All challenges completed!");
            currentChallenge = "";
        }
    }

    public void SetResult(string shellPhenotype, string gender, Creature creature)
    {
        List<string> traits = new() { shellPhenotype, gender };
        SetResult(traits, creature);
    }

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
    }

}
