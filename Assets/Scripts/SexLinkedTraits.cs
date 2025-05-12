using UnityEngine;
using System.Collections.Generic;

public class SexLinkedBreedingStrategy : IBreedingStrategy
{
    public Creature Breed(Creature p1, Creature p2)
    {
        // Ensure correct parent roles (just in case)
        Creature dad = p1.Gender == "Male" ? p1 : p2;
        Creature mom = p1.Gender == "Female" ? p1 : p2;

        // Get mom’s X alleles (remove Y if present)
        List<char> motherAlleles = new List<char> { mom.CoatColorGene.Allele1, mom.CoatColorGene.Allele2 };
        motherAlleles.RemoveAll(a => a == 'Y');

        if (motherAlleles.Count == 0)
        {
            // This should never happen, but guard against it
            motherAlleles.Add('b');
        }

        char femaleX1 = motherAlleles[0];
        char femaleX2 = motherAlleles.Count > 1 ? motherAlleles[1] : motherAlleles[0];

        // Get dad’s X allele (not Y)
        char dadA = dad.CoatColorGene.Allele1;
        char dadB = dad.CoatColorGene.Allele2;
        char maleX = dadA != 'Y' ? dadA : dadB;

        bool isMale = Random.value < 0.5f;
        string gender = isMale ? "Male" : "Female";
        string name = "Offspring_" + Random.Range(1000, 9999);

        char allele1, allele2;

        if (isMale)
        {
            // Male: X from mom, Y from dad
            allele1 = Random.value < 0.5f ? femaleX1 : femaleX2;
            allele2 = 'Y';
        }
        else
        {
            // Female: X from mom, X from dad
            allele1 = Random.value < 0.5f ? femaleX1 : femaleX2;
            allele2 = maleX;
        }

        Gene shellGene = new Gene("ShellColor", allele1, allele2);
        return new Creature(name, gender, shellGene);
    }
}
