using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class LevelOneChallenge : Challenge
{ 
    //Sets the list of challenges specific to Level One
    protected override void SetListItem()
    {
        challenges.Add("Green");
        challenges.Add("Yellow");
    }

    //Overload for submitting a single phenotype as a string
    public void SetResult(string phenotype, Creature creature)
    {
        SetResult(new List<string> { phenotype }, creature);
    }

    //Shows visual cues (turtle images) based on the current challenge
    protected override void ShowVisualCue()
    {
        if (string.IsNullOrEmpty(currentChallenge))
        {
            Debug.LogWarning("[LevelOneChallenge] currentChallenge is null or empty.");
            return;
        }

        try
        {
            string challenge = currentChallenge.ToLower();

            if (TurtleOne != null)
                TurtleOne.gameObject.SetActive(challenge == "green");

            if (TurtleTwo != null)
                TurtleTwo.gameObject.SetActive(challenge == "yellow");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[LevelOneChallenge] Error in ShowVisualCue: {ex.Message}");
        }
    }

    //Picks and displays the next challenge in the list
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
