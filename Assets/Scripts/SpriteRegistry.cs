using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class SpriteRegistry
{
    private static readonly Dictionary<string, Sprite> phenotypeMap = new();

    public static void Register(Creature creature, Sprite sprite)// adds a sprite and makes a key for it 
    {
        string key = MakeKey(creature);

        if (phenotypeMap.TryGetValue(key, out var existing))
        {
            if (existing == sprite)
            {
                return;//same sprite for this key  skip it  
            }
            else
            {
                Debug.LogWarning($"[SpriteRegistry] Sprite conflict for key '{key}'. Existing sprite differs from new one. Keeping original.");
                return;
            }
        }

        phenotypeMap[key] = sprite;
        Debug.Log($"[SpriteRegistry] Registered sprite for {creature.CreatureName} -> {key}");
    }



    public static Sprite GetSprite(Creature creature)//uses key to get value of sprite 
    {
        string key = MakeKey(creature);
        return phenotypeMap.TryGetValue(key, out var sprite) ? sprite : null;
    }

    public static string MakeKey(Creature creature)
    {
        string phenotype = null;

        foreach (var trait in creature.GetTraitNames())
        {
            string pheno = creature.GetPhenotype(trait);// uses the first phenotype found 
            if (!string.IsNullOrEmpty(pheno))
            {
                phenotype = pheno;
                break;
            }
        }

        if (string.IsNullOrEmpty(phenotype))
            throw new System.Exception($"[SpriteRegistry] Cannot determine phenotype for creature: {creature.CreatureName}");

        if (string.IsNullOrEmpty(creature.BodyColor))
            throw new System.Exception($"[SpriteRegistry] Missing BodyColor for creature: {creature.CreatureName}");

        if (string.IsNullOrEmpty(creature.Gender))
            throw new System.Exception($"[SpriteRegistry] Missing Gender for creature: {creature.CreatureName}");

        return $"{creature.CreatureName}_{creature.Gender}_{phenotype}_{creature.BodyColor}";
    }



}
