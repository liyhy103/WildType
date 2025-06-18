using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class SpriteRegistry
{
    private static Dictionary<string, Sprite> phenotypeMap = new();

    public static void RegisterFromPrefabs(DisplayMetadata[] prefabs)
    {
        foreach (var meta in prefabs)
        {
            string key = MakeKey(meta.Gender, meta.Phenotype, meta.BodyColor);
            var sprite = meta.GetComponent<Image>()?.sprite;

            if (!string.IsNullOrEmpty(key) && sprite != null && !phenotypeMap.ContainsKey(key))
            {
                phenotypeMap[key] = sprite;
                Debug.Log($"[SpriteRegistry] Registered sprite for key: {key}");
            }
        }
    }

    public static Sprite GetSprite(string gender, string phenotype, string bodyColor)
    {
        string key = MakeKey(gender, phenotype, bodyColor);
        return phenotypeMap.TryGetValue(key, out var sprite) ? sprite : null;
    }

    private static string MakeKey(string gender, string phenotype, string bodyColor)
    {
        return $"{gender.Trim().ToLower()}_{phenotype.Trim().ToLower()}_{bodyColor.Trim().ToLower()}";
    }
}
