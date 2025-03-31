using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;


public class Challenge : MonoBehaviour
{
    public TMP_Text challengeText;
    private List<string> challenges = new List<string>();

    public void Start()
    {
        SetListItem();
        challengeText.gameObject.SetActive(false);
    }

    private void SetListItem()
    {
        // Add challenges to the list
        challenges.Add("Blue");
        challenges.Add("Red");

    }

    public void SetChallengeText(string text)
    {
        challengeText.text = text;
    }

    public void OnButtonClick()
    {
        //assign a random challenge to the player
        string challenge = "Breed a " + (challenges[Random.Range(0, challenges.Count)] + "creature");
        SetChallengeText(challenge);

        //displays the challenge
        challengeText.gameObject.SetActive(true);
    }
}
