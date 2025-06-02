using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

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
    public Button SaveToCompendiumButton;
    public Button OpenCompendiumButton;
    private Creature currentOffspring;
    private static TutorialUI instance;

    private int step = 0; // tutorial step
    private bool challengeChecked = false;

    void Start(){
        // Avoid repeating the ShowWelcomeMessage
        if (step == 0){
            ShowWelcomeMessage();
        }
        BreedButton.interactable = false; // disable breeding initially
        SaveToCompendiumButton.interactable = false;
        OpenCompendiumButton.interactable = false;
    }

    void Awake(){
        if (instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);  // Keep TutorialManager across Scenes
        }
        else{
            Destroy(gameObject);  // Avoid duplicate creation
        }
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

        CancelInvoke(nameof(CheckParentSelection));
        BreedButton.interactable = true;
        SaveToCompendiumButton.interactable = true;
        OpenCompendiumButton.interactable = true;
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
            step = 2;
            CancelInvoke(nameof(CheckParentSelection));

            TutorialText.text = "Perfect!\nNow read the <b>Challenge</b> and click the <b>Breed</b> button to continue!"; ;
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
        // Step 2 → Step 3: After offspring is created
        if (step == 2 && currentOffspring != null && !SaveToCompendiumButton.interactable){
            step = 3;
            TutorialText.text = "Well done!\nNow click <b>Save</b> to store this creature.";
            TutorialPanel.SetActive(true);
            StartCoroutine(HidePanelAfterDelay(2.5f));
            SaveToCompendiumButton.interactable = true;

        }

        // Step 3 → Step 4: After save is done (button hidden)
        if (step == 3 && !SaveToCompendiumButton.gameObject.activeSelf){
            step = 4;
            TutorialText.text = "Awesome!\nYou can now open the <b>Compendium</b> to view saved creatures.\n" +
                                "You can click on a saved creature.\nAnd assign it as Parent 1 or Parent 2 to try new breeding combinations.\n" +
                                "Put your knowledge and strategy to the test.\nComplete the challenge to finish the Tutorial!";
            TutorialPanel.SetActive(true);
            StartCoroutine(HidePanelAfterDelay(7.0f));
            OpenCompendiumButton.interactable = true;
        }
    }

    // Called by BreedingUI after offspring is created
    public void NotifyOffspring(Creature offspring){
        currentOffspring = offspring;
        challengeManager.currentCreature = offspring;
        challengeChecked = false;
        Debug.Log($"[Tutorial] Offspring notified: {offspring.CreatureName}");
    }
}   