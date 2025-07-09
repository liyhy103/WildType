using System.Collections.Generic;
using UnityEngine;
using static SpriteRegistry;

public class CompendiumManager : MonoBehaviour
{
    public static CompendiumManager Instance;

    public List<Creature> compendium = new();
    public Dictionary<Creature, Sprite> creatureSprites = new();

    public string PreviousSceneName;

    public Dictionary<string, List<Creature>> LevelStarters = new();
    private static Dictionary<string, List<Creature>> SavedCompendiumMemory = new();
    private static Dictionary<Creature, Sprite> SavedSpriteMemory = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("[CompendiumManager] Singleton created and preserved.");
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("[CompendiumManager] Duplicate destroyed.");
            return;
        }

        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        if (SavedCompendiumMemory.TryGetValue(sceneName, out var saved))
        {
            compendium = new List<Creature>(saved);
            Debug.Log($"[CompendiumManager] Restored compendium memory for {sceneName}");

            foreach (var creature in compendium)
            {
                if (SavedSpriteMemory.TryGetValue(creature, out var sprite))
                {
                    creatureSprites[creature] = sprite;
                }
            }
        }

        InitializeLevelStarters();
    }

    private void OnDestroy()
    {
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        SavedCompendiumMemory[sceneName] = new List<Creature>(compendium);

        foreach (var entry in creatureSprites)
        {
            SavedSpriteMemory[entry.Key] = entry.Value;
        }

        Debug.Log($"[CompendiumManager] Saved compendium memory and sprites for {sceneName}");
    }

    public void AddToCompendium(Creature creature, Sprite sprite = null)
    {
        if (!compendium.Contains(creature))
        {
            compendium.Add(creature);
            Debug.Log($"[CompendiumManager] Creature added: {creature.GetFullDescription()}");
        }
        else
        {
            Debug.Log($"[CompendiumManager] Already present: {creature.GetFullDescription()}");
        }

        if (sprite != null)
        {
            creatureSprites[creature] = sprite;
        }
    }

    public Sprite GetCreatureSprite(Creature creature)
    {
        return creatureSprites.TryGetValue(creature, out var sprite) ? sprite : null;
    }

    private void InitializeLevelStarters()
    {
        // Tutorial Level
        var TutorialLevelDad = new Creature("GreenDad", "Male", new List<Gene> {
            new Gene(Gene.Traits.CoatColor, 'G', 'G') }, "Green")
        { SourceLevel = "TutorialLevel" };
        var TutorialLevelMom = new Creature("YellowMom", "Female", new List<Gene> {
            new Gene(Gene.Traits.CoatColor, 'g', 'g') }, "Yellow")
        { SourceLevel = "TutorialLevel" };

        LevelStarters["TutorialLevel"] = new List<Creature> { TutorialLevelDad, TutorialLevelMom };
        creatureSprites[TutorialLevelDad] = SpriteRegistry.GetSprite("Male", TutorialLevelDad.GetPhenotype(Gene.Traits.CoatColor), "Green");
        creatureSprites[TutorialLevelMom] = SpriteRegistry.GetSprite("Female", TutorialLevelMom.GetPhenotype(Gene.Traits.CoatColor), "Yellow");

        // Level One
        var levelOneDad = new Creature("GreenDad", Creature.GenderMale, new List<Gene> {
            new Gene(Gene.Traits.CoatColor, 'G', 'G') }, Gene.Phenotypes.Green)
        { SourceLevel = Creature.LevelOne };

        var levelOneMom = new Creature("YellowMom", Creature.GenderFemale, new List<Gene> {
            new Gene(Gene.Traits.CoatColor, 'g', 'g') }, Gene.Phenotypes.Yellow)
        { SourceLevel = Creature.LevelOne };

        LevelStarters[Creature.LevelOne] = new List<Creature> { levelOneDad, levelOneMom };
        creatureSprites[levelOneDad] = GetSprite(levelOneDad.Gender, levelOneDad.GetPhenotype(Gene.Traits.CoatColor), levelOneDad.BodyColor);
        creatureSprites[levelOneMom] = GetSprite(levelOneMom.Gender, levelOneMom.GetPhenotype(Gene.Traits.CoatColor), levelOneMom.BodyColor);

        // Level Two
        var levelTwoMale = new Creature("Parent1_Green_Light_Male", Creature.GenderMale, new List<Gene> {
            new Gene(Gene.Traits.ShellColor, 'b', 'Y') }, Gene.Phenotypes.Green)
        { SourceLevel = Creature.LevelTwo };

        var levelTwoFemale = new Creature("Parent2_Green_Dark_Female", Creature.GenderFemale, new List<Gene> {
            new Gene(Gene.Traits.ShellColor, 'B', 'b') }, Gene.Phenotypes.Green)
        { SourceLevel = Creature.LevelTwo };

        LevelStarters[Creature.LevelTwo] = new List<Creature> { levelTwoMale, levelTwoFemale };
        creatureSprites[levelTwoMale] = GetSprite(levelTwoMale.Gender, levelTwoMale.GetPhenotype(Gene.Traits.ShellColor), levelTwoMale.BodyColor);
        creatureSprites[levelTwoFemale] = GetSprite(levelTwoFemale.Gender, levelTwoFemale.GetPhenotype(Gene.Traits.ShellColor), levelTwoFemale.BodyColor);

        // Level Three
        var levelThreeMale = new Creature("Parent1_Green_Long_Male", Creature.GenderMale, new List<Gene> {
            new Gene(Gene.Traits.HornLength, 'L', 'L') }, Gene.Phenotypes.Green)
        { SourceLevel = Creature.LevelThree };

        var levelThreeFemale = new Creature("Parent1_Green_No_Female", Creature.GenderFemale, new List<Gene> {
            new Gene(Gene.Traits.HornLength, 'l', 'l') }, Gene.Phenotypes.Green)
        { SourceLevel = Creature.LevelThree };

        LevelStarters[Creature.LevelThree] = new List<Creature> { levelThreeMale, levelThreeFemale };
        creatureSprites[levelThreeMale] = GetSprite(levelThreeMale.Gender, levelThreeMale.GetPhenotype(Gene.Traits.HornLength), levelThreeMale.BodyColor);
        creatureSprites[levelThreeFemale] = GetSprite(levelThreeFemale.Gender, levelThreeFemale.GetPhenotype(Gene.Traits.HornLength), levelThreeFemale.BodyColor);

        // Level Four
        var c1 = new Creature("Parent1_Green_LongTail_Male", Creature.GenderMale, new List<Gene> {
            new Gene(Gene.Traits.CoatColor, 'G', 'G'),
            new Gene(Gene.Traits.TailLength, 'L', 'L') }, Gene.Phenotypes.Green)
        { SourceLevel = Creature.LevelFour };

        var c2 = new Creature("Parent1_Yellow_ShortTail_Female", Creature.GenderFemale, new List<Gene> {
            new Gene(Gene.Traits.CoatColor, 'g', 'g'),
            new Gene(Gene.Traits.TailLength, 'l', 'l') }, Gene.Phenotypes.Yellow)
        { SourceLevel = Creature.LevelFour };

        var c3 = new Creature("Parent1_Yellow_ShortTail_Male", Creature.GenderMale, new List<Gene> {
            new Gene(Gene.Traits.CoatColor, 'g', 'g'),
            new Gene(Gene.Traits.TailLength, 'l', 'l') }, Gene.Phenotypes.Yellow)
        { SourceLevel = Creature.LevelFour };

        var c4 = new Creature("Parent1_Green_ShortTail_Female", Creature.GenderFemale, new List<Gene> {
            new Gene(Gene.Traits.CoatColor, 'G', 'G'),
            new Gene(Gene.Traits.TailLength, 'l', 'l') }, Gene.Phenotypes.Green)
        { SourceLevel = Creature.LevelFour };

        LevelStarters[Creature.LevelFour] = new List<Creature> { c1, c2, c3, c4 };

        foreach (var creature in LevelStarters[Creature.LevelFour])
        {
            var phenotype = creature.GetPhenotype(Gene.Traits.TailLength);
            creatureSprites[creature] = GetSprite(creature.Gender, phenotype, creature.BodyColor);
        }
    }
}
