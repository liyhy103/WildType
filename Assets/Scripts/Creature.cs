using UnityEngine;
using System.Collections.Generic;

public class Creature
{
    public string CreatureName;
    public string Gender;
    // Store multiple traits, e.g., 'coatcolor', 'taillength'
    public Dictionary<string, Gene> Genes;
    public string BodyColor;
    public string SourceLevel;

    public Creature(string name, string gender, List<Gene> geneList, string bodyColor = "")
    {
        this.CreatureName = name;
        this.Gender = gender;
        Genes = new Dictionary<string, Gene>();

        // Loop through the geneList to add into the Genes dictionary
        foreach (Gene gene in geneList)
        {
            if (!Genes.ContainsKey(gene.TraitName))
            {
                Genes.Add(gene.TraitName, gene);
            }
            else
            {
                Debug.LogWarning($"[Creature] Duplicate gene for trait: {gene.TraitName} in {name}");
            }
        }

        this.BodyColor = string.IsNullOrEmpty(bodyColor) ? "Unknown" : bodyColor;
    }

    // Returns the genotype from the gene
    public string GetGenotype(string trait)
    {
        if (Genes.TryGetValue(trait, out Gene gene))
        {
            return gene.GetGenotype();
        }

        Debug.LogWarning($"[Creature] Trait '{trait}' not found in creature '{CreatureName}'");
        return null;
    }

    // Returns the phenotype from the gene
    public string GetPhenotype(string trait)
    {
        if (Genes.TryGetValue(trait, out Gene gene))
        {
            return gene.GetPhenotype();
        }

        Debug.LogWarning($"[Creature] Trait '{trait}' not found in creature '{CreatureName}'");
        return null;
    }

    // Full description of the creature including phenotype and genotype
    public string GetFullDescription()
    {
        string descripition = $"{CreatureName} ({Gender})";

        // Loop through all traits in the Genes dictionary
        foreach (var pair in Genes)
        {
            string trait = pair.Key; // Get the trait name（e.g., 'coatColor'）
            string genotype = pair.Value.GetGenotype(); // Get the genotype (e.g., 'G/g')
            string phenotype = pair.Value.GetPhenotype(); // Get the phenotype (e.g., 'Green')

            descripition += $"\n- {trait}: {phenotype} [{genotype}]";
        }

        return descripition;
    }

    public override bool Equals(object obj)
    {
        if (obj is not Creature other) return false;
        return CreatureName == other.CreatureName &&
               Gender == other.Gender &&
               SourceLevel == other.SourceLevel;
    }

    public override int GetHashCode()
    {
        return (CreatureName + Gender + SourceLevel).GetHashCode();
    }
}
