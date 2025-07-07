using UnityEngine;
using System.Collections.Generic;

public class SexLinkedBreedingStrategy : IBreedingStrategy
{
    public Creature Breed(Creature p1, Creature p2)
    {
        Creature dad = p1.Gender == Creature.GenderMale ? p1 : p2;
        Creature mom = p1.Gender == Creature.GenderFemale ? p1 : p2;

        if (!dad.Genes.TryGetValue(Gene.Traits.ShellColor, out Gene dadGene) ||
            !mom.Genes.TryGetValue(Gene.Traits.ShellColor, out Gene momGene))
        {
            Debug.LogWarning("[SexLinked] One or both parents missing ShellColor gene.");
            return null;
        }

        List<char> motherAlleles = new List<char> { momGene.Allele1, momGene.Allele2 };
        motherAlleles.RemoveAll(a => a == 'Y');
        if (motherAlleles.Count == 0) motherAlleles.Add('b');

        char femaleX1 = motherAlleles[0];
        char femaleX2 = motherAlleles.Count > 1 ? motherAlleles[1] : motherAlleles[0];

        char dadA = dadGene.Allele1;
        char dadB = dadGene.Allele2;
        char maleX = dadA != 'Y' ? dadA : dadB;

        bool isMale = Random.value < 0.5f;
        string gender = isMale ? Creature.GenderMale : Creature.GenderFemale;
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

        Gene shellGene = new Gene(Gene.Traits.ShellColor, allele1, allele2);
        string bodyColor = Gene.Phenotypes.Green;

        return new Creature(name, gender, new List<Gene> { shellGene }, bodyColor);
    }
}
