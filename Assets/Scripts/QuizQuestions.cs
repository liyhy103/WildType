using UnityEngine;

[System.Serializable]

public class QuizQuestion
{
    //The question text
    public string question;
    //The possible answers
    public string[] answers;
    //The index of the correct answer in the answers array
    public int correctAnswerIndex;
}
