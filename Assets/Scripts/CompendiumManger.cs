using System.Collections.Generic;
using UnityEngine;

public class CompendiumManager : MonoBehaviour
{
    public static CompendiumManager Instance;

    public List<Creature> compendium = new List<Creature>();
    public Dictionary<Creature, Sprite> creatureSprites = new Dictionary<Creature, Sprite>();

    public string PreviousSceneName;

    public Dictionary<string, List<Creature>> LevelStarters = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("[CompendiumManager] Singleton created and preserved.");
            InitializeLevelStarters(); 
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("[CompendiumManager] Duplicate destroyed.");
        }
    }

    public void AddToCompendium(Creature creature, Sprite sprite = null)
    {
        compendium.Add(creature);
        if (sprite != null)
        {
            creatureSprites[creature] = sprite;
        }
        Debug.Log("Creature added to compendium: " + creature.GetFullDescription());
    }

    public Sprite GetCreatureSprite(Creature creature)
    {
        return creatureSprites.ContainsKey(creature) ? creatureSprites[creature] : null;
    }

    private void InitializeLevelStarters()
    {
        var levelOneDad = new Creature("GreenDad", "Male", new List<Gene> { new Gene("CoatColor", 'G', 'G') }, "Green") { SourceLevel = "LevelOne" };
        var levelOneMom = new Creature("YellowMom", "Female", new List<Gene> { new Gene("CoatColor", 'g', 'g') }, "Yellow") { SourceLevel = "LevelOne" };
        LevelStarters["LevelOne"] = new List<Creature> { levelOneDad, levelOneMom };
        creatureSprites[levelOneDad] = SpriteRegistry.GetSprite("Male", "Green", "Green");
        creatureSprites[levelOneMom] = SpriteRegistry.GetSprite("Female", "Yellow", "Yellow");

        var levelTwoMale = new Creature("Parent1_Green_Light_Male", "Male", new List<Gene> { new Gene("ShellColor", 'b', 'Y') }, "Green") { SourceLevel = "LevelTwo" };
        var levelTwoFemale = new Creature("Parent2_Green_Dark_Female", "Female", new List<Gene> { new Gene("ShellColor", 'B', 'b') }, "Green") { SourceLevel = "LevelTwo" };
        LevelStarters["LevelTwo"] = new List<Creature> { levelTwoMale, levelTwoFemale };
        creatureSprites[levelTwoMale] = SpriteRegistry.GetSprite("Male", levelTwoMale.GetPhenotype("ShellColor"), "Green");
        creatureSprites[levelTwoFemale] = SpriteRegistry.GetSprite("Female", levelTwoFemale.GetPhenotype("ShellColor"), "Green");

        var levelThreeMale = new Creature("LongHorn", "Male", new List<Gene> { new Gene("HornLength", 'L', 'L') }, "Green") { SourceLevel = "LevelThree" };
        var levelThreeFemale = new Creature("NoHorn", "Female", new List<Gene> { new Gene("HornLength", 'l', 'l') }, "Green") { SourceLevel = "LevelThree" };
        LevelStarters["LevelThree"] = new List<Creature> { levelThreeMale, levelThreeFemale };
        creatureSprites[levelThreeMale] = SpriteRegistry.GetSprite("Male", levelThreeMale.GetPhenotype("HornLength"), "Green");
        creatureSprites[levelThreeFemale] = SpriteRegistry.GetSprite("Female", levelThreeFemale.GetPhenotype("HornLength"), "Green");

        var levelFourMale = new Creature("Green_LongTail_Male", "Male", new List<Gene> {
        new Gene("CoatColor", 'G', 'G'), new Gene("TailLength", 'L', 'L') }, "Green")
        { SourceLevel = "LevelFour" };
        var levelFourFemale = new Creature("Yellow_ShortTail_Female", "Female", new List<Gene> {
        new Gene("CoatColor", 'g', 'g'), new Gene("TailLength", 'l', 'l') }, "Yellow")
        { SourceLevel = "LevelFour" };
        LevelStarters["LevelFour"] = new List<Creature> { levelFourMale, levelFourFemale };
        creatureSprites[levelFourMale] = SpriteRegistry.GetSprite("Male", levelFourMale.GetPhenotype("TailLength"), "Green");
        creatureSprites[levelFourFemale] = SpriteRegistry.GetSprite("Female", levelFourFemale.GetPhenotype("TailLength"), "Yellow");
    }
}
