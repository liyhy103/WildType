using UnityEngine;

[System.Serializable]
public class TutorialStep
{
    public string stepName;
    public string guideText;
    public RectTransform targetUI;
    public string targetUIName;
    public float autoAdvanceAfterSeconds;
}