using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuiz", menuName = "Quiz/Quiz Options")]
public class QuizOptions : ScriptableObject
{
    //A string used to track the level type this quiz belongs to (e.g., "Level1", "Breeding")
    public string LevelType;

    //A list of questions to be used in this quiz
    public List<QuizQuestion> questions;
}