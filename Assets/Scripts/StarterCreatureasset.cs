using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStarter", menuName = "WildType/Starter Creature")]
public class StarterCreatureAsset : ScriptableObject
{
    public string CreatureName;
    public string Gender;
    public string BodyColor;
    public string SourceLevel;
    public Sprite Sprite;
    public List<GeneData> Genes;

    [System.Serializable]
    public class GeneData
    {
        public string TraitName;
        public char Allele1;
        public char Allele2;
    }

    public Creature ToCreature()
    {
        List<Gene> geneList = new();
        foreach (var g in Genes)
        {
            geneList.Add(new Gene(g.TraitName, g.Allele1, g.Allele2));
        }

        var creature = new Creature(CreatureName, Gender, geneList, BodyColor);
        creature.SourceLevel = SourceLevel;
        return creature;
    }
}
