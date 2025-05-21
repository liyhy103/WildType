using System;
using System.CodeDom.Compiler;
using UnityEngine;
using System.Collections.Generic;

public class IncompleteDominance : IBreedingStrategy
{
    public Creature Breed(Creature p1, Creature p2)
    {
        Gene g1 = p1.Genes["hornlength"];
        Gene g2 = p2.Genes["hornlength"];

        char allele1 = UnityEngine.Random.value < 0.5f ? g1.Allele1 : g1.Allele2;
        char allele2 = UnityEngine.Random.value < 0.5f ? g2.Allele1 : g2.Allele2;

        Gene childGene = new Gene("HornLength", allele1, allele2);
        string gender = UnityEngine.Random.value < 0.5f ? "Male" : "Female";
        string name = "Offspring_" + UnityEngine.Random.Range(1000, 9999);

        string bodyColor = UnityEngine.Random.value < 0.5f ? p1.BodyColor : p2.BodyColor;

        return new Creature(name, gender, new List<Gene> { childGene }, bodyColor);
    }

}


