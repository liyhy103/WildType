using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

using Debug = UnityEngine.Debug;


public abstract class Challenge : MonoBehaviour
{

    public TMP_Text challengeText;
    public ParticleSystem particleSystem;
    public Image TurtleOne, TurtleTwo, TurtleThree, TurtleFour;

    protected List<string> challenges = new();
    protected string currentChallenge = "";
    protected List<string> expectedTraits = new();
    protected List<string> submittedTraits = new();
    public Creature currentCreature;

    public string CurrentChallenge => currentChallenge;

    public void Start()
    {
        ValidateReferences();
        SetListItem();
        HideAllVisuals();
        PickNextChallenge();
    }

    protected virtual void SetListItem() { }

    protected virtual void ValidateReferences()
    {
        if (challengeText == null)
            Debug.LogError("[Challenge] challengeText not assigned!");

        if (particleSystem == null)
            Debug.LogWarning("[Challenge] particleSystem not assigned!");

        if (TurtleOne == null || TurtleTwo == null)
            Debug.LogWarning("[Challenge] One or more turtle images not assigned!");
    }

    protected virtual void HideAllVisuals()
    {
        TurtleOne?.gameObject.SetActive(false);
        TurtleTwo?.gameObject.SetActive(false);
        TurtleThree?.gameObject.SetActive(false);
        TurtleFour?.gameObject.SetActive(false);
    }

    public virtual void SetChallengeText(string text)
    {
        challengeText.text = text;
        UnityEngine.Debug.Log("[Challenge] Showing challenge text: " + text);
    }

    public virtual void SetResult(List<string> traits, Creature creature)
    {
        submittedTraits = traits;
        currentCreature = creature;
        Debug.Log($"[Challenge] Received result: [{string.Join(", ", traits)}]");

        if (string.IsNullOrEmpty(currentChallenge))
        {
            Debug.Log("[Challenge] No active challenge — assuming all are complete.");
            return;
        }

        Debug.Log($"[Challenge] Comparing result '{string.Join(", ", submittedTraits)}' with current challenge '{currentChallenge}'");

        ProcessResult();
    }

    protected virtual void ProcessResult()
    {
        if (currentCreature == null)
        {
            Debug.LogWarning("No creature has been bred yet!");
            return;
        }

        bool match = expectedTraits.Count == submittedTraits.Count &&
                     expectedTraits.Zip(submittedTraits, (a, b) => a.ToLower() == b.ToLower()).All(x => x);

        if (match)
        {
            SetChallengeText("Challenge complete!");
            TriggerSuccessEffect();
            RemoveCurrentChallenge();
            PickNextChallenge();
        }

        currentCreature = null;
        submittedTraits.Clear();
    }

    protected virtual void TriggerSuccessEffect()
    {
        if (particleSystem != null)
            particleSystem.Play();
    }

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
    protected virtual void RemoveCurrentChallenge()
    {
        challenges.Remove(currentChallenge);
    }
    protected virtual void ShowVisualCue() { }
}
