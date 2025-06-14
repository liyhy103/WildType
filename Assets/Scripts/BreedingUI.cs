using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BreedingUI : MonoBehaviour
{
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

    private string GetCurrentTrait()
    {
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

        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "LevelOne")
            breedingUIHandler = new LevelOneBreedingUIHandler();
        else if (sceneName == "LevelTwo")
            breedingUIHandler = new LevelTwoBreedingUIHandler(challengeManager as LevelTwoChallenge);
        else if (sceneName == "LevelThree")
            breedingUIHandler = new LevelThreeBreedingUIHandler(challengeManager as LevelThreeChallenges);
        else if (sceneName == "TutorialLevel")
            breedingUIHandler = new LevelOneBreedingUIHandler();
        else if (sceneName == "LevelFour")
            breedingUIHandler = new LevelFourBreedingUIHandler(challengeManager as LevelFourChallenge);
        else
            breedingUIHandler = null;

        if (challengeManager == null)
        {
            challengeManager = FindObjectOfType<Challenge>();
            if (challengeManager == null)
                Debug.LogWarning("ChallengeManager not found in the scene!");
        }

        Creature compendiumCreature = CreatureTransfer.CreatureToAssign;
        int targetParent = CreatureTransfer.TargetParentIndex;
        CreatureTransfer.CreatureToAssign = null;

        if (sceneName == "LevelOne")
        {
            Creatures.Add(new Creature("GreenDad", "Male", new List<Gene> { new Gene("CoatColor", 'G', 'G') }, "Green"));
            Creatures.Add(new Creature("YellowMom", "Female", new List<Gene> { new Gene("CoatColor", 'g', 'g') }, "Yellow"));
        }
        else if (sceneName == "LevelTwo")
        {
            Creatures.Add(new Creature("Parent2_Green_Light_Male", "Male", new List<Gene> { new Gene("ShellColor", 'b', 'Y') }, "Green"));
            Creatures.Add(new Creature("Parent2_Green_Dark_Female", "Female", new List<Gene> { new Gene("ShellColor", 'B', 'b') }, "Green"));
        }
        else if (sceneName == "LevelThree")
        {
            Creatures.Add(new Creature("LongHorn", "Male", new List<Gene> { new Gene("HornLength", 'L', 'L') }, "Green"));
            Creatures.Add(new Creature("NoHorn", "Male", new List<Gene> { new Gene("HornLength", 'l', 'l') }, "Green"));
            Creatures.Add(new Creature("NoHorn", "Female", new List<Gene> { new Gene("HornLength", 'l', 'l') }, "Green"));
        }
        else if (sceneName == "TutorialLevel")
        {
            Creatures.Add(new Creature("GreenDad", "Male", new List<Gene> { new Gene("CoatColor", 'G', 'G') }, "Green"));
            Creatures.Add(new Creature("YellowMom", "Female", new List<Gene> { new Gene("CoatColor", 'g', 'g') }, "Yellow"));
        }
        else if (sceneName == "LevelFour")
        {
            Creatures.Add(new Creature("Green_LongTail_Male", "Male", new List<Gene> { new Gene("CoatColor", 'G', 'G'), new Gene("TailLength", 'L', 'L') }, "Green"));
            Creatures.Add(new Creature("Yellow_ShortTail_Female", "Female", new List<Gene> { new Gene("CoatColor", 'g', 'g'), new Gene("TailLength", 'l', 'l') }, "Yellow"));
            Creatures.Add(new Creature("Yellow_ShortTail_Male", "Male", new List<Gene> { new Gene("CoatColor", 'g', 'g'), new Gene("TailLength", 'l', 'l') }, "Yellow"));
            Creatures.Add(new Creature("Green_ShortTail_Female", "Female", new List<Gene> { new Gene("CoatColor", 'G', 'G'), new Gene("TailLength", 'l', 'l') }, "Green"));
        }

        if (compendiumCreature != null)
        {
            Debug.Log($"[BreedingUI] Compendium creature incoming: {compendiumCreature.CreatureName} from level {compendiumCreature.SourceLevel}");
            if (compendiumCreature.SourceLevel == sceneName)
            {
                if (string.IsNullOrEmpty(compendiumCreature.BodyColor) || compendiumCreature.BodyColor == "Unknown")
                {
                    string trait = GetCurrentTrait();
                    if (trait == "CoatColor" || trait == "ShellColor")
                    {
                        compendiumCreature.BodyColor = compendiumCreature.GetPhenotype(trait);
                        Debug.Log($"[BreedingUI] Set missing BodyColor to {compendiumCreature.BodyColor} based on {trait}");
                    }
                }
                Creatures.Add(compendiumCreature);
                Debug.Log($"[BreedingUI] Added compendium creature: {compendiumCreature.CreatureName} ({compendiumCreature.Gender})");
            }
            else
            {
                Debug.LogWarning("[BreedingUI] Compendium creature source level mismatch. Not adding.");
            }
        }

        PopulateDropdown(Parent1);
        PopulateDropdown(Parent2);

        Parent1.onValueChanged.AddListener(UpdateCreatureDisplayParent1);
        Parent2.onValueChanged.AddListener(UpdateCreatureDisplayParent2);
        BreedButton.onClick.AddListener(OnBreedClicked);

        if (compendiumCreature != null && compendiumCreature.SourceLevel == sceneName)
        {
            int index = Creatures.FindIndex(c =>
                c.CreatureName == compendiumCreature.CreatureName &&
                c.Gender == compendiumCreature.Gender &&
                c.SourceLevel == compendiumCreature.SourceLevel);

            if (targetParent == 1)
            {
                Parent1.value = index;
                UpdateCreatureDisplayParent1(index);
            }
            else
            {
                Parent2.value = index;
                UpdateCreatureDisplayParent2(index);
            }
        }
        else
        {
            UpdateCreatureDisplayParent1(Parent1.value);
            UpdateCreatureDisplayParent2(Parent2.value);
        }
    }



    void PopulateDropdown(TMP_Dropdown dropdown)

    {
        Debug.Log($"[PopulateDropdown] Populating {dropdown.name} with {Creatures.Count} creatures.");

        int previousIndex = dropdown.value;  
        dropdown.ClearOptions();

        List<string> names = new List<string>();
        foreach (var creature in Creatures)
        {

            string label;


            if (breedingType == BreedingType.Mendelian)
            {
                label = $"{creature.CreatureName} ({creature.Gender})";
            }
            else if (breedingType == BreedingType.SexLinked)
            {
                label = $"{creature.BodyColor} - {creature.GetPhenotype("ShellColor")} ({creature.Gender})";
            }
            else if (breedingType == BreedingType.IncompleteDominance)
            {
                label = $"{creature.Gender} - {creature.GetPhenotype("HornLength")}";
            }
            else if (breedingType == BreedingType.DihybridInheritance)
            {
                label = $"{creature.Gender} - {creature.GetPhenotype("CoatColor")}, Tail: {creature.GetPhenotype("TailLength")}";
            }
            else
            {
                label = creature.CreatureName;
            }

            names.Add(label);
            Debug.Log($"[PopulateDropdown] Adding to dropdown: {label}");

        }

        dropdown.AddOptions(names);

        if (previousIndex >= 0 && previousIndex < dropdown.options.Count)
        {
            dropdown.value = previousIndex;
        }
        else
        {
            dropdown.value = 0;
        }

        int safeIndex = Mathf.Clamp(previousIndex, 0, dropdown.options.Count - 1);
        dropdown.value = safeIndex;
        dropdown.RefreshShownValue();  
        Debug.Log($"[PopulateDropdown] Final selection index for {dropdown.name}: {safeIndex}");
    }


    private bool ShouldCompareBodyColor()
    {
        string levelName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        return levelName == "LevelOne" || levelName == "LevelFour";
    }



    public void UpdateCreatureDisplayParent1(int index)
    {
        Creature creature = Creatures[index];
        string currentTrait = GetCurrentTrait();

        for (int i = 0; i < parent1DisplayObjects.Length; i++)
        {
            GameObject obj = parent1DisplayObjects[i];
            var meta = obj.GetComponent<DisplayMetadata>();

            bool match = meta.Gender.Trim().ToLower() == creature.Gender.Trim().ToLower()
                      && meta.Phenotype.Trim().ToLower() == creature.GetPhenotype(currentTrait).Trim().ToLower();

            if (ShouldCompareBodyColor())
            {
                match &= meta.BodyColor.Trim().ToLower() == creature.BodyColor.Trim().ToLower();
            }

            Debug.Log($"[MATCH DEBUG] meta.Gender={meta.Gender}, meta.Phenotype={meta.Phenotype}, meta.BodyColor={meta.BodyColor} || Creature(Gender:{creature.Gender}, Phenotype:{creature.GetPhenotype(currentTrait)}, Trait={currentTrait})");
            Debug.Log($"[Parent1] {obj.name} | Match: {match} vs Creature(Gender:{creature.Gender}, Phenotype:{creature.GetPhenotype(currentTrait)}, Trait={currentTrait})");

            obj.SetActive(match);

        }
    }




    public void UpdateCreatureDisplayParent2(int index)
    {
        Creature creature = Creatures[index];
        string currentTrait = GetCurrentTrait();

        for (int i = 0; i < parent2DisplayObjects.Length; i++)
        {
            GameObject obj = parent2DisplayObjects[i];
            var meta = obj.GetComponent<DisplayMetadata>();

            bool match = meta.Gender.Trim().ToLower() == creature.Gender.Trim().ToLower()
                      && meta.Phenotype.Trim().ToLower() == creature.GetPhenotype(currentTrait).Trim().ToLower();

            if (ShouldCompareBodyColor())
            {
                match &= meta.BodyColor.Trim().ToLower() == creature.BodyColor.Trim().ToLower();
            }

            Debug.Log($"[MATCH DEBUG] meta.Gender={meta.Gender}, meta.Phenotype={meta.Phenotype}, meta.BodyColor={meta.BodyColor} || Creature(Gender:{creature.Gender}, Phenotype:{creature.GetPhenotype(currentTrait)}, Trait={currentTrait})");
            Debug.Log($"[Parent2] {obj.name} | Match: {match} vs Creature(Gender:{creature.Gender}, Phenotype:{creature.GetPhenotype(currentTrait)}, Trait={currentTrait})");

            obj.SetActive(match);

        }
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
            challengeManager.SetResult(phenotype, offspring);
        }
        lastOffspring = offspring;
        SaveToCompendiumButton.gameObject.SetActive(true);


        if (tutorialUI != null)
        {
            tutorialUI.NotifyOffspring(offspring);
        }
    }

    public void OnSaveToCompendiumClicked()
    {
        if (lastOffspring != null)
        {
            Sprite sprite = breedingUIHandler?.GetOffspringSprite(this);
            lastOffspring.SourceLevel = SceneManager.GetActiveScene().name;
            Debug.Log("Saving creature with sprite: " + (sprite != null ? sprite.name : "NULL"));


            if (lastOffspring.BodyColor == "Unknown" || string.IsNullOrEmpty(lastOffspring.BodyColor))
            {
                string trait = GetCurrentTrait();

                // Explicit body color setting for levels that care
                if (trait == "CoatColor" || trait == "ShellColor")
                    lastOffspring.BodyColor = lastOffspring.GetPhenotype(trait);
            }

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

        UnityEngine.Debug.LogWarning($"[BreedingUI] Invalid creature index: {index}");
        return null;
    }

    public void AssignCompendiumCreature(int parentIndex, Creature creature)
    {
        if (creature.SourceLevel != SceneManager.GetActiveScene().name)
        {
            Debug.LogWarning("Cannot use this creature in a different level!");
            return;
        }

        int index = Creatures.IndexOf(creature);
        if (index == -1)
        {
            Creatures.Add(creature);

            PopulateDropdown(Parent1);
            PopulateDropdown(Parent2);

            index = Creatures.Count - 1;
        }

        if (parentIndex == 1)
        {
            Parent1.value = index;
            UpdateCreatureDisplayParent1(index);
        }
        else
        {
            Parent2.value = index;
            UpdateCreatureDisplayParent2(index);
        }

        Debug.Log($"Assigned {creature.CreatureName} to Parent {parentIndex}");
    }





}
