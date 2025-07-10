using System.Diagnostics;
using System.Runtime.InteropServices.ComTypes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;


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

    public ParticleSystem particleSystem;// Particle system to play effects when the question is answered correctly

    public AudioSource cheerAudio;

    public AudioSource IncorrectAudio;

    private bool canClickAnswers = false;

    void Start()
    {
        // Hide the quiz panel at the start
        quizPanel.SetActive(false);
    }

    public void StartQuiz(QuizOptions data)
    {
        quizOptions = data;

        if (quizOptions == null || quizOptions.questions == null || quizOptions.questions.Count == 0)
        {
            UnityEngine.Debug.LogError("QuizOptions or questions list is not properly assigned!");
            return;
        }

        currentQuestionIndex = 0;
        correctAnswersCount = 0;
        quizPanel.SetActive(true);
        ShowQuestion();
    }

    void ShowQuestion()
    {
        if (currentQuestionIndex < 0 || currentQuestionIndex >= quizOptions.questions.Count)
        {
            Debug.LogError($"Invalid question index: {currentQuestionIndex}");
            EndQuiz();
            return;
        }

        QuizQuestion q = quizOptions.questions[currentQuestionIndex];

        if (q.answers == null || q.answers.Length == 0)
        {
            Debug.LogError($"Question {currentQuestionIndex} has no answers.");
            EndQuiz();
            return;
        }

        if (q.correctAnswerIndex < 0 || q.correctAnswerIndex >= q.answers.Length)
        {
            Debug.LogError($"Question {currentQuestionIndex} has an invalid correctAnswerIndex.");
            EndQuiz();
            return;
        }

        questionText.text = q.question;

        int optionsToShow = Mathf.Min(answerButtons.Length, q.answers.Length);
        for (int i = 0; i < optionsToShow; i++)
        {
            answerButtons[i].interactable = true;
            answerButtons[i].gameObject.SetActive(true); // Ensure the button is visible
            answerButtons[i].GetComponentInChildren<TMP_Text>().text = q.answers[i];
            int index = i;
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => CheckAnswer(index));
        }
        
        canClickAnswers = true;

        // Hide unused buttons
        for (int i = q.answers.Length; i < answerButtons.Length; i++)
        {
            answerButtons[i].gameObject.SetActive(false);
        }
    }

    void CheckAnswer(int index)
    {
        if (!canClickAnswers)
        {
            return;
        }

        canClickAnswers = false;
        foreach (var button in answerButtons)
        {
            button.interactable = false;
        }
        var question = quizOptions.questions[currentQuestionIndex]; // Get the current question
        if (index == question.correctAnswerIndex)
        {
            correctAnswersCount++;
            particleSystem.Play();
            resultText.text = (correctAnswersCount + "/3 correct");
            cheerAudio.Play();
        }
        else
        {
            resultText.text = (correctAnswersCount + "/3 correct");
            IncorrectAudio.Play();
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

