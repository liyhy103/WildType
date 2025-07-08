using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public QuizManager quizManager;
    public QuizOptions QuizToShow;
    public string nextLevelName; // The name of the next level scene

    private bool quizCompleted = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        quizManager.quizPanel.SetActive(false); // Hide the quiz panel initially
        quizManager.OnQuizFinished += LoadNextLevel;
    }

    public void playerReady()
    {
        UnityEngine.Debug.Log("[LevelThreeChallenges] NextLevel button clicked.");
        quizManager.StartQuiz(QuizToShow); // Start the quiz with the specified options
       
    }

    
    void LoadNextLevel()
    {
        // Load the next level scene
        SceneManager.LoadScene(nextLevelName);
    }

}
