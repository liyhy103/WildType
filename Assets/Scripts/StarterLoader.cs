using UnityEngine;
using System.Collections.Generic;

public static class StarterCreatureLoader
{
 public static List<Creature> LoadStarters(string levelName)//Loads the starter objects from the file path and name 
    {
        var starters = new List<Creature>();
        var allAssets = Resources.LoadAll<StarterCreatureAsset>($"LevelStarters/{levelName}");

        foreach (var asset in allAssets)
        {
            var geneList = new List<Gene>();
            foreach (var def in asset.Genes)
                geneList.Add(new Gene(def.TraitName, def.Allele1, def.Allele2));

            var creature = new Creature(asset.CreatureName, asset.Gender, geneList, asset.BodyColor)
            {
                SourceLevel = asset.SourceLevel
            };

            starters.Add(creature);

            if (asset.Sprite != null)
            {
                SpriteRegistry.Register(creature, asset.Sprite);
            }

        }
            return starters;
    }
}
