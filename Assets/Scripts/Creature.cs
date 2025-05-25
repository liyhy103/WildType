using UnityEngine;
using System.Collections.Generic;

public class Creature
{
    public string CreatureName;
    public string Gender;
    // Store multiple traits, e.g., 'coatcolor', 'hairtype'
    public Dictionary<string, Gene> Genes;
    public string BodyColor;

    public Creature(string name, string gender, List<Gene> geneList, string bodyColor = "")
    {
        this.CreatureName = name;
        this.Gender = gender;
        Genes = new Dictionary<string, Gene>();

        foreach (Gene gene in geneList)
        {
            // Store trait name exactly as passed in
            Genes[gene.TraitName.ToLower()] = gene;
        }

        this.BodyColor = string.IsNullOrEmpty(bodyColor) ? "Unknown" : bodyColor;
    }

    // Returns the genotype from the gene
    public string GetGenotype(string trait){
        trait = trait.ToLower();
        return Genes.ContainsKey(trait) // Check if the trait is included 
                ? Genes[trait].GetGenotype() // If yes, call Gene's GetGenotype() method 
                : "N/A"; // If the trait does not exist, return "N/A".
    }

    // Returns the phenotype from the gene
    public string GetPhenotype(string trait){
        trait = trait.ToLower();
        return Genes.ContainsKey(trait) // Check if the trait is included 
                ? Genes[trait].GetPhenotype() // If yes, call Gene's GetPhenotype() method 
                : "N/A"; // If the trait does not exist, return "N/A".
    }

    // Full description of the creature including phenotype and genotype
    public string GetFullDescription(){
        string descripition = $"{CreatureName} ({Gender})";

        // Loop through all traits in the Genes dictionary
        foreach (var pair in Genes){
            string trait = pair.Key; // Get the trait name（e.g., 'coatcolor'）
            string genotype = pair.Value.GetGenotype(); // Get the genotype (e.g., 'B/b')
            string phenotype = pair.Value.GetPhenotype(); // Get the phenotype (e.g., 'Blue')

            descripition += $"\n- {trait}: {phenotype} [{genotype}]";
        }

        return descripition;
    }
}
