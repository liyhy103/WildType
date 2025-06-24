using UnityEngine;
using System.Collections.Generic;

public class DihybridInheritance : IBreedingStrategy
{    
    public Creature Breed(Creature p1, Creature p2)
    {
        List<Gene> childGenes = new List<Gene>();

        // coat color
        if (p1.Genes.TryGetValue(Gene.Traits.CoatColor, out Gene coat1) && p2.Genes.TryGetValue(Gene.Traits.CoatColor, out Gene coat2))
        {
            char allele1 = Random.value < 0.5f ? coat1.Allele1 : coat1.Allele2;
            char allele2 = Random.value < 0.5f ? coat2.Allele1 : coat2.Allele2;
            childGenes.Add(new Gene(Gene.Traits.CoatColor, allele1, allele2));
        }
        else
        {
            Debug.LogWarning("[DihybridInheritance] parents missing CoatColor.");
        }

        // tail length
        if (p1.Genes.TryGetValue(Gene.Traits.TailLength, out Gene tail1) && p2.Genes.TryGetValue(Gene.Traits.TailLength, out Gene tail2))
        {
            char allele1 = Random.value < 0.5f ? tail1.Allele1 : tail1.Allele2;
            char allele2 = Random.value < 0.5f ? tail2.Allele1 : tail2.Allele2;
            childGenes.Add(new Gene(Gene.Traits.TailLength, allele1, allele2));
        }
        else
        {
            Debug.LogWarning("[DihybridInheritance] parents missing TailLength.");
        }
        string gender = Random.value < 0.5f ? "Male" : "Female";
        string name = "Offspring_" + Random.Range(1000, 9999);
        string bodyColor = p2.BodyColor;

        return new Creature(name, gender, childGenes, bodyColor);
    }
}
