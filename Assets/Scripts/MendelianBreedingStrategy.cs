using UnityEngine;
using System.Collections.Generic;

public class MendelianBreedingStrategy : IBreedingStrategy
{
    public Creature Breed(Creature p1, Creature p2)
    {
        if (!p1.Genes.ContainsKey(Gene.Traits.CoatColor) || !p2.Genes.ContainsKey(Gene.Traits.CoatColor))
        {
            Debug.LogWarning("[Mendelian] Parents are missing CoatColor.");
            return null;
        }

        Gene g1 = p1.Genes[Gene.Traits.CoatColor];
        Gene g2 = p2.Genes[Gene.Traits.CoatColor];

        char allele1 = Random.value < 0.5f ? g1.Allele1 : g1.Allele2;
        char allele2 = Random.value < 0.5f ? g2.Allele1 : g2.Allele2;

        Gene childGene = new Gene(Gene.Traits.CoatColor, allele1, allele2);
        string gender = Random.value < 0.5f ? Creature.GenderMale : Creature.GenderFemale;
        string name = "Offspring_" + Random.Range(1000, 9999);

        var offspring = new Creature(name, gender, new List<Gene> { childGene });
        offspring.BodyColor = offspring.GetPhenotype(Gene.Traits.CoatColor);

        Debug.Log($"[Mendelian] Assigned BodyColor={offspring.BodyColor} to {offspring.CreatureName}");

        return offspring;
    }
}
