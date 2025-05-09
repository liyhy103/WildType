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

    public TMP_Text Parent1GenotypeText;
    public TMP_Text Parent2GenotypeText;


    public GameObject greenOffspringDisplay;
    public GameObject yellowOffspringDisplay;
    public GameObject greenLightShellDisplay;
    public GameObject yellowLightShellDisplay;


    public GameObject heartEffectPrefab;
    public Transform heartSpawn1;
    public Transform heartSpawn2;
    public float heartDuration = 2f;

    public TMP_Text OffspringText;
    private List<Creature> Creatures = new List<Creature>();
    public enum BreedingType
    {
        Mendelian,
        SexLinked
    }

    public BreedingType breedingType = BreedingType.Mendelian;
    private IBreedingStrategy breedingStrategy;
    private IBreedingUIHandler uiHandler;




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
            default:
                Debug.LogWarning("Unsupported breeding type selected.");
                break;
        }
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "LevelOne")
        {
            uiHandler = new LevelOneBreedingUIHandler();
        }
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "LevelTwo")
        {
            uiHandler = new LevelTwoBreedingUIHandler();
        }


        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "LevelOne")
        {
            // Level 1 parents 
            Creatures.Add(new Creature("GreenDad", "Male", new Gene("CoatColor", 'G', 'y'), "Green"));
            Creatures.Add(new Creature("YellowMom", "Female", new Gene("CoatColor", 'y', 'y'), "Yellow"));

        }
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "LevelTwo")
        {
            // Level 2 parents 
            Creatures.Add(new Creature("Parent1_Yellow_Light_Female", "Female", new Gene("ShellColor", 'b', 'b'), "Yellow"));
            Creatures.Add(new Creature("Parent1_Green_Dark_Female", "Female", new Gene("ShellColor", 'B', 'b'), "Green"));
            Creatures.Add(new Creature("Parent1_Green_Light_Male", "Male", new Gene("ShellColor", 'b', 'Y'), "Green"));
            Creatures.Add(new Creature("Parent1_Yellow_Dark_Male", "Male", new Gene("ShellColor", 'B', 'Y'), "Yellow"));





            foreach (var c in Creatures)
            {
                Debug.Log($"[Start] Creature: {c.CreatureName} ({c.Gender}) [{c.GetPhenotype()}]");
            }


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
            string entry = $"{creature.BodyColor} – {creature.GetPhenotype()} ({creature.Gender})";

            names.Add(entry);
        Debug.Log($"[PopulateDropdown:{dropdown.name}] Added: {entry}");
    }

    dropdown.AddOptions(names);
}


    void UpdateCreatureDisplayParent1(int index)
    {
        var creature = Creatures[index];
        string gender = creature.Gender;
        string phenotype = creature.GetPhenotype();

        string bodyColor = creature.BodyColor;

        // Fix: fallback only for Mendelian (Level One)
        if (string.IsNullOrEmpty(bodyColor) && breedingType == BreedingType.Mendelian)
        {
            bodyColor = phenotype;
        }

        foreach (GameObject obj in parent1DisplayObjects)
        {
            var meta = obj.GetComponent<DisplayMetadata>();
            if (meta == null)
            {
                Debug.LogWarning($"{obj.name} missing DisplayMetadata!");
                obj.SetActive(false);
                continue;
            }

            bool match = meta.Gender == gender &&
                         meta.Phenotype == phenotype &&
                         meta.BodyColor == bodyColor;

            obj.SetActive(match);

            Debug.Log($"[Parent1] Checking: {obj.name} | Meta(Gender:{meta.Gender}, Phenotype:{meta.Phenotype}, Color:{meta.BodyColor}) vs Creature(Gender:{gender}, Phenotype:{phenotype}, Color:{bodyColor}) => Match: {match}");
        }

        if (Parent1GenotypeText != null)
            Parent1GenotypeText.text = $"Genotype: {creature.GetGenotype()}";
    }








    void UpdateCreatureDisplayParent2(int index)
    {
        var creature = Creatures[index];
        string gender = creature.Gender;
        string phenotype = creature.GetPhenotype();

        string bodyColor = creature.BodyColor;

        // Fix: fallback only for Mendelian (Level One)
        if (string.IsNullOrEmpty(bodyColor) && breedingType == BreedingType.Mendelian)
        {
            bodyColor = phenotype;
        }

        foreach (GameObject obj in parent2DisplayObjects)
        {
            var meta = obj.GetComponent<DisplayMetadata>();
            if (meta == null)
            {
                Debug.LogWarning($"{obj.name} missing DisplayMetadata!");
                obj.SetActive(false);
                continue;
            }

            bool match = meta.Gender == gender &&
                         meta.Phenotype == phenotype &&
                         meta.BodyColor == bodyColor;

            obj.SetActive(match);
            Debug.Log($"[Parent2] Checking: {obj.name} | Meta(Gender:{meta.Gender}, Phenotype:{meta.Phenotype}, Color:{meta.BodyColor}) vs Creature(Gender:{gender}, Phenotype:{phenotype}, Color:{bodyColor}) => Match: {match}");
        }

        if (Parent2GenotypeText != null)
            Parent2GenotypeText.text = $"Genotype: {creature.GetGenotype()}";
    }









    public void OnBreedClicked()
    {
        int index1 = Parent1.value;
        int index2 = Parent2.value;

        Creature parent1 = Creatures[index1];
        Creature parent2 = Creatures[index2];

        if (!uiHandler.ValidateParents(this, parent1, parent2, out string error))
        {
            OffspringText.text = error;
            return;
        }

        Creature offspring = Breed(parent1, parent2);

        OffspringText.text =
            $"Offspring Created!\n" +
            $"- Name: {offspring.CreatureName}\n" +
            $"- Gender: {offspring.Gender}\n" +
            $"- Coat Color: {offspring.GetPhenotype()} [{offspring.GetGenotype()}]";

        PlayHeartEffect();
        uiHandler.ShowOffspring(this, offspring);
    }



    public Creature Breed(Creature p1, Creature p2)
{
    return breedingStrategy.Breed(p1, p2);
}


public void PlayHeartEffect()
    {
        Instantiate(heartEffectPrefab, heartSpawn1.position, Quaternion.identity, heartSpawn1);
        Instantiate(heartEffectPrefab, heartSpawn2.position, Quaternion.identity, heartSpawn2);
    }
}
