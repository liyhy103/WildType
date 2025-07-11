using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using Debug = UnityEngine.Debug;


public abstract class Challenge : MonoBehaviour
{

    //Text component that shows the current challenge
    public TMP_Text challengeText;
    //Particle system played when a challenge is completed
    public ParticleSystem particleSystem;
    //Turtle images used for visual cues
    public UnityEngine.UI.Image TurtleOne, TurtleTwo, TurtleThree, TurtleFour;
    //Sound effect played when the player completes a challenge
    public AudioSource cheerAudio;

    //List of all available challenge strings (e.g., "Green Female")
    protected List<string> challenges = new();
    //The current active challenge string
    protected string currentChallenge = "";
    //The traits expected from the player’s submission
    protected List<string> expectedTraits = new();
    //The traits the player submitted
    protected List<string> submittedTraits = new();
    //The creature bred by the player
    public Creature currentCreature;

    //Public property to access the current challenge
    public string CurrentChallenge => currentChallenge;

    //Method that runs when this script starts
    public void Start()
    {
        //Check required components are assigned
        ValidateReferences(); 
        //Hook for child class customization
        SetListItem();
        //Turn off turtle hints
        HideAllVisuals();
        //Start the first challenge
        PickNextChallenge(); 
    }

    //Hook for subclasses to override setup logic
    protected virtual void SetListItem() { }

    //Checks all public references and logs warnings or errors
    protected virtual void ValidateReferences()
    {
        if (challengeText == null)
            Debug.LogError("[Challenge] challengeText not assigned!");
        if (particleSystem == null)
            Debug.LogWarning("[Challenge] particleSystem not assigned!");
        if (TurtleOne == null || TurtleTwo == null || TurtleThree == null || TurtleFour == null)
            Debug.LogWarning("[Challenge] One or more turtle images not assigned!");
        if (cheerAudio == null)
            Debug.LogWarning("[Challenge] cheerAudio not assigned!");
    }

    //Turns off visibility of all turtle hint images
    protected virtual void HideAllVisuals()
    {
        TurtleOne?.gameObject.SetActive(false);
        TurtleTwo?.gameObject.SetActive(false);
        TurtleThree?.gameObject.SetActive(false);
        TurtleFour?.gameObject.SetActive(false);
    }

    //Displays a message in the UI challengeText field
    public virtual void SetChallengeText(string text)
    {
        if (challengeText != null)
        {
            challengeText.text = text;
            Debug.Log("[Challenge] Showing challenge text: " + text);
        }
        else
        {
            Debug.LogWarning("[Challenge] challengeText is null. Cannot display challenge.");
        }
    }

    //Called when the player submits a creature and its traits
    public virtual void SetResult(List<string> traits, Creature creature)
    {
        submittedTraits = traits ?? new List<string>();
        currentCreature = creature;
        Debug.Log($"[Challenge] Received result: [{string.Join(", ", submittedTraits)}]");

        if (string.IsNullOrEmpty(currentChallenge))
        {
            Debug.Log("[Challenge] No active challenge — assuming all are complete.");
            return;
        }

        Debug.Log($"[Challenge] Comparing result '{string.Join(", ", submittedTraits)}' with current challenge '{currentChallenge}'");

        Debug.Log($"[Challenge] ExpectedTraits: [{string.Join(", ", expectedTraits)}]");
        Debug.Log($"[Challenge] SubmittedTraits: [{string.Join(", ", submittedTraits)}]");
        //Check if traits match the challenge
        ProcessResult(); 
    }

    //Compares submittedTraits with expectedTraits and triggers success if they match
    protected virtual void ProcessResult()
    {
        if (currentCreature == null)
        {
            Debug.LogWarning("[Challenge] No creature has been bred yet!");
            return;
        }

        try
        {
            Debug.Log($"[Challenge] inside process ExpectedTraits: [{string.Join(", ", expectedTraits)}]");
            Debug.Log($"[Challenge] inside process SubmittedTraits: [{string.Join(", ", submittedTraits)}]");

            bool match = expectedTraits.Count == submittedTraits.Count &&
            expectedTraits.Zip(submittedTraits, (a, b) =>
                string.Equals(a?.Trim(), b?.Trim(), StringComparison.OrdinalIgnoreCase))
            .All(equal => equal);

            if (match)
            {
                //Display success message
                SetChallengeText("Challenge complete!");
                //Trigger visual success effect
                TriggerSuccessEffect();
                //Play cheering sound
                cheerAudio?.Play();
                //Remove completed challenge from list
                RemoveCurrentChallenge();
                //Pick the next challenge
                PickNextChallenge();
            }
            else
            {
                //Log a message if the submitted traits did not match
                Debug.Log("[Challenge] Traits did not match. Challenge not complete.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Challenge] Error in ProcessResult: {ex.Message}");
        }

        currentCreature = null;
        submittedTraits.Clear();
    }

    //Plays particle effect when challenge is completed
    protected virtual void TriggerSuccessEffect()
    {
        if (particleSystem != null)
            particleSystem.Play();
    }

    //Selects the next challenge from the list and updates the UI
    protected virtual void PickNextChallenge()
    {
        if (challenges.Count > 0)
        {
            currentChallenge = challenges[0];
            SetChallengeText($"Breed a {currentChallenge} creature");
            ShowVisualCue(); 
        }
        else
        {
            SetChallengeText("All challenges completed!");
            currentChallenge = "";
        }
    }

    //Removes the currently completed challenge from the list
    protected virtual void RemoveCurrentChallenge()
    {
        challenges.Remove(currentChallenge);
    }

    //Child class to visually highlight what to breed
    protected virtual void ShowVisualCue() { }
}