using System.Collections.Generic;
using UnityEngine;

public class CompendiumManager : MonoBehaviour
{
    public static CompendiumManager Instance;

    public List<Creature> compendium = new List<Creature>();
    public Dictionary<Creature, Sprite> creatureSprites = new Dictionary<Creature, Sprite>();

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
            Debug.Log("Creature added to compendium: " + creature.GetFullDescription());
        }
        else
        {
            Debug.Log("Creature already in compendium: " + creature.GetFullDescription());
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
        var levelOneDad = new Creature("GreenDad", "Male", new List<Gene> {
            new Gene("CoatColor", 'G', 'G') }, "Green")
        { SourceLevel = "LevelOne" };
        var levelOneMom = new Creature("YellowMom", "Female", new List<Gene> {
            new Gene("CoatColor", 'g', 'g') }, "Yellow")
        { SourceLevel = "LevelOne" };

        LevelStarters["LevelOne"] = new List<Creature> { levelOneDad, levelOneMom };
        creatureSprites[levelOneDad] = SpriteRegistry.GetSprite("Male", levelOneDad.GetPhenotype("CoatColor"), "Green");
        creatureSprites[levelOneMom] = SpriteRegistry.GetSprite("Female", levelOneMom.GetPhenotype("CoatColor"), "Yellow");

        // Level Two
        var levelTwoMale = new Creature("Parent1_Green_Light_Male", "Male", new List<Gene> {
            new Gene("ShellColor", 'b', 'Y') }, "Green")
        { SourceLevel = "LevelTwo" };
        var levelTwoFemale = new Creature("Parent2_Green_Dark_Female", "Female", new List<Gene> {
            new Gene("ShellColor", 'B', 'b') }, "Green")
        { SourceLevel = "LevelTwo" };

        LevelStarters["LevelTwo"] = new List<Creature> { levelTwoMale, levelTwoFemale };
        creatureSprites[levelTwoMale] = SpriteRegistry.GetSprite("Male", levelTwoMale.GetPhenotype("ShellColor"), "Green");
        creatureSprites[levelTwoFemale] = SpriteRegistry.GetSprite("Female", levelTwoFemale.GetPhenotype("ShellColor"), "Green");

        // Level  Three
        var levelThreeMale = new Creature("Parent1_Green_Long_Male", "Male", new List<Gene> {
            new Gene("HornLength", 'L', 'L') }, "Green")
        { SourceLevel = "LevelThree" };
        var levelThreeFemale = new Creature("Parent1_Green_No_Female", "Female", new List<Gene> {
            new Gene("HornLength", 'l', 'l') }, "Green")
        { SourceLevel = "LevelThree" };

        LevelStarters["LevelThree"] = new List<Creature> { levelThreeMale, levelThreeFemale };
        creatureSprites[levelThreeMale] = SpriteRegistry.GetSprite("Male", levelThreeMale.GetPhenotype("HornLength"), "Green");
        creatureSprites[levelThreeFemale] = SpriteRegistry.GetSprite("Female", levelThreeFemale.GetPhenotype("HornLength"), "Green");

        // Level Four-c stands creature since there a four starters this time
        var c1 = new Creature("Parent1_Green_LongTail_Male", "Male", new List<Gene> {
            new Gene("CoatColor", 'G', 'G'), new Gene("TailLength", 'L', 'L') }, "Green")
        { SourceLevel = "LevelFour" };

        var c2 = new Creature("Parent1_Yellow_ShortTail_Female", "Female", new List<Gene> {
            new Gene("CoatColor", 'g', 'g'), new Gene("TailLength", 'l', 'l') }, "Yellow")
        { SourceLevel = "LevelFour" };

        var c3 = new Creature("Parent1_Yellow_ShortTail_Male", "Male", new List<Gene> {
            new Gene("CoatColor", 'g', 'g'), new Gene("TailLength", 'l', 'l') }, "Yellow")
        { SourceLevel = "LevelFour" };

        var c4 = new Creature("Parent1_Green_ShortTail_Female", "Female", new List<Gene> {
            new Gene("CoatColor", 'G', 'G'), new Gene("TailLength", 'l', 'l') }, "Green")
        { SourceLevel = "LevelFour" };

        LevelStarters["LevelFour"] = new List<Creature> { c1, c2, c3, c4 };

        creatureSprites[c1] = SpriteRegistry.GetSprite("Male", c1.GetPhenotype("TailLength"), "Green");
        creatureSprites[c2] = SpriteRegistry.GetSprite("Female", c2.GetPhenotype("TailLength"), "Yellow");
        creatureSprites[c3] = SpriteRegistry.GetSprite("Male", c3.GetPhenotype("TailLength"), "Yellow");
        creatureSprites[c4] = SpriteRegistry.GetSprite("Female", c4.GetPhenotype("TailLength"), "Green");


    }
}
