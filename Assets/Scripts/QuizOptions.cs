using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuiz", menuName = "Quiz/Quiz Options")]
public class QuizOptions : ScriptableObject
{
    public string LevelType;
    public List<QuizQuestion> questions; // Corrected type name + property name
}