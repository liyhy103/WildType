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

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "LevelOne")
        {
            // Level 1 parents 
            Creatures.Add(new Creature("GreenDad", "Male", new Gene("CoatColor", 'G', 'y')));
            Creatures.Add(new Creature("YellowMom", "Female", new Gene("CoatColor", 'y', 'y')));
        }
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "LevelTwo")
        {
            // Level 2 parents 
            Creatures.Add(new Creature("BlueDad", "Male", new Gene("CoatColor", 'B', 'B')));
            Creatures.Add(new Creature("PinkMom", "Female", new Gene("CoatColor", 'P', 'B')));
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



    void PopulateDropdown(TMP_Dropdown dropdown){
        dropdown.ClearOptions();
        List<string> names = new List<string>();

        foreach (var creature in Creatures){
            names.Add($"{creature.CreatureName} ({creature.Gender})");
        }
        dropdown.AddOptions(names);
    }

    void UpdateCreatureDisplayParent1(int index)
    {
        for (int i = 0; i < parent1DisplayObjects.Length; i++)
        {
            parent1DisplayObjects[i].SetActive(i == index);
        }
    }

    void UpdateCreatureDisplayParent2(int index)
    {
        for (int i = 0; i < parent2DisplayObjects.Length; i++)
        {
            parent2DisplayObjects[i].SetActive(i == index);
        }
    }



    void OnBreedClicked()
    {
        int index1 = Parent1.value;
        int index2 = Parent2.value;

        // Avoid selecting the same creature twice
        if (index1 == index2)
        {
            OffspringText.text = "Please select two different parents!";
            return;
        }

        Creature parent1 = Creatures[index1];
        Creature parent2 = Creatures[index2];
        Creature offspring = Breed(parent1, parent2);

        string result = $"Offspring Created!\n" +
                        $"- Name: {offspring.CreatureName}\n" +
                        $"- Gender: {offspring.Gender}\n" +
                        $"- Coat Color: {offspring.GetPhenotype()} [{offspring.GetGenotype()}]";

        PlayHeartEffect();

        OffspringText.text = result;

        string phenotype = offspring.GetPhenotype();

        greenOffspringDisplay.SetActive(false);
        yellowOffspringDisplay.SetActive(false);

        if (phenotype == "Green")
        {
            greenOffspringDisplay.SetActive(true);
            challengeManager.setResult("green");
        }
        else if (phenotype == "Yellow")
        {
            yellowOffspringDisplay.SetActive(true);
            challengeManager.setResult("yellow");
        }

        else if (phenotype == "Blue")
        {
            greenOffspringDisplay.SetActive(true);
        }
        else if (phenotype == "Pink")
        {
            yellowOffspringDisplay.SetActive(true);
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
