using UnityEngine;
using System.Collections.Generic;

public class IncompleteDominance : IBreedingStrategy
{
    public Creature Breed(Creature p1, Creature p2)
    {
        if (!p1.Genes.TryGetValue(Gene.Traits.HornLength, out Gene g1) ||
            !p2.Genes.TryGetValue(Gene.Traits.HornLength, out Gene g2))
        {
            Debug.LogWarning("[IncompleteDominance] Parents missing HornLength gene.");
            return null;
        }

        char allele1 = Random.value < 0.5f ? g1.Allele1 : g1.Allele2;
        char allele2 = Random.value < 0.5f ? g2.Allele1 : g2.Allele2;

        Gene childGene = new Gene(Gene.Traits.HornLength, allele1, allele2);
        string gender = Random.value < 0.5f ? Creature.GenderMale : Creature.GenderFemale;
        string name = "Offspring_" + Random.Range(1000, 9999);
        string bodyColor = Random.value < 0.5f ? p1.BodyColor : p2.BodyColor;

        return new Creature(name, gender, new List<Gene> { childGene }, bodyColor);
    }
}
