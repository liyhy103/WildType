using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class BreedingUI : MonoBehaviour
{

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
    public RectTransform parent1Slot;
    public RectTransform parent2Slot;
    public GameObject parentDisplayPrefab;

    private GameObject parent1Instance;
    private GameObject parent2Instance;
    public RectTransform offspringSlot;

    private Creature selectedParent1;
    private Creature selectedParent2;

 


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
            BreedingType.Mendelian => Gene.Traits.CoatColor,
            BreedingType.SexLinked => Gene.Traits.ShellColor,
            BreedingType.IncompleteDominance => Gene.Traits.HornLength,
            BreedingType.DihybridInheritance => Gene.Traits.TailLength,
            _ => Gene.Traits.CoatColor
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
        Debug.Log($"[BreedingUI] Breeding strategy initialized: {breedingStrategy?.GetType().Name ?? "null"}");


        string sceneName = SceneManager.GetActiveScene().name;

        if (challengeManager == null)
        {
            challengeManager = FindFirstObjectByType<Challenge>(); ;
            if (challengeManager == null)
                Debug.LogWarning("ChallengeManager not found in the scene!");
        }

        if (sceneName == "LevelOne")
            breedingUIHandler = new LevelOneBreedingUIHandler(challengeManager as LevelOneChallenge);
        else if (sceneName == "LevelTwo")
            breedingUIHandler = new LevelTwoBreedingUIHandler(challengeManager as LevelTwoChallenge);
        else if (sceneName == "LevelThree")
            breedingUIHandler = new LevelThreeBreedingUIHandler(challengeManager as LevelThreeChallenges);
        else if (sceneName == "TutorialLevel")
            breedingUIHandler = new LevelOneBreedingUIHandler(challengeManager as LevelOneChallenge);
        else if (sceneName == "LevelFour")
            breedingUIHandler = new LevelFourBreedingUIHandler(challengeManager as LevelFourChallenge);
        else
            breedingUIHandler = null;


        Creature compendiumCreature = CreatureTransfer.CreatureToAssign;
        int targetParent = CreatureTransfer.TargetParentIndex;
        CreatureTransfer.CreatureToAssign = null;

        if (CompendiumManager.Instance.LevelStarters.TryGetValue(sceneName, out var starters))
        {
            foreach (var starter in starters)
            {
                Creatures.Add(starter);
                Sprite sprite = CompendiumManager.Instance.GetCreatureSprite(starter);

                if (!CompendiumManager.Instance.compendium.Contains(starter))
                {
                    if (sprite == null)
                        Debug.LogWarning($"[BreedingUI] No sprite found for: {starter.CreatureName}");
                    else
                        CompendiumManager.Instance.AddToCompendium(starter, sprite);
                }
                else
                {
                    if (CompendiumManager.Instance.GetCreatureSprite(starter) == null)
                    {
                        CompendiumManager.Instance.AddToCompendium(starter, sprite);
                        Debug.Log($"[BreedingUI] Starter sprite re-linked in compendium: {starter.CreatureName}");
                    }
                    else
                    {
                        Debug.Log($"[BreedingUI] Starter already in compendium with sprite: {starter.CreatureName}");
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning($"[BreedingUI] No level starters defined for scene {sceneName}");
        }


        if (compendiumCreature != null)
        {
            Debug.Log($"[BreedingUI] Compendium creature incoming: {compendiumCreature.CreatureName} from level {compendiumCreature.SourceLevel}");
            if (compendiumCreature.SourceLevel == sceneName)
            {
                if (string.IsNullOrEmpty(compendiumCreature.BodyColor) || compendiumCreature.BodyColor == null)
                {
                    string trait = GetCurrentTrait();
                    if (trait == Gene.Traits.CoatColor || trait == Gene.Traits.ShellColor)
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

        BreedButton.onClick.AddListener(OnBreedClicked);
    }


    private bool ShouldCompareBodyColor()
    {
        string levelName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        return levelName == "LevelOne" || levelName == "LevelFour";
    }
    //updates the parents traits for parent one and  two
    public void UpdateCreatureDisplayParent1(Creature creature)
    {
        string currentTrait = GetCurrentTrait();
        string gender = creature.Gender;
        string phenotype = creature.GetPhenotype(currentTrait);
        string bodyColor = creature.BodyColor;

        foreach (GameObject obj in parent1DisplayObjects)
        {
            var meta = obj.GetComponent<DisplayMetadata>();
            if (meta == null)
                continue;

            bool genderMatch = meta.Gender.Equals(gender, System.StringComparison.OrdinalIgnoreCase);
            bool phenotypeMatch = meta.Phenotype.Equals(phenotype, System.StringComparison.OrdinalIgnoreCase);
            bool bodyColorMatch = !ShouldCompareBodyColor() ||
                                  meta.BodyColor.Equals(bodyColor, System.StringComparison.OrdinalIgnoreCase);


            obj.SetActive(genderMatch && phenotypeMatch && bodyColorMatch);

            if (genderMatch && phenotypeMatch && bodyColorMatch)
                Debug.Log($"[BreedingUI] Activated parent1 sprite: {obj.name}");
        }
    }


    public void UpdateCreatureDisplayParent2(Creature creature)
    {
        string currentTrait = GetCurrentTrait();
        string gender = creature.Gender;
        string phenotype = creature.GetPhenotype(currentTrait);
        string bodyColor = creature.BodyColor;

        foreach (GameObject obj in parent2DisplayObjects)
        {
            var meta = obj.GetComponent<DisplayMetadata>();
            if (meta == null)
                continue;

            bool genderMatch = meta.Gender.Equals(gender, System.StringComparison.OrdinalIgnoreCase);
            bool phenotypeMatch = meta.Phenotype.Equals(phenotype, System.StringComparison.OrdinalIgnoreCase);
            bool bodyColorMatch = !ShouldCompareBodyColor() ||
                                  meta.BodyColor.Equals(bodyColor, System.StringComparison.OrdinalIgnoreCase);


            obj.SetActive(genderMatch && phenotypeMatch && bodyColorMatch);

            if (genderMatch && phenotypeMatch && bodyColorMatch)
                Debug.Log($"[BreedingUI] Activated parent2 sprite: {obj.name}");
        }
    }


    public void OnBreedClicked()
    {
        Debug.Log("[BreedingUI] Breed button clicked");

        if (selectedParent1 == null || selectedParent2 == null)
        {
            OffspringText.text = "Both parents must be selected!";
            return;
        }

        if (selectedParent1.Gender == selectedParent2.Gender)
        {
            OffspringText.text = "Breeding requires one male and one female parent!";
            return;
        }

        if (breedingUIHandler != null &&
            !breedingUIHandler.ValidateParents(this, selectedParent1, selectedParent2, out string errorMessage))
        {
            Debug.LogWarning($"[BreedingUI] Breeding failed: Validation failed with message: {errorMessage}");
            OffspringText.text = errorMessage;
            return;
        }

        string trait = GetCurrentTrait();
        Debug.Log($"[BreedingUI] Breeding using trait: {trait}");

        Creature offspring = Breed(selectedParent1, selectedParent2);
        if (offspring == null)
        {
            Debug.LogError("[BreedingUI] Breeding strategy returned null. Check logic.");
            OffspringText.text = "Breeding failed: no offspring was created!";
            return;
        }

        Debug.Log($"[BreedingUI] Offspring created: {offspring.CreatureName} ({offspring.Gender})");

        PlayHeartEffect();

        if (OffspringText != null)
            OffspringText.text = "";

        string phenotype = offspring.GetPhenotype(Gene.Traits.CoatColor);
        Debug.Log($"[BreedingUI] Offspring phenotype: {phenotype}");

        Sprite offspringSprite = breedingUIHandler?.GetOffspringSprite(this);
        if (offspringSprite == null)
        {
            offspringSprite = CompendiumManager.Instance.GetCreatureSprite(offspring);
            Debug.Log("[BreedingUI] Offspring sprite fallback: used GetCreatureSprite()");
        }


        if (breedingUIHandler != null)
        {
            Debug.Log("[BreedingUI] Displaying offspring using handler.");
            breedingUIHandler.ShowOffspring(this, offspring);
        }
        else
        {
            Debug.Log("[BreedingUI] No handler found. Instantiating offspring manually.");
            GameObject offspringInstance = Instantiate(parentDisplayPrefab, offspringSlot);
            offspringInstance.transform.localPosition = Vector3.zero;
            offspringInstance.transform.localScale = Vector3.one;
            UpdateCreatureImage(offspringInstance, offspringSprite);
            Debug.Log($"[BreedingUI] Displayed offspring: {offspring.CreatureName}, Sprite: {offspringSprite?.name ?? "NULL"}");
        }

        if (challengeManager != null)
        {
            challengeManager.SetResult(new List<string> { phenotype }, offspring);
            Debug.Log("[BreedingUI] ChallengeManager updated with offspring result");
        }

        lastOffspring = offspring;
        SaveToCompendiumButton.gameObject.SetActive(true);
        Debug.Log("[BreedingUI] Offspring saved to lastOffspring, save button enabled.");
    }

    public void OnSaveToCompendiumClicked()
    {
        if (lastOffspring != null)
        {
            if (CompendiumManager.Instance.compendium.Contains(lastOffspring))
            {
                Debug.Log("[BreedingUI] Creature already in compendium. Skipping save.");
                SaveToCompendiumButton.gameObject.SetActive(false);
                return;
            }

            Sprite sprite = breedingUIHandler?.GetOffspringSprite(this);
            lastOffspring.SourceLevel = SceneManager.GetActiveScene().name;
            Debug.Log("Saving creature with sprite: " + (sprite != null ? sprite.name : "NULL"));

            if (string.IsNullOrEmpty(lastOffspring.BodyColor))
            {
                string trait = GetCurrentTrait();
                if (trait == Gene.Traits.CoatColor || trait == Gene.Traits.ShellColor)
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
     

        if (!Creatures.Contains(creature))
            Creatures.Add(creature);

        Sprite sprite = CompendiumManager.Instance.GetCreatureSprite(creature);
        //Assigns based on chose option
        if (parentIndex == 1)
        {
            selectedParent1 = creature;
            if (parent1Instance != null) Destroy(parent1Instance);
            parent1Instance = Instantiate(parentDisplayPrefab, parent1Slot);
            
            UpdateCreatureImage(parent1Instance, sprite);
            
            UpdateCreatureDisplayParent1(creature);

            Debug.Log($"[BreedingUI] parent1Instance set to: {creature.CreatureName}");
        }
        else
        {
            selectedParent2 = creature;
            if (parent2Instance != null) Destroy(parent2Instance);
            parent2Instance = Instantiate(parentDisplayPrefab, parent2Slot);
            UpdateCreatureImage(parent2Instance, sprite);
            UpdateCreatureDisplayParent2(creature);
            Debug.Log($"[BreedingUI] parent2Instance set to: {creature.CreatureName}");
        }

        Debug.Log($"[BreedingUI] Assigned {creature.CreatureName} to Parent {parentIndex}");

        if (selectedParent1 != null && selectedParent2 != null)
            BreedButton.interactable = true;
    }

    void UpdateCreatureImage(GameObject displayObj, Sprite sprite)//instansiates the display object 
    {
        var image = displayObj.GetComponent<UnityEngine.UI.Image>();
        if (image != null)
        {
            image.sprite = sprite;
            image.preserveAspect = true;
        }
    }
}
