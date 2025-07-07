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
        if (currentChallenge.ToLower() == "green" +
            "")
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
}
