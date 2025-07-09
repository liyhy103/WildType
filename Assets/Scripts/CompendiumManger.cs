using System.Collections.Generic;
using UnityEngine;

public class CompendiumManager : MonoBehaviour
{
    public static CompendiumManager Instance;

    public List<Creature> compendium = new();
    public string PreviousSceneName;

    public Dictionary<string, List<Creature>> LevelStarters = new();

    // Static memory for preserving state between scenes
    private static Dictionary<string, List<Creature>> SavedCompendiumMemory = new();

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

        // Restore saved creatures if available
        if (SavedCompendiumMemory.TryGetValue(sceneName, out var saved))
        {
            compendium = new List<Creature>(saved);
            Debug.Log($"[CompendiumManager] Restored compendium memory for {sceneName}");
        }

        InitializeLevelStarters();
    }

    private void OnDestroy()
    {
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        // Save compendium state
        SavedCompendiumMemory[sceneName] = new List<Creature>(compendium);

        Debug.Log($"[CompendiumManager] Saved compendium memory for {sceneName}");
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
            SpriteRegistry.Register(creature, sprite);
        }
    }

    public Sprite GetCreatureSprite(Creature creature)
    {
        return SpriteRegistry.GetSprite(creature);
    }

    private void InitializeLevelStarters()
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
