using System.Diagnostics;
using UnityEngine;

public class Gene
{
    // Name of the trait this gene controls (e.g., "CoatColor")
    public string TraitName;

    // Two alleles for this gene (e.g., 'B' and 'b')
    public char Allele1;
    public char Allele2;


    public Gene(string traitName, char allele1, char allele2)
    {
        this.TraitName = traitName;
        this.Allele1 = allele1;
        this.Allele2 = allele2;
    }

    // Returns the genotype as a string (e.g., "B/b")
    public string GetGenotype()
    {
        return $"{Allele1}/{Allele2}";
    }

    // Returns the phenotype based on genotype (for lesson 1 Mendelian)
    public string GetPhenotype()
    {
        if (TraitName.ToLower() == "coatcolor")
        {
            return ResolveCoatColorPhenotype(Allele1, Allele2);
        }

        if(TraitName.ToLower() == "hornlength")
        {
            return ResolveHornLengthPhenotype(Allele1, Allele2);
        }

        return "Unknown";
    }

    private string ResolveHornLengthPhenotype(char allele1, char allele2)
    {

        UnityEngine.Debug.Log($"[Debug] Allele1: {allele1}, Allele2: {allele2}");

        allele1 = char.ToUpper(allele1);
        allele2 = char.ToUpper(allele2);

        if (allele1 == 'L' && allele2 == 'L')
            return "Long";
        if (allele1 == 'S' && allele2 == 'S')
            return "Short";
        if ((allele1 == 'S' && allele2 == 'L') || (allele1 == 'L' && allele2 == 'S'))
            return "Medium";
        return "Unknown";
    }

    private string ResolveCoatColorPhenotype(char allele1, char allele2)
    {
        string genotype = $"{allele1}/{allele2}";
        string reverseGenotype = $"{allele2}/{allele1}";

        if ((allele1 == 'P' && allele2 == 'Y') || (allele1 == 'Y' && allele2 == 'P'))
            return "Pink";  // Male Pink

        if ((allele1 == 'B' && allele2 == 'Y') || (allele1 == 'Y' && allele2 == 'B'))
            return "Blue";  // Male Blue

        if (allele1 == 'P' && allele2 == 'P')
            return "Pink";  // Female Pink

        if (allele1 == 'B' && allele2 == 'B')
            return "Blue";  // Female Blue

        if ((allele1 == 'P' && allele2 == 'B') || (allele1 == 'B' && allele2 == 'P'))
            return "Pink"; 

        // Green / Yellow
        if (genotype == "G/G")
           return "Green";
        if (genotype == "G/y" || reverseGenotype == "G/y")
            return "Green";
        if (genotype == "y/y")
            return "Yellow";


        return "Unknown";
    }
}
