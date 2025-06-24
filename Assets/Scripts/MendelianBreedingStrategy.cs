using UnityEngine;
using System.Collections.Generic;

public class MendelianBreedingStrategy : IBreedingStrategy
{
    public Creature Breed(Creature p1, Creature p2)
    {
        if (!p1.Genes.ContainsKey(Gene.Traits.CoatColor) || !p2.Genes.ContainsKey(Gene.Traits.CoatColor))
        {
            Debug.LogWarning("[Mendelian]parents are missing CoatColor.");
            return null;
        }

        Gene g1 = p1.Genes[Gene.Traits.CoatColor];
        Gene g2 = p2.Genes[Gene.Traits.CoatColor];

        // Randomly choose one allele from each parent
        char allele1 = Random.value < 0.5f ? g1.Allele1 : g1.Allele2;
        char allele2 = Random.value < 0.5f ? g2.Allele1 : g2.Allele2;

        // Create offspring gene and assign gender and name
        Gene childGene = new Gene(Gene.Traits.CoatColor, allele1, allele2);
        string gender = Random.value < 0.5f ? "Male" : "Female";
        string name = "Offspring_" + Random.Range(1000, 9999);

        return new Creature(name, gender, new List<Gene>{childGene});
    }
}
