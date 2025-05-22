using UnityEngine;

[System.Serializable]

public class QuizQuestion
{
    public string question; // The question text
    public string[] answers; // The possible answers
    public int correctAnswerIndex; // The index of the correct answer in the answers array
}
