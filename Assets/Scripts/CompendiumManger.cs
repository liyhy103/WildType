using System.Collections.Generic;
using UnityEngine;
using static SpriteRegistry;
using static LevelNames;
using static BodyColors;


public class CompendiumManager : MonoBehaviour
{
    public static CompendiumManager Instance;

    public List<Creature> compendium = new List<Creature>();
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
        // Level One
        var levelOneDad = new Creature("GreenDad", Gender.Male.ToString(), new List<Gene> {
            new Gene(Gene.Traits.CoatColor, 'G', 'G') }, BodyColors.Green)
        { SourceLevel = LevelNames.LevelOne };

        var levelOneMom = new Creature("YellowMom", Gender.Female.ToString(), new List<Gene> {
            new Gene(Gene.Traits.CoatColor, 'g', 'g') }, BodyColors.Yellow)
        { SourceLevel = LevelNames.LevelOne };

        LevelStarters[LevelNames.LevelOne] = new List<Creature> { levelOneDad, levelOneMom };
        creatureSprites[levelOneDad] = SpriteRegistry.GetSprite(levelOneDad.Gender, levelOneDad.GetPhenotype(Gene.Traits.CoatColor), levelOneDad.BodyColor);
        creatureSprites[levelOneMom] = SpriteRegistry.GetSprite(levelOneMom.Gender, levelOneMom.GetPhenotype(Gene.Traits.CoatColor), levelOneMom.BodyColor);

        // Level Two
        var levelTwoMale = new Creature("Parent1_Green_Light_Male", Gender.Male.ToString(), new List<Gene> {
            new Gene(Gene.Traits.ShellColor, 'b', 'Y') }, BodyColors.Green)
        { SourceLevel = LevelNames.LevelTwo };

        var levelTwoFemale = new Creature("Parent2_Green_Dark_Female", Gender.Female.ToString(), new List<Gene> {
            new Gene(Gene.Traits.ShellColor, 'B', 'b') }, BodyColors.Green)
        { SourceLevel = LevelNames.LevelTwo };

        LevelStarters[LevelNames.LevelTwo] = new List<Creature> { levelTwoMale, levelTwoFemale };
        creatureSprites[levelTwoMale] = SpriteRegistry.GetSprite(levelTwoMale.Gender, levelTwoMale.GetPhenotype(Gene.Traits.ShellColor), levelTwoMale.BodyColor);
        creatureSprites[levelTwoFemale] = SpriteRegistry.GetSprite(levelTwoFemale.Gender, levelTwoFemale.GetPhenotype(Gene.Traits.ShellColor), levelTwoFemale.BodyColor);

        // Level Three
        var levelThreeMale = new Creature("Parent1_Green_Long_Male", Gender.Male.ToString(), new List<Gene> {
            new Gene(Gene.Traits.HornLength, 'L', 'L') }, BodyColors.Green)
        { SourceLevel = LevelNames.LevelThree };

        var levelThreeFemale = new Creature("Parent1_Green_No_Female", Gender.Female.ToString(), new List<Gene> {
            new Gene(Gene.Traits.HornLength, 'l', 'l') }, BodyColors.Green)
        { SourceLevel = LevelNames.LevelThree };

        LevelStarters[LevelNames.LevelThree] = new List<Creature> { levelThreeMale, levelThreeFemale };
        creatureSprites[levelThreeMale] = SpriteRegistry.GetSprite(levelThreeMale.Gender, levelThreeMale.GetPhenotype(Gene.Traits.HornLength), levelThreeMale.BodyColor);
        creatureSprites[levelThreeFemale] = SpriteRegistry.GetSprite(levelThreeFemale.Gender, levelThreeFemale.GetPhenotype(Gene.Traits.HornLength), levelThreeFemale.BodyColor);

        // Level Four
        var c1 = new Creature("Parent1_Green_LongTail_Male", Gender.Male.ToString(), new List<Gene> {
            new Gene(Gene.Traits.CoatColor, 'G', 'G'),
            new Gene(Gene.Traits.TailLength, 'L', 'L') }, BodyColors.Green)
        { SourceLevel = LevelNames.LevelFour };

        var c2 = new Creature("Parent1_Yellow_ShortTail_Female", Gender.Female.ToString(), new List<Gene> {
            new Gene(Gene.Traits.CoatColor, 'g', 'g'),
            new Gene(Gene.Traits.TailLength, 'l', 'l') }, BodyColors.Yellow)
        { SourceLevel = LevelNames.LevelFour };

        var c3 = new Creature("Parent1_Yellow_ShortTail_Male", Gender.Male.ToString(), new List<Gene> {
            new Gene(Gene.Traits.CoatColor, 'g', 'g'),
            new Gene(Gene.Traits.TailLength, 'l', 'l') }, BodyColors.Yellow)
        { SourceLevel = LevelNames.LevelFour };

        var c4 = new Creature("Parent1_Green_ShortTail_Female", Gender.Female.ToString(), new List<Gene> {
            new Gene(Gene.Traits.CoatColor, 'G', 'G'),
            new Gene(Gene.Traits.TailLength, 'l', 'l') }, BodyColors.Green)
        { SourceLevel = LevelNames.LevelFour };

        LevelStarters[LevelNames.LevelFour] = new List<Creature> { c1, c2, c3, c4 };

        foreach (var creature in LevelStarters[LevelNames.LevelFour])
        {
            var phenotype = creature.GetPhenotype(Gene.Traits.TailLength);
            creatureSprites[creature] = SpriteRegistry.GetSprite(creature.Gender, phenotype, creature.BodyColor);
        }
    }
}
