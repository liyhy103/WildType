using System.Collections.Generic;
using UnityEngine;

public class CompendiumManager : MonoBehaviour
{
    public static CompendiumManager Instance;

    public List<Creature> compendium = new List<Creature>();
    public Dictionary<Creature, Sprite> creatureSprites = new Dictionary<Creature, Sprite>();

    public string PreviousSceneName;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("[CompendiumManager] Singleton created and preserved.");
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
        if (creatureSprites.ContainsKey(creature))
            return creatureSprites[creature];
        return null;
    }
}


