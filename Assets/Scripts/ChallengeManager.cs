using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using System.Diagnostics;
using System;


public class Challenge : MonoBehaviour
{

    public TMP_Text challengeText;
    private List<string> challenges = new List<string>();
    public Creature currentCreature;
    private string currentChallenge = "";
    private string result = "";

    public void Start()
    {
        if (challengeText == null)
        {
            UnityEngine.Debug.LogError("challengeText is not assigned in the Inspector!");
            return;
        }
        challengeText.text = "";
        challengeText.gameObject.SetActive(false);


    }
    void Awake()
    {
        SetListItem(); // Populate the list early
    }
    private void SetListItem()
    {
        // Add challenges to the list
        challenges.Add("Green");
        challenges.Add("Yellow");
        challenges.Add("Pink");
        challenges.Add("Blue");

    }


    public void OnButtonClick()
    {


        //sets currentChallenge to be first item in the list
        currentChallenge = challenges[0].ToLower();

        challengeText.text = "Breed a " + challenges[0].ToLower() + " Creature!";
        challengeText.gameObject.SetActive(true);

    }




    public string setResult(string result)
    {
        this.result = result.ToLower();

        currentChallenge = challenges[0].ToLower();

        if (currentChallenge == result)
        {
            challengeText.text = "Completed Challenge! \nClick again to get a new challenge!";
            challengeText.gameObject.SetActive(true);
            // Remove the completed challenge
            challenges.RemoveAt(0);

        }

        if(result == "yellow" && currentChallenge != "green")
        {
            challengeText.text = "Congratulations! \nYou completed level one!";
            challengeText.gameObject.SetActive(true);

        }
        challengeText.gameObject.SetActive(true);
        UnityEngine.Debug.Log("Challenge text set to: " + result);

        return result;
    }

    private void CheckChallengeCompleted()
    {


    }
}
