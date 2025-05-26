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
    public GameObject[] level3OffspringDisplayObjects;
    public GameObject[] level4OffspringDisplayObjects;


    public GameObject heartEffectPrefab;
    public Transform heartSpawn1;
    public Transform heartSpawn2;
    public float heartDuration = 2f;

    public Challenge challengeManager;

    public TMP_Text OffspringText;
    private List<Creature> Creatures = new List<Creature>();

    public TMP_Text Parent1GenotypeText;
    public TMP_Text Parent2GenotypeText;

    public Button SaveToCompendiumButton;
    private Creature lastOffspring;
    public TutorialUI tutorialUI;


    public enum BreedingType
    {
        Mendelian,
        SexLinked,
        IncompleteDominance,
        DihybridInheritance
    }

    public BreedingType breedingType = BreedingType.Mendelian;
    private IBreedingStrategy breedingStrategy;
    // Return the gene trait name according to the level type
    private string GetCurrentTrait(){
        return breedingType switch
        {
            BreedingType.Mendelian => "CoatColor",
            BreedingType.SexLinked => "ShellColor",
            BreedingType.IncompleteDominance => "HornLength",
            BreedingType.DihybridInheritance => "TailLength", 
            _ => "CoatColor"
        };
    }


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
            case BreedingType.DihybridInheritance:
                breedingStrategy = new DihybridInheritance();
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
        else if (sceneName == "LevelThree")
        {
            breedingUIHandler = new LevelThreeBreedingUIHandler();
        }
        else if (sceneName == "TutorialLevel")
        {
            breedingUIHandler = new LevelOneBreedingUIHandler();
        }
        else if (sceneName == "LevelFour")
        {
            breedingUIHandler = new LevelFourBreedingUIHandler();
        }
        else
        {
            breedingUIHandler = null; // Add other handlers here 
        }

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "LevelOne")
        {
            // Level 1 parents 
            Creatures.Add(new Creature("GreenDad", "Male", new List<Gene> { new Gene("CoatColor", 'G', 'y') }, "Green"));
            Creatures.Add(new Creature("YellowMom", "Female", new List<Gene> { new Gene("CoatColor", 'y', 'y') }, "Yellow"));
        }
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "LevelTwo")
        {
            // Level 2 parents 
            Creatures.Add(new Creature("Parent2_Green_Light_Male", "Male", new List<Gene> { new Gene("ShellColor", 'b', 'Y') }, "Green"));
            Creatures.Add(new Creature("Parent2_Green_Dark_Female", "Female", new List<Gene> { new Gene("ShellColor", 'B', 'b') }, "Green"));

        }

        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "LevelThree")
        {
            // Level 3 parents (all green)
            Creatures.Add(new Creature("LongHorn", "Male", new List<Gene> { new Gene("HornLength", 'L', 'L') }, "Green"));
            Creatures.Add(new Creature("ShortHorn", "Male", new List<Gene> { new Gene("HornLength", 'L', 'S') }, "Green"));
            Creatures.Add(new Creature("ShortHorn", "Female", new List<Gene> { new Gene("HornLength", 'S', 'L') }, "Green"));
            Creatures.Add(new Creature("NoHorn", "Female", new List<Gene> { new Gene("HornLength", 'S', 'S') }, "Green"));

        }
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "TutorialLevel")
        {
            // Level 1 parents 
            Creatures.Add(new Creature("GreenDad", "Male", new List<Gene> { new Gene("CoatColor", 'G', 'y') }, "Green"));
            Creatures.Add(new Creature("YellowMom", "Female", new List<Gene> { new Gene("CoatColor", 'y', 'y') }, "Yellow"));
        }
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "LevelFour")
        {
            // Level 4 parents
            Creatures.Add(new Creature("Green_LongTail_Male", "Male", new List<Gene> { new Gene("CoatColor", 'G', 'G'), new Gene("TailLength", 'L', 'L') }, "Green"));
            Creatures.Add(new Creature("Yellow_LongTail_Female", "Female", new List<Gene> { new Gene("CoatColor", 'y', 'y'), new Gene("TailLength", 'L', 'l') }, "Yellow"));
            Creatures.Add(new Creature("Yellow_ShortTail_Male", "Male", new List<Gene> { new Gene("CoatColor", 'y', 'y'), new Gene("TailLength", 'l', 'l') }, "Yellow"));
            Creatures.Add(new Creature("Green_ShortTail_Female", "Female", new List<Gene> { new Gene("CoatColor", 'G', 'y'), new Gene("TailLength", 'l', 'l') }, "Green"));
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
                label = $"{creature.BodyColor} - {creature.GetPhenotype("ShellColor")} ({creature.Gender})";
            }
            else if (breedingType == BreedingType.IncompleteDominance)
            {
                // Level Three
                label = $"{creature.Gender} - {creature.GetPhenotype("HornLength")}";
            }
            else if (breedingType == BreedingType.DihybridInheritance)
            {
                // Level Four
                label = $"{creature.Gender} - {creature.GetPhenotype("CoatColor")}, Tail: {creature.GetPhenotype("TailLength")}";
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
        string trait = GetCurrentTrait();
        var creature = Creatures[index];
        string gender = creature.Gender;
        string phenotype = creature.GetPhenotype(trait);
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
            Parent1GenotypeText.text = $"Genotype:\n{creature.GetGenotype(trait)}";
    }


    void UpdateCreatureDisplayParent2(int index)
    {
        string trait = GetCurrentTrait();
        var creature = Creatures[index];
        string gender = creature.Gender;
        string phenotype = creature.GetPhenotype(trait);
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
            Parent2GenotypeText.text = $"Genotype:\n{creature.GetGenotype(trait)}";

    }




    void OnBreedClicked()
    {

        if (Parent1.value == Parent2.value || Creatures[Parent1.value].Gender == Creatures[Parent2.value].Gender)
        {
            OffspringText.text = "Breeding requires one male and one female parent!";
            return;
        }

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

        string trait = GetCurrentTrait();
        Creature offspring = Breed(parent1, parent2);

        string result = $"Offspring Created!\n" +
                        $"- Name: {offspring.CreatureName}\n" +
                        $"- Gender: {offspring.Gender}";

        foreach (var pair in offspring.Genes)
        {
            string geneTrait = pair.Key;
            string genePhenotype = offspring.GetPhenotype(geneTrait);
            string geneGenotype = offspring.GetGenotype(geneTrait);
            result += $"\n- {geneTrait}: {genePhenotype} [{geneGenotype}]";
        }
        PlayHeartEffect();
        OffspringText.text = result;

        string phenotype = offspring.GetPhenotype("CoatColor");

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
            challengeManager.SetResult(phenotype);
        }
        lastOffspring = offspring;
        SaveToCompendiumButton.gameObject.SetActive(true);


    }

    public void OnSaveToCompendiumClicked()
    {
        if (lastOffspring != null)
        {
            Sprite sprite = breedingUIHandler?.GetOffspringSprite(this);
            lastOffspring.SourceLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

            Debug.Log("Saving creature with sprite: " + (sprite != null ? sprite.name : "NULL"));

            CompendiumManager.Instance.AddToCompendium(lastOffspring, sprite);
            SaveToCompendiumButton.gameObject.SetActive(false);
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

    public Creature GetCreature(int index)
    {
        if (index >= 0 && index < Creatures.Count)
            return Creatures[index];

        Debug.LogWarning($"[BreedingUI] Invalid creature index: {index}");
        return null;
    }

}
