using UnityEngine;

public class LevelOneChallenge : Challenge
{
    protected override void SetListItem()
    {
        challenges.Add("Green");
        challenges.Add("Yellow");

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
