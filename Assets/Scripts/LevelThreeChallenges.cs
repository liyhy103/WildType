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
    protected override void SetListItem()
    {
        challenges.Add("Long");
        challenges.Add("Short");
        challenges.Add("No");
    }
    public void SetResult(string phenotype, Creature creature)
    {
        SetResult(new List<string> { phenotype }, creature);
    }

    protected override void ShowVisualCue()
    {
        if (currentChallenge.ToLower() == "long")
        {
            TurtleOne?.gameObject.SetActive(true);
            TurtleTwo?.gameObject.SetActive(false);
            TurtleThree?.gameObject.SetActive(false);
        }
        else if (currentChallenge.ToLower() == "short")
        {
            TurtleOne?.gameObject.SetActive(false);
            TurtleTwo?.gameObject.SetActive(true);
            TurtleThree?.gameObject.SetActive(false);
        }
        else
        {
            TurtleOne?.gameObject.SetActive(false);
            TurtleTwo?.gameObject.SetActive(false);
            TurtleThree?.gameObject.SetActive(true);
        }
    }

    protected override void PickNextChallenge()
    {
        if (challenges.Count > 0)
        {
            currentChallenge = challenges[0];
            expectedTraits = new List<string> { currentChallenge }; 

            SetChallengeText($"Breed a {currentChallenge} horned creature");
            ShowVisualCue();
        }
        else
        {
            SetChallengeText("All challenges completed!");
            currentChallenge = "";
        }
    }
}