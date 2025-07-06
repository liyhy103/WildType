using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class CreatureSpriteEntry
{
    public string creatureName;
    public Sprite sprite;
}


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
    public TutorialUI tutorialUI;
    public RectTransform parent1Slot;
    public RectTransform parent2Slot;
    public GameObject parentDisplayPrefab;

    private GameObject parent1Instance;
    private GameObject parent2Instance;
    public RectTransform offspringSlot;

    private Creature selectedParent1;
    private Creature selectedParent2;

    [SerializeField]
    private List<CreatureSpriteEntry> starterSprites = new();


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
                UnityEngine.Debug.LogWarning("Unsupported breeding type selected.");

                break;

        }
        UnityEngine.Debug.Log($"[BreedingUI] Breeding strategy initialized: {breedingStrategy?.GetType().Name ?? "null"}");


        string sceneName = SceneManager.GetActiveScene().name;

        if (challengeManager == null)
        {
            challengeManager = FindFirstObjectByType<Challenge>(); ;
            if (challengeManager == null)
                UnityEngine.Debug.LogWarning("ChallengeManager not found in the scene!");
        }

        if (sceneName == "LevelOne")
        {
            breedingUIHandler = new LevelOneBreedingUIHandler();
        }
        else if (sceneName == "LevelTwo")
        {
            breedingUIHandler = new LevelTwoBreedingUIHandler(challengeManager as LevelTwoChallenge);
        }
        else if (sceneName == "LevelThree")
        {
            LevelThreeChallenges challenge = challengeManager as LevelThreeChallenges;
            if (challenge == null)
            {
                UnityEngine.Debug.LogError("challengeManager is not a LevelThreeChallenges or is null!");
            }
            breedingUIHandler = new LevelThreeBreedingUIHandler(challengeManager as LevelThreeChallenges);
        }
        else if (sceneName == "TutorialLevel")
        {
            breedingUIHandler = new LevelOneBreedingUIHandler();
        }
        else if (sceneName == "LevelFour")
        {
            breedingUIHandler = new LevelFourBreedingUIHandler(challengeManager as LevelFourChallenge);
        }
        else
        {
            breedingUIHandler = null;
        }


        Creature compendiumCreature = CreatureTransfer.CreatureToAssign;
        int targetParent = CreatureTransfer.TargetParentIndex;
        CreatureTransfer.CreatureToAssign = null;

        if (CompendiumManager.Instance.LevelStarters.TryGetValue(sceneName, out var starters))
        {
            foreach (var starter in starters)
            {
                Creatures.Add(starter);
                Sprite sprite = starterSprites.Find(s => s.creatureName == starter.CreatureName)?.sprite;

                if (sprite == null)
                {
                    UnityEngine.Debug.LogWarning($"[BreedingUI] No sprite found in inspector for: {starter.CreatureName}");
                    continue;
                }

                if (!CompendiumManager.Instance.compendium.Contains(starter))
                {
                    CompendiumManager.Instance.AddToCompendium(starter, sprite);
                    UnityEngine.Debug.Log($"[BreedingUI] Starter added to compendium: {starter.CreatureName}");
                }
                else
                {
                    if (CompendiumManager.Instance.GetCreatureSprite(starter) == null)
                    {
                        CompendiumManager.Instance.AddToCompendium(starter, sprite);
                        UnityEngine.Debug.Log($"[BreedingUI] Starter sprite re-linked in compendium: {starter.CreatureName}");
                    }
                    else
                    {
                        UnityEngine.Debug.Log($"[BreedingUI] Starter already in compendium with sprite: {starter.CreatureName}");
                    }
                }

            }

        }
        else
        {
            UnityEngine.Debug.LogWarning($"[BreedingUI] No level starters defined for scene {sceneName}");
        }

        if (compendiumCreature != null)
        {
            UnityEngine.Debug.Log($"[BreedingUI] Compendium creature incoming: {compendiumCreature.CreatureName} from level {compendiumCreature.SourceLevel}");
            if (compendiumCreature.SourceLevel == sceneName)
            {
                if (string.IsNullOrEmpty(compendiumCreature.BodyColor) || compendiumCreature.BodyColor == null)
                {
                    string trait = GetCurrentTrait();
                    if (trait == Gene.Traits.CoatColor || trait == Gene.Traits.ShellColor)
                    {
                        compendiumCreature.BodyColor = compendiumCreature.GetPhenotype(trait);
                        UnityEngine.Debug.Log($"[BreedingUI] Set missing BodyColor to {compendiumCreature.BodyColor} based on {trait}");
                    }
                }
                Creatures.Add(compendiumCreature);
                UnityEngine.Debug.Log($"[BreedingUI] Added compendium creature: {compendiumCreature.CreatureName} ({compendiumCreature.Gender})");
            }
            else
            {
                UnityEngine.Debug.LogWarning("[BreedingUI] Compendium creature source level mismatch. Not adding.");
            }
        }

        BreedButton.onClick.AddListener(OnBreedClicked);
    }


    private bool ShouldCompareBodyColor()
    {
        string levelName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        return levelName == "LevelOne" || levelName == "LevelFour";
    }

    public void UpdateCreatureDisplayParent1(Creature creature)
    {
        string currentTrait = GetCurrentTrait();

        foreach (GameObject obj in parent1DisplayObjects)
        {
            var meta = obj.GetComponent<DisplayMetadata>();
            bool match = meta != null &&
                         meta.Gender.Trim().ToLower() == creature.Gender.Trim().ToLower() &&
                         meta.Phenotype.Trim().ToLower() == creature.GetPhenotype(currentTrait).Trim().ToLower();

            if (ShouldCompareBodyColor())
            {
                match &= meta.BodyColor.Trim().ToLower() == creature.BodyColor.Trim().ToLower();
            }

            obj.SetActive(match);
        }
    }

    public void UpdateCreatureDisplayParent2(Creature creature)
    {
        string currentTrait = GetCurrentTrait();

        foreach (GameObject obj in parent2DisplayObjects)
        {
            var meta = obj.GetComponent<DisplayMetadata>();
            bool match = meta != null &&
                         meta.Gender.Trim().ToLower() == creature.Gender.Trim().ToLower() &&
                         meta.Phenotype.Trim().ToLower() == creature.GetPhenotype(currentTrait).Trim().ToLower();

            if (ShouldCompareBodyColor())
            {
                match &= meta.BodyColor.Trim().ToLower() == creature.BodyColor.Trim().ToLower();
            }

            obj.SetActive(match);
        }
    }

    public void OnBreedClicked()
    {
        UnityEngine.Debug.Log("[BreedingUI] Breed button clicked");

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
            UnityEngine.Debug.LogWarning($"[BreedingUI] Breeding failed: Validation failed with message: {errorMessage}");
            OffspringText.text = errorMessage;
            return;
        }

        string trait = GetCurrentTrait();
        UnityEngine.Debug.Log($"[BreedingUI] Breeding using trait: {trait}");

        Creature offspring = Breed(selectedParent1, selectedParent2);
        if (offspring == null)
        {
            UnityEngine.Debug.LogError("[BreedingUI] Breeding strategy returned null. Check logic.");
            OffspringText.text = "Breeding failed: no offspring was created!";
            return;
        }

        UnityEngine.Debug.Log($"[BreedingUI] Offspring created: {offspring.CreatureName} ({offspring.Gender})");

        PlayHeartEffect();

        if (OffspringText != null)
            OffspringText.text = ""; 

        string phenotype = offspring.GetPhenotype(Gene.Traits.CoatColor);
        UnityEngine.Debug.Log($"[BreedingUI] Offspring phenotype: {phenotype}");

        Sprite offspringSprite = breedingUIHandler?.GetOffspringSprite(this);
        if (offspringSprite == null)
        {
            offspringSprite = CompendiumManager.Instance.GetCreatureSprite(offspring);
            UnityEngine.Debug.Log("[BreedingUI] Offspring sprite fallback: used GetCreatureSprite()");
        }


        if (breedingUIHandler != null)
        {
            UnityEngine.Debug.Log("[BreedingUI] Displaying offspring using handler.");
            breedingUIHandler.ShowOffspring(this, offspring);
        }
        else
        {
            UnityEngine.Debug.Log("[BreedingUI] No handler found. Instantiating offspring manually.");
            GameObject offspringInstance = Instantiate(parentDisplayPrefab, offspringSlot);
            offspringInstance.transform.localPosition = Vector3.zero;
            offspringInstance.transform.localScale = Vector3.one;
            UpdateCreatureImage(offspringInstance, offspringSprite);
            UnityEngine.Debug.Log($"[BreedingUI] Displayed offspring: {offspring.CreatureName}, Sprite: {offspringSprite?.name ?? "NULL"}");
        }

        lastOffspring = offspring;
        SaveToCompendiumButton.gameObject.SetActive(true);
        UnityEngine.Debug.Log("[BreedingUI] Offspring saved to lastOffspring, save button enabled.");

        if (tutorialUI != null)
        {
            tutorialUI.NotifyOffspring(offspring);
            UnityEngine.Debug.Log("[BreedingUI] TutorialUI notified of new offspring");
        }
    }

    IEnumerator DisableButtonAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SaveToCompendiumButton.gameObject.SetActive(false);
    }

    public void OnSaveToCompendiumClicked()
    {
        if (lastOffspring != null)
        {
            if (CompendiumManager.Instance.compendium.Contains(lastOffspring))
            {
                UnityEngine.Debug.Log("[BreedingUI] Creature already in compendium. Skipping save.");
                SaveToCompendiumButton.gameObject.SetActive(false);
                return;
            }

            Sprite sprite = breedingUIHandler?.GetOffspringSprite(this);
            lastOffspring.SourceLevel = SceneManager.GetActiveScene().name;
            UnityEngine.Debug.Log("Saving creature with sprite: " + (sprite != null ? sprite.name : "NULL"));

            if (lastOffspring.BodyColor == "Unknown" || string.IsNullOrEmpty(lastOffspring.BodyColor))
            {
                string trait = GetCurrentTrait();
                if (trait == Gene.Traits.CoatColor || trait == Gene.Traits.ShellColor)
                    lastOffspring.BodyColor = lastOffspring.GetPhenotype(trait);
            }

            CompendiumManager.Instance.AddToCompendium(lastOffspring, sprite);
            StartCoroutine(DisableButtonAfterDelay(0.3f));
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
            UnityEngine.Debug.LogWarning("Cannot use this creature in a different level!");
            return;
        }

        if (!Creatures.Contains(creature))
            Creatures.Add(creature);

        Sprite sprite = CompendiumManager.Instance.GetCreatureSprite(creature);

        if (parentIndex == 1)
        {
            selectedParent1 = creature;
            if (parent1Instance != null) Destroy(parent1Instance);
            parent1Instance = Instantiate(parentDisplayPrefab, parent1Slot);
            UpdateCreatureImage(parent1Instance, sprite);
            UpdateCreatureDisplayParent1(creature);
            UnityEngine.Debug.Log($"[BreedingUI] parent1Instance set to: {creature.CreatureName}");
        }
        else
        {
            selectedParent2 = creature;
            if (parent2Instance != null) Destroy(parent2Instance);
            parent2Instance = Instantiate(parentDisplayPrefab, parent2Slot);
            UpdateCreatureImage(parent2Instance, sprite);
            UpdateCreatureDisplayParent2(creature);
            UnityEngine.Debug.Log($"[BreedingUI] parent2Instance set to: {creature.CreatureName}");
        }

        UnityEngine.Debug.Log($"[BreedingUI] Assigned {creature.CreatureName} to Parent {parentIndex}");

        if (selectedParent1 != null && selectedParent2 != null)
            BreedButton.interactable = true;
    }

    void UpdateCreatureImage(GameObject displayObj, Sprite sprite)
    {
        var image = displayObj.GetComponent<Image>();
        if (image != null)
        {
            image.sprite = sprite;
            image.preserveAspect = true;
        }
    }
}

