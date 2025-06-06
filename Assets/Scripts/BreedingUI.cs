using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class BreedingUI : MonoBehaviour
{
    // UI 


    public TMP_Dropdown Parent1;
    public TMP_Dropdown Parent2;
    public Button BreedButton;

    private IBreedingUIHandler breedingUIHandler;


    public GameObject[] parent1DisplayObjects;
    public GameObject[] parent2DisplayObjects;

    public GameObject greenOffspringDisplay;
    public GameObject yellowOffspringDisplay;
    public GameObject greenLightShellDisplay;
    public GameObject yellowLightShellDisplay;

    public GameObject heartEffectPrefab;
    public Transform heartSpawn1;
    public Transform heartSpawn2;
    public float heartDuration = 2f;

    public Challenge challengeManager;

    public TMP_Text OffspringText;
    private List<Creature> Creatures = new List<Creature>();

    public TMP_Text Parent1GenotypeText;
    public TMP_Text Parent2GenotypeText;

    public enum BreedingType
    {
        Mendelian,
        SexLinked,
        IncompleteDominance
    }

    public BreedingType breedingType = BreedingType.Mendelian;
    private IBreedingStrategy breedingStrategy;



    void Start()
    {

        switch (breedingType)
        {
            case BreedingType.Mendelian:
                breedingStrategy = new MendelianBreedingStrategy();
                break;
            case BreedingType.SexLinked:
                breedingStrategy = new SexLinkedBreedingStrategy();
                break;
            case BreedingType.IncompleteDominance:
                breedingStrategy = new IncompleteDominance();
                break;
            default:
                Debug.LogWarning("Unsupported breeding type selected.");
                break;
        }

        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        if (sceneName == "LevelOne")
        {
            breedingUIHandler = new LevelOneBreedingUIHandler();
        }
        else if (sceneName == "LevelTwo")
        {
            breedingUIHandler = new LevelTwoBreedingUIHandler();
        }
        else
        {
            breedingUIHandler = null; // Add other handlers here 
        }

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "LevelOne")
        {
            // Level 1 parents 
            Creatures.Add(new Creature("GreenDad", "Male", new Gene("CoatColor", 'G', 'y')));
            Creatures.Add(new Creature("YellowMom", "Female", new Gene("CoatColor", 'y', 'y')));
        }
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "LevelTwo")
        {
            // Level 2 parents 
            Creatures.Add(new Creature("Parent2_Green_Light_Male", "Male", new Gene("ShellColor", 'b', 'Y'), "Green"));
            Creatures.Add(new Creature("Parent2_Yellow_Light_Female", "Female", new Gene("ShellColor", 'b', 'b'), "Yellow"));
            Creatures.Add(new Creature("Parent2_Yellow_Dark_Male", "Male", new Gene("ShellColor", 'B', 'Y'), "Yellow"));
            Creatures.Add(new Creature("Parent2_Green_Dark_Female", "Female", new Gene("ShellColor", 'B', 'b'), "Green"));

            foreach (var c in Creatures)
            {
                Debug.Log($"[Start] Creature: {c.CreatureName} ({c.Gender}) [{c.GetPhenotype()}]");
            }
        }

        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "LevelThree")
        {
            // Level 3 parents
            Creatures.Add(new Creature("LongHorn", "Male", new Gene("HornLength", 'L', 'L')));
            Creatures.Add(new Creature("MediumHorn", "Male", new Gene("HornLength", 'L', 'S')));
            Creatures.Add(new Creature("MediumHorn", "Female", new Gene("HornLength", 'S', 'L')));
            Creatures.Add(new Creature("ShortHorn", "Female", new Gene("HornLength", 'S', 'S')));
        }

        PopulateDropdown(Parent1);
        PopulateDropdown(Parent2);

        Parent1.onValueChanged.AddListener(UpdateCreatureDisplayParent1);
        Parent2.onValueChanged.AddListener(UpdateCreatureDisplayParent2);
        BreedButton.onClick.AddListener(OnBreedClicked);

        UpdateCreatureDisplayParent1(Parent1.value);
        UpdateCreatureDisplayParent2(Parent2.value);
    }



    void PopulateDropdown(TMP_Dropdown dropdown)
    {
        dropdown.ClearOptions();
        List<string> names = new List<string>();

        foreach (var creature in Creatures)
        {
            string label;

            if (breedingType == BreedingType.Mendelian)
            {
                // Level One
                label = $"{creature.CreatureName} ({creature.Gender})";
            }
            else if (breedingType == BreedingType.SexLinked)
            {
                // Level Two
                label = $"{creature.BodyColor} - {creature.GetPhenotype()} ({creature.Gender})";
            }
            else if (breedingType == BreedingType.IncompleteDominance)
            {
                // Level Three
                label = $"{creature.Gender} - {creature.GetPhenotype()}";
            }
            else
            {
                label = creature.CreatureName;
            }

            names.Add(label);
        }

        dropdown.AddOptions(names);
    }


    void UpdateCreatureDisplayParent1(int index)
    {
        var creature = Creatures[index];
        string gender = creature.Gender;
        string phenotype = creature.GetPhenotype();
        string bodyColor = creature.BodyColor;

        foreach (GameObject obj in parent1DisplayObjects)
        {
            var meta = obj.GetComponent<DisplayMetadata>();
            if (meta == null)
            {
                obj.SetActive(false);
                continue;
            }

            bool match = meta.Gender == gender &&
                         meta.Phenotype == phenotype &&
                         meta.BodyColor == bodyColor;

            obj.SetActive(match);
            Debug.Log($"[Parent1] {obj.name} | Match: {match} vs Creature(Gender:{gender}, Phenotype:{phenotype}, Color:{bodyColor})");
           

        }
        if (Parent1GenotypeText != null)
            Parent1GenotypeText.text = $"Genotype:\n{creature.GetGenotype()}";
    }


    void UpdateCreatureDisplayParent2(int index)
    {
        var creature = Creatures[index];
        string gender = creature.Gender;
        string phenotype = creature.GetPhenotype();
        string bodyColor = creature.BodyColor;

        foreach (GameObject obj in parent2DisplayObjects)
        {
            var meta = obj.GetComponent<DisplayMetadata>();
            if (meta == null)
            {
                obj.SetActive(false);
                continue;
            }

            bool match = meta.Gender == gender &&
                         meta.Phenotype == phenotype &&
                         meta.BodyColor == bodyColor;

            obj.SetActive(match);
            Debug.Log($"[Parent2] {obj.name} | Match: {match} vs Creature(Gender:{gender}, Phenotype:{phenotype}, Color:{bodyColor})");

        }
        if (Parent2GenotypeText != null)
            Parent2GenotypeText.text = $"Genotype:\n{creature.GetGenotype()}";

    }




    void OnBreedClicked()
    {
        int index1 = Parent1.value;
        int index2 = Parent2.value;

        Creature parent1 = Creatures[index1];
        Creature parent2 = Creatures[index2];

        if (breedingUIHandler != null &&
            !breedingUIHandler.ValidateParents(this, parent1, parent2, out string errorMessage))
        {
            OffspringText.text = errorMessage;
            return;
        }

        Creature offspring = Breed(parent1, parent2);

        string result = $"Offspring Created!\n" +
                        $"- Name: {offspring.CreatureName}\n" +
                        $"- Gender: {offspring.Gender}\n" +
                        $"- Coat Color: {offspring.GetPhenotype()} [{offspring.GetGenotype()}]";

        PlayHeartEffect();
        OffspringText.text = result;

        string phenotype = offspring.GetPhenotype().ToLower();

        if (breedingUIHandler != null)
        {
            breedingUIHandler.ShowOffspring(this, offspring);
        }
        else
        {
            greenOffspringDisplay.SetActive(false);
            yellowOffspringDisplay.SetActive(false);

            if (phenotype == "green")
                greenOffspringDisplay.SetActive(true);
            else if (phenotype == "yellow")
                yellowOffspringDisplay.SetActive(true);
        }

        if (challengeManager != null)
        {
            challengeManager.setResult(phenotype);
        }

    }


    Creature Breed(Creature p1, Creature p2)
{
    return breedingStrategy.Breed(p1, p2);
}


void PlayHeartEffect()
    {
        Instantiate(heartEffectPrefab, heartSpawn1.position, Quaternion.identity, heartSpawn1);
        Instantiate(heartEffectPrefab, heartSpawn2.position, Quaternion.identity, heartSpawn2);
    }
}
