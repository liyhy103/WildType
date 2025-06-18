using UnityEngine;
using System.Collections.Generic;

public class SexLinkedBreedingStrategy : IBreedingStrategy
{
    public Creature Breed(Creature p1, Creature p2)
    {
        Creature dad = p1.Gender == "Male" ? p1 : p2;
        Creature mom = p1.Gender == "Female" ? p1 : p2;

        List<char> motherAlleles = new List<char> { mom.Genes["shellcolor"].Allele1, mom.Genes["shellcolor"].Allele2 };
        motherAlleles.RemoveAll(a => a == 'Y');

        if (motherAlleles.Count == 0)
        {
            motherAlleles.Add('b');
        }

        char femaleX1 = motherAlleles[0];
        char femaleX2 = motherAlleles.Count > 1 ? motherAlleles[1] : motherAlleles[0];

        char dadA = dad.Genes["shellcolor"].Allele1;
        char dadB = dad.Genes["shellcolor"].Allele2;
        char maleX = dadA != 'Y' ? dadA : dadB;

        bool isMale = Random.value < 0.5f;
        string gender = isMale ? "Male" : "Female";
        string name = "Offspring_" + Random.Range(1000, 9999);

        char allele1, allele2;

        if (isMale)
        {
            allele1 = Random.value < 0.5f ? femaleX1 : femaleX2;
            allele2 = 'Y';
        }
        else
        {
            allele1 = Random.value < 0.5f ? femaleX1 : femaleX2;
            allele2 = maleX;
        }

        Gene shellGene = new Gene("ShellColor", allele1, allele2);
        return new Creature(name, gender, new List<Gene>{shellGene});
    }
}
