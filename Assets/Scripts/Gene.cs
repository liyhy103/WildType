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

        if (TraitName.ToLower() == "shellcolor")
        {
            return ResolveShellColorPhenotype(Allele1, Allele2);
        }

        if (TraitName.ToLower() == "hornlength")
        {
            return ResolveHornLengthPhenotype(Allele1, Allele2);
        }


        return "Unknown";
    }

    private string ResolveShellColorPhenotype(char allele1, char allele2)
    {
        // XB = 'B', Xb = 'b', Y = 'Y'
        if (allele1 == 'Y' || allele2 == 'Y') //Male
        {
            return (allele1 == 'B' || allele2 == 'B') ? "Dark" : "Light";
        }
        else // Female
        {
            return (allele1 == 'B' || allele2 == 'B') ? "Dark" : "Light";
        }
    }
    private string ResolveHornLengthPhenotype(char a1, char a2)
    {
        if (a1 == 'L' && a2 == 'L') return "Long";
        if ((a1 == 'L' && a2 == 'S') || (a1 == 'S' && a2 == 'L')) return "Medium";
        if (a1 == 'S' && a2 == 'S') return "Short";
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

        // Blue / Red
        if (genotype == "B/B")
            return "Blue";
        if (genotype == "B/r" || reverseGenotype == "B/r")
            return "Blue";
        if (genotype == "r/r")
            return "Red";

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
