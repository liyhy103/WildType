using System.Diagnostics;
using System.Runtime.InteropServices.ComTypes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    public GameObject quizPanel; // The panel that contains the quiz UI elements
    public TMP_Text questionText; // The text component that displays the question
    public Button[] answerButtons; // The buttons for the answers
    public TMP_Text resultText; // The text component that displays the result

    public QuizOptions quizOptions; // The quiz options containing questions and answers

    private int currentQuestionIndex = 0; // The index of the current question
    private int correctAnswersCount = 0; // The count of correct answers

    public bool IsQuizActive => quizPanel.activeSelf; // Property to check if the quiz is active
    public delegate void QuizFinishedHandler();

    public event QuizFinishedHandler OnQuizFinished; // Event to notify when the quiz is finished

    void Start()
    {
        // Hide the quiz panel at the start
        quizPanel.SetActive(false);
    }

    public void StartQuiz(QuizOptions data)
    {
        //quizOptions = data;

        if (quizOptions == null)
        {
            UnityEngine.Debug.LogError("QuizOptions not assigned!");
            return;
        }

        currentQuestionIndex = 0;
        correctAnswersCount = 0;
        quizPanel.SetActive(true);
        ShowQuestion();
    }

    void ShowQuestion()
    {
        QuizQuestion q = quizOptions.questions[currentQuestionIndex]; // Get the current question
        questionText.text = q.question; // Set the question text

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].GetComponentInChildren<TMP_Text>().text = q.answers[i];
            int index = i; // Capture the current index for the button click event
            answerButtons[i].onClick.RemoveAllListeners(); // Remove previous listeners
            answerButtons[i].onClick.AddListener(() => CheckAnswer(index)); // Add a new listener for the button click
        }
    }

    void CheckAnswer(int index)
    {
        var question = quizOptions.questions[currentQuestionIndex]; // Get the current question
        if (index == question.correctAnswerIndex)
        {
            correctAnswersCount++;
            resultText.text = (correctAnswersCount + "/3 correct"); // Show correct answer message
        }
        else
        {
            resultText.text = (correctAnswersCount + "/3 correct"); // Show wrong answer message
        }

            currentQuestionIndex++; // Move to the next question

            if (currentQuestionIndex != quizOptions.questions.Count)
            {
                Invoke(nameof(ShowQuestion), 1f); // Show the next question after a delay
             }
            else
            {
                Invoke(nameof(EndQuiz), 1f); // End the quiz after all questions are answered
             }
        }
    void EndQuiz()
    {
        quizPanel.SetActive(false); // Hide the quiz panel
        resultText.text = "Quiz Finished! You got " + correctAnswersCount + " out of " + quizOptions.questions.Count + " correct."; // Show final result
        OnQuizFinished?.Invoke();
    }
}

