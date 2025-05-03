using UnityEngine;
using System.Collections.Generic;

public class SexLinkedBreedingStrategy : IBreedingStrategy
{
    public Creature Breed(Creature p1, Creature p2)
    {
        // alleles
        char femaleAllele = p2.Genes["coatcolor"].Allele1;  
        char maleAllele = p1.Genes["coatcolor"].Allele1;    

        bool isMale = Random.value < 0.5f;
        string gender = isMale ? "Male" : "Female";
        string name = "Offspring_" + Random.Range(1000, 9999);

        char allele1, allele2;

        if (isMale)
        {
            
            allele1 = femaleAllele;
            allele2 = 'Y';
        }
        else
        {
            allele1 = femaleAllele;
            allele2 = maleAllele;
        }

        Gene childGene = new Gene("CoatColor", allele1, allele2);
        return new Creature(name, gender, new List<Gene>{childGene});
    }
}
