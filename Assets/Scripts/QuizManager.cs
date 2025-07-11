using System.Diagnostics;
using System.Runtime.InteropServices.ComTypes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;


public class QuizManager : MonoBehaviour
{
    //The panel that contains the quiz UI elements
    public GameObject quizPanel;
    //The text component that displays the question
    public TMP_Text questionText;
    //The buttons for the answers
    public Button[] answerButtons;
    //The text component that displays the result
    public TMP_Text resultText;
    //The quiz options containing questions and answers
    public QuizOptions quizOptions;
    //The index of the current question
    private int currentQuestionIndex = 0;
    //The count of correct answers
    private int correctAnswersCount = 0;
    //Property to check if the quiz is active#

    public bool IsQuizActive => quizPanel.activeSelf;

    public delegate void QuizFinishedHandler();
    //Event to notify when the quiz is finished
    public event QuizFinishedHandler OnQuizFinished;
    //Particle system to play effects when the question is answered correctly
    public ParticleSystem particleSystem;
    //Audio system that plays noise when answer is correct
    public AudioSource cheerAudio;
    //Audio system that plays when player answers a question incorrect 
    public AudioSource IncorrectAudio;
    //Boolean that shows says if players can check answers 
    private bool canClickAnswers = false;

    void Start()
    {
        //Checks to make sure nothing is null before the game starts
        if (quizPanel == null) Debug.LogError("[QuizManager] quizPanel is not assigned.");
        if (questionText == null) Debug.LogError("[QuizManager] questionText is not assigned.");
        if (resultText == null) Debug.LogError("[QuizManager] resultText is not assigned.");
        if (answerButtons == null || answerButtons.Length == 0) Debug.LogError("[QuizManager] answerButtons not set.");
        if (particleSystem == null) Debug.LogWarning("[QuizManager] particleSystem not assigned.");
        if (cheerAudio == null) Debug.LogWarning("[QuizManager] cheerAudio not assigned.");
        if (IncorrectAudio == null) Debug.LogWarning("[QuizManager] IncorrectAudio not assigned.");

        //Hide the quiz panel at the start
        quizPanel.SetActive(false);
    }

    public void StartQuiz(QuizOptions data)
    {
        //Sets quizOptions to match the quiz options (data) being sent in 
        quizOptions = data;

        //Checks that quiz data exists before starting
        if (quizOptions == null || quizOptions.questions == null || quizOptions.questions.Count == 0)
        {
            UnityEngine.Debug.LogError("QuizOptions or questions list is not properly assigned!");
            return;
        }

        //sets all varibables to be 0 before being set properly
        currentQuestionIndex = 0;
        correctAnswersCount = 0;

        //Sets quiz to be active 
        quizPanel.SetActive(true);

        //Show the questions 
        ShowQuestion();
    }

    void ShowQuestion()
    {
        try
        {
            //Checks that quiz data is still valid
            if (quizOptions == null || quizOptions.questions == null)
            {
                Debug.LogError("[QuizManager] quizOptions or its questions list is null in ShowQuestion.");
                EndQuiz();
                return;
            }

            //Prevents index out of bounds errors
            if (currentQuestionIndex < 0 || currentQuestionIndex >= quizOptions.questions.Count)
            {
                Debug.LogError($"[QuizManager] Invalid question index: {currentQuestionIndex}");
                EndQuiz();
                return;
            }

            //Gets the current question
            QuizQuestion q = quizOptions.questions[currentQuestionIndex];

            //Checks if the question has valid answer options
            if (q.answers == null || q.answers.Length == 0)
            {
                Debug.LogError($"[QuizManager] Question {currentQuestionIndex} has no answers.");
                EndQuiz();
                return;
            }

            //Checks if the correct answer index is valid
            if (q.correctAnswerIndex < 0 || q.correctAnswerIndex >= q.answers.Length)
            {
                Debug.LogError($"[QuizManager] Question {currentQuestionIndex} has an invalid correctAnswerIndex.");
                EndQuiz();
                return;
            }

            //Displays the question text
            if (questionText != null)
            {
                questionText.text = q.question;
            }

            //Sets up the answer buttons
            int optionsToShow = Mathf.Min(answerButtons.Length, q.answers.Length);
            for (int i = 0; i < optionsToShow; i++)
            {
                Button button = answerButtons[i];
                if (button == null)
                {
                    Debug.LogWarning($"[QuizManager] answerButtons[{i}] is null.");
                    continue;
                }


                //Activates and sets text for each answer button
                button.interactable = true;
                button.gameObject.SetActive(true);

                TMP_Text btnText = button.GetComponentInChildren<TMP_Text>();
                if (btnText != null)
                {
                    btnText.text = q.answers[i];
                }
                else
                {
                    Debug.LogWarning($"[QuizManager] Button {i} missing TMP_Text.");
                }

                //Sets up the click event to check the answer
                int index = i;
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => CheckAnswer(index));
            }

            // Hide unused buttons
            for (int i = q.answers.Length; i < answerButtons.Length; i++)
            {
                if (answerButtons[i] != null)
                    answerButtons[i].gameObject.SetActive(false);
            }

            //Allows answers to be clicked
            canClickAnswers = true;
        }
        catch (System.Exception ex)
        {
            //Logs any unexpected error and ends quiz to prevent crash
            Debug.LogError($"[QuizManager] Exception in ShowQuestion: {ex.Message}");
            EndQuiz();
        }
    }

    void CheckAnswer(int index)
    {
        //Prevents answering if quiz is not ready
        if (!canClickAnswers || quizOptions == null || quizOptions.questions == null)
        {
            Debug.LogWarning("[QuizManager] Answer clicked before ready or quiz data missing.");
            return;
        }

        try
        {
            //Prevents multiple answers being clicked
            canClickAnswers = false;

            //Disables all buttons
            foreach (var button in answerButtons)
            {
                if (button != null)
                    button.interactable = false;
            }

            //Gets the current question
            var question = quizOptions.questions[currentQuestionIndex];

            //Plays particle/sound if correct or incorrect
            if (index == question.correctAnswerIndex)
            {
                correctAnswersCount++;
                particleSystem?.Play();
                cheerAudio?.Play();
            }
            else
            {
                IncorrectAudio?.Play();
            }

            //Updates the result display
            if (resultText != null)
                resultText.text = $"{correctAnswersCount}/3 correct";

            //Moves to the next question or ends quiz
            currentQuestionIndex++;

            if (currentQuestionIndex < quizOptions.questions.Count)
                Invoke(nameof(ShowQuestion), 1f);
            else
                Invoke(nameof(EndQuiz), 1f);
        }
        catch (System.Exception ex)
        {
            //Logs error and ends quiz if something goes wrong
            Debug.LogError($"[QuizManager] Error in CheckAnswer: {ex.Message}");
            EndQuiz();
        }
    }
    void EndQuiz()
    {
        try
        {
            //Hides the quiz panel
            quizPanel?.SetActive(false);

            //Shows the final result if references are valid
            if (resultText != null && quizOptions != null && quizOptions.questions != null)
                resultText.text = $"Quiz Finished! You got {correctAnswersCount} out of {quizOptions.questions.Count} correct.";

            //Triggers any event listeners for finishing (changing to the next level)
            OnQuizFinished?.Invoke();
        }
        catch (System.Exception ex)
        {
            //Logs any unexpected issue when ending quiz
            Debug.LogError($"[QuizManager] Error in EndQuiz: {ex.Message}");
        }
    }
}

