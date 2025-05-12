using System;
using System.CodeDom.Compiler;
using UnityEngine;

public class IncompleteDominance : IBreedingStrategy
{
    public Creature Breed(Creature p1, Creature p2)
    {
        Gene g1 = p1.CoatColorGene;
        Gene g2 = p2.CoatColorGene;

        char allele1 = UnityEngine.Random.value < 0.5f ? g1.Allele1 : g1.Allele2;
        char allele2 = UnityEngine.Random.value < 0.5f ? g2.Allele1 : g2.Allele2;

        Gene childGene = new Gene("hornlength", allele1, allele2);
        string gender = UnityEngine.Random.value < 0.5f ? "Male" : "Female";
        string name = "Offspring_" + UnityEngine.Random.Range(1000, 9999);

        return new Creature(name, gender, childGene);

    }
}


