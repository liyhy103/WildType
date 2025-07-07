using System.Collections;
using System.Collections.Generic;
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

    private bool TraitControlsBodyColor(string trait)
    {
        return trait == Gene.Traits.CoatColor || trait == Gene.Traits.ShellColor;
    }

    void Start()
    {
        switch (breedingType)
        {
            case BreedingType.Mendelian:
                breedingStrategy = new MendelianBreedingStrategy(); break;
            case BreedingType.SexLinked:
                breedingStrategy = new SexLinkedBreedingStrategy(); break;
            case BreedingType.IncompleteDominance:
                breedingStrategy = new IncompleteDominance(); break;
            case BreedingType.DihybridInheritance:
                breedingStrategy = new DihybridInheritance(); break;
            default:
                Debug.LogWarning("Unsupported breeding type selected."); break;
        }

        Debug.Log($"[BreedingUI] Breeding strategy initialized: {breedingStrategy?.GetType().Name ?? "null"}");

        string sceneName = SceneManager.GetActiveScene().name;

        if (challengeManager == null)
        {
            challengeManager = FindFirstObjectByType<Challenge>();
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
                Sprite sprite = starterSprites.Find(s => s.creatureName == starter.CreatureName)?.sprite;

                if (sprite == null)
                {
                    Debug.LogWarning($"[BreedingUI] No sprite found in inspector for: {starter.CreatureName}");
                    continue;
                }

                if (!CompendiumManager.Instance.compendium.Contains(starter))
                    CompendiumManager.Instance.AddToCompendium(starter, sprite);
                else if (CompendiumManager.Instance.GetCreatureSprite(starter) == null)
                    CompendiumManager.Instance.AddToCompendium(starter, sprite);
            }
        }

        if (compendiumCreature != null)
        {
            Debug.Log($"[BreedingUI] Compendium creature incoming: {compendiumCreature.CreatureName} from level {compendiumCreature.SourceLevel}");
            if (compendiumCreature.SourceLevel == sceneName)
            {
                string trait = GetCurrentTrait();
                if (string.IsNullOrEmpty(compendiumCreature.BodyColor) && TraitControlsBodyColor(trait))
                {
                    compendiumCreature.BodyColor = compendiumCreature.GetPhenotype(trait);
                    Debug.Log($"[BreedingUI] Set missing BodyColor to {compendiumCreature.BodyColor} based on {trait}");
                }

                Creatures.Add(compendiumCreature);
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
        string levelName = SceneManager.GetActiveScene().name;
        return levelName == "LevelOne" || levelName == "LevelFour";
    }

    public void UpdateCreatureDisplayParent1(Creature creature)
    {
        string currentTrait = GetCurrentTrait();
        Debug.Log($"[DEBUG] Updating Parent1 display for: {creature.CreatureName}, Gender={creature.Gender}, Phenotype={creature.GetPhenotype(currentTrait)}, BodyColor={creature.BodyColor}");

        foreach (GameObject obj in parent1DisplayObjects)
        {
            var meta = obj.GetComponent<DisplayMetadata>();
            if (meta == null) continue;

            bool genderMatch = meta.Gender.Trim().ToLower() == creature.Gender.Trim().ToLower();
            bool phenotypeMatch = meta.Phenotype.Trim().ToLower() == creature.GetPhenotype(currentTrait).Trim().ToLower();
            bool bodyColorMatch = !ShouldCompareBodyColor() || meta.BodyColor.Trim().ToLower() == creature.BodyColor.Trim().ToLower();

            bool isMatch = genderMatch && phenotypeMatch && bodyColorMatch;
            Debug.Log($"[DEBUG] Checking {obj.name} ? Gender: {genderMatch}, Phenotype: {phenotypeMatch}, BodyColor: {bodyColorMatch} => MATCH: {isMatch}");

            obj.SetActive(isMatch);

            if (isMatch)
            {
                Debug.Log($"[DEBUG] Activated Parent1 display: {obj.name}");
            }
        }
    }




    public void UpdateCreatureDisplayParent2(Creature creature)
    {
        string currentTrait = GetCurrentTrait();
        Debug.Log($"[DEBUG] Updating Parent2 display for: {creature.CreatureName}, Gender={creature.Gender}, Phenotype={creature.GetPhenotype(currentTrait)}, BodyColor={creature.BodyColor}");

        foreach (GameObject obj in parent2DisplayObjects)
        {
            var meta = obj.GetComponent<DisplayMetadata>();
            if (meta == null) continue;

            bool genderMatch = meta.Gender.Trim().ToLower() == creature.Gender.Trim().ToLower();
            bool phenotypeMatch = meta.Phenotype.Trim().ToLower() == creature.GetPhenotype(currentTrait).Trim().ToLower();
            bool bodyColorMatch = !ShouldCompareBodyColor() || meta.BodyColor.Trim().ToLower() == creature.BodyColor.Trim().ToLower();

            bool isMatch = genderMatch && phenotypeMatch && bodyColorMatch;
            Debug.Log($"[DEBUG] Checking {obj.name} ? Gender: {genderMatch}, Phenotype: {phenotypeMatch}, BodyColor: {bodyColorMatch} => MATCH: {isMatch}");

            obj.SetActive(isMatch);

            if (isMatch)
            {
                Debug.Log($"[DEBUG] Activated Parent2 display: {obj.name}");
            }
        }
    }



    public void OnBreedClicked()
    {
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
            OffspringText.text = errorMessage;
            return;
        }

        string trait = GetCurrentTrait();
        Creature offspring = Breed(selectedParent1, selectedParent2);
        if (offspring == null)
        {
            OffspringText.text = "Breeding failed: no offspring was created!";
            return;
        }

        PlayHeartEffect();
        OffspringText.text = "";

        Sprite offspringSprite = breedingUIHandler?.GetOffspringSprite(this)
                                 ?? CompendiumManager.Instance.GetCreatureSprite(offspring);

        if (breedingUIHandler != null)
            breedingUIHandler.ShowOffspring(this, offspring);
        else
        {
            GameObject offspringInstance = Instantiate(parentDisplayPrefab, offspringSlot);
            offspringInstance.transform.localPosition = Vector3.zero;
            offspringInstance.transform.localScale = Vector3.one;
            UpdateCreatureImage(offspringInstance, offspringSprite);
        }

        lastOffspring = offspring;
        SaveToCompendiumButton.gameObject.SetActive(true);

        tutorialUI?.NotifyOffspring(offspring);
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
                SaveToCompendiumButton.gameObject.SetActive(false);
                return;
            }

            string trait = GetCurrentTrait();
            if (string.IsNullOrEmpty(lastOffspring.BodyColor) && TraitControlsBodyColor(trait))
                lastOffspring.BodyColor = lastOffspring.GetPhenotype(trait);

            Sprite sprite = breedingUIHandler?.GetOffspringSprite(this);
            lastOffspring.SourceLevel = SceneManager.GetActiveScene().name;
            Debug.Log($"[DEBUG] Saving offspring sprite: {(sprite != null ? sprite.name : "NULL")} for {lastOffspring.CreatureName}");

            CompendiumManager.Instance.AddToCompendium(lastOffspring, sprite);
            StartCoroutine(DisableButtonAfterDelay(0.3f));
        }
    }

    Creature Breed(Creature p1, Creature p2) => breedingStrategy.Breed(p1, p2);

    void PlayHeartEffect()
    {
        Instantiate(heartEffectPrefab, heartSpawn1.position, Quaternion.identity, heartSpawn1);
        Instantiate(heartEffectPrefab, heartSpawn2.position, Quaternion.identity, heartSpawn2);
    }

    public Creature GetCreature(int index)
    {
        return (index >= 0 && index < Creatures.Count) ? Creatures[index] : null;
    }

    public void AssignCompendiumCreature(int parentIndex, Creature creature)
    {
        if (creature.SourceLevel != SceneManager.GetActiveScene().name)
            return;

        if (!Creatures.Contains(creature))
            Creatures.Add(creature);

        Sprite sprite = CompendiumManager.Instance.GetCreatureSprite(creature);
        Debug.Log($"[DEBUG] Retrieved sprite for {creature.CreatureName}: {(sprite != null ? sprite.name : "NULL")}");

        string trait = GetCurrentTrait();
        if (string.IsNullOrEmpty(creature.BodyColor) && TraitControlsBodyColor(trait))
        {
            creature.BodyColor = creature.GetPhenotype(trait);
            Debug.Log($"[DEBUG] Assigned BodyColor: {creature.BodyColor} from trait: {trait}");
        }

        if (parentIndex == 1)
        {
            selectedParent1 = creature;

            if (parent1Instance != null) Destroy(parent1Instance);
            parent1Instance = Instantiate(parentDisplayPrefab, parent1Slot);
            UpdateCreatureImage(parent1Instance, sprite);
            UpdateCreatureDisplayParent1(creature);
        }
        else
        {
            selectedParent2 = creature;

            if (parent2Instance != null) Destroy(parent2Instance);
            parent2Instance = Instantiate(parentDisplayPrefab, parent2Slot);
            UpdateCreatureImage(parent2Instance, sprite);
            UpdateCreatureDisplayParent2(creature);
        }

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
