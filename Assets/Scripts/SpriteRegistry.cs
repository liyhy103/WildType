using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class SpriteRegistry
{
    private static readonly Dictionary<string, Sprite> phenotypeMap = new();

    public static void RegisterFromPrefabs(DisplayMetadata[] prefabs)
    {
        foreach (var meta in prefabs)
        {
            string key = MakeKey(meta.Gender, meta.Phenotype, meta.BodyColor);
            Sprite sprite = meta.GetComponent<Image>()?.sprite;

            if (string.IsNullOrWhiteSpace(key) || sprite == null)
            {
                Debug.LogWarning("[SpriteRegistry] Skipped registering due to null key or sprite.");
                continue;
            }

            if (phenotypeMap.ContainsKey(key))
            {
                Debug.LogWarning($"[SpriteRegistry] Duplicate key detected: {key}. Registration skipped.");
                continue;
            }

            phenotypeMap[key] = sprite;
            Debug.Log($"[SpriteRegistry] Registered sprite for key: {key}");
        }
    }

    public static Sprite GetSprite(string gender, string phenotype, string bodyColor)
    {
        string key = MakeKey(gender, phenotype, bodyColor);
        if (phenotypeMap.TryGetValue(key, out var sprite))
        {
            return sprite;
        }

        return null;
    }

    public static Sprite GetSprite(Gender gender, string phenotype, string bodyColor)
    {
        return GetSprite(gender.ToString(), phenotype, bodyColor);
    }

    //default string formatter
    public static string MakeKey(string gender, string phenotype, string bodyColor)
    {
        return $"{gender.Trim().ToLower()}_{phenotype.Trim().ToLower()}_{bodyColor.Trim().ToLower()}";
    }

    //overloaded type formatter 
    public static string MakeKey(Gender gender, string phenotype, string bodyColor)
    {
        return MakeKey(gender.ToString(), phenotype, bodyColor);
    }
}
