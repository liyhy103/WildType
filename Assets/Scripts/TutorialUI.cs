using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class TutorialUI : MonoBehaviour
{
    public GameObject TutorialPanel;
    public TMP_Text TutorialText;
    public Button ContinueButton;
    public Button SkipButton;
    public TMP_Dropdown Parent1;
    public TMP_Dropdown Parent2;
    public Button BreedButton;
    public List<Creature> creatures;
    public BreedingUI breedingUI;
    public Challenge challengeManager;

    private int step = 0; // tutorial step
    private bool challengeChecked = false;

    void Start(){
        BreedButton.interactable = false; // disable breeding initially
        ShowWelcomeMessage();
    }

    void ShowWelcomeMessage(){
        TutorialPanel.SetActive(true); // show text panel
        TutorialText.text = "Welcome to Wildtype!\nLet's learn how to breed creatures.";

        ContinueButton.onClick.RemoveAllListeners();
        ContinueButton.onClick.AddListener(() => ProceedToStep1());

        SkipButton.onClick.RemoveAllListeners();
        SkipButton.onClick.AddListener(() => SkipTutorial());
    }

    void ProceedToStep1(){
        step = 1;
        TutorialPanel.SetActive(true);
        TutorialText.text = "Step 1:\nChoose a green <b>male</b> and a yellow <b>female</b> from the dropdowns.";

        // Hide the Continue and Skip buttons
        ContinueButton.gameObject.SetActive(false);
        SkipButton.gameObject.SetActive(false);

        StartCoroutine(HideTutorialAndStartChecking()); // Start coroutine to hide the tutorial panel and begin selection checking
    }

    IEnumerator HideTutorialAndStartChecking(){
        yield return new WaitForSeconds(2.5f); // Wait for 2.5 seconds so the player can read the instruction

        TutorialPanel.SetActive(false); // Hide the tutorial panel
        InvokeRepeating(nameof(CheckParentSelection), 1f, 1f); // Start checking every 1 second whether the correct parents are selected
    }

    void SkipTutorial(){
        step = -1;  // sign as skip

        // Hide the tutorial panel, Continue and Skip buttons
        TutorialPanel.SetActive(false);
        ContinueButton.gameObject.SetActive(false);
        SkipButton.gameObject.SetActive(false);
        BreedButton.interactable = true;

        CancelInvoke(nameof(CheckParentSelection));
        Debug.Log("Skipped.");
    }

    void CheckParentSelection(){
        int index1 = Parent1.value;
        int index2 = Parent2.value;

        Creature p1 = breedingUI.GetCreature(index1);
        Creature p2 = breedingUI.GetCreature(index2);

        Debug.Log($"[Check] Parent1: {p1.CreatureName}, Gender: {p1.Gender}, Pheno: {p1.GetPhenotype("coatcolor")}");
        Debug.Log($"[Check] Parent2: {p2.CreatureName}, Gender: {p2.Gender}, Pheno: {p2.GetPhenotype("coatcolor")}");

        bool isP1Correct = p1.Gender == "Male" && p1.GetPhenotype("coatcolor") == "Green";
        bool isP2Correct = p2.Gender == "Female" && p2.GetPhenotype("coatcolor") == "Yellow";

        if (isP1Correct && isP2Correct && step == 1){
            step = 3;
            CancelInvoke(nameof(CheckParentSelection));

            TutorialText.text = "Perfect!\nNow read the <b>Challenge</b> and click the <b>Breed</b> button to continue!";;
            TutorialPanel.SetActive(true);
            StartCoroutine(HidePanelAfterDelay(2.5f));

            BreedButton.interactable = true;
            Debug.Log("[Tutorial] Breed button is now interactable.");
        }
    }

    IEnumerator HidePanelAfterDelay(float seconds){
        yield return new WaitForSeconds(seconds);
        TutorialPanel.SetActive(false); // hide tutorial panel
    }

    void Update(){
        // Step 3: challenge result check
        if (step == 3 && challengeManager != null){
            if (!challengeChecked && challengeManager.currentCreature != null){
                challengeChecked = true;
                StartCoroutine(CheckChallengeOutcome());
            }
        }
    }

    IEnumerator CheckChallengeOutcome(){
        yield return new WaitForSeconds(1.0f); // let challenge manager check
        
        TutorialText.text = "Challenge result has been updated.";
        TutorialPanel.SetActive(true);
        StartCoroutine(HidePanelAfterDelay(2.5f));

        if (challengeManager.CurrentChallenge == ""){
            step = 4;
            TutorialText.text = "Well done!\nYou've completed all challenge!";

            TutorialPanel.SetActive(true);
            StartCoroutine(HidePanelAfterDelay(2.5f));
        }
    }

    // Called by BreedingUI after offspring is created
    public void NotifyOffspring(Creature offspring){
        challengeChecked = false;
        challengeManager.currentCreature = offspring;
    }
}