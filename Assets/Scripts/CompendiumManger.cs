using System.Collections.Generic;
using UnityEngine;

public class CompendiumManager : MonoBehaviour
{
    public static CompendiumManager Instance;

    public List<Creature> compendium = new();
    public string PreviousSceneName;

    public Dictionary<string, List<Creature>> LevelStarters = new();

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

        InitializeLevelStarters();
    }

    public void AddToCompendium(Creature creature, Sprite sprite) //adds a creature to compendium and registers its sprite 
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
            SpriteRegistry.Register(creature, sprite);
        }
    }

    public Sprite GetCreatureSprite(Creature creature)//get sprite from sprite registery
    {
        return SpriteRegistry.GetSprite(creature);
    }

    private void InitializeLevelStarters()//loads default options 
    {
        string levelName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        var starters = StarterCreatureLoader.LoadStarters(levelName);
        LevelStarters[levelName] = starters;

        foreach (var c in starters)
        {
            Debug.Log($"[CompendiumManager] Loaded starter: {c.CreatureName} for {levelName}");
            Debug.Log($"[DebugCheck] Starter = {c.CreatureName}, Sprite = {SpriteRegistry.GetSprite(c)}");
        }
    }
}
