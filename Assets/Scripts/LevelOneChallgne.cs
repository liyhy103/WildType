using System.Collections.Generic;
using UnityEngine;

public class LevelOneChallenge : Challenge
{
    protected override void SetListItem()
    {
        challenges.Add("Green");
        challenges.Add("Yellow");

    }

    public void SetResult(string phenotype, Creature creature)
    {
        SetResult(new List<string> { phenotype }, creature);
    }

    protected override void ShowVisualCue()
    {
        if (currentChallenge.ToLower() == "green")
        {
            TurtleOne?.gameObject.SetActive(true);
            TurtleTwo?.gameObject.SetActive(false);
        }
        else if (currentChallenge.ToLower() == "yellow")
        {
            TurtleOne?.gameObject.SetActive(false);
            TurtleTwo?.gameObject.SetActive(true);
        }

    }

    protected override void PickNextChallenge()
    {
        if (challenges.Count > 0)
        {
            currentChallenge = challenges[0];
            expectedTraits = new List<string> { currentChallenge }; 

            SetChallengeText($"Breed a {currentChallenge} creature");
            ShowVisualCue();
        }
        else
        {
            SetChallengeText("All challenges completed!");
            currentChallenge = "";
        }
    }
}
