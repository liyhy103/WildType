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
            return ResolveCoatColorPhenotype(Allele1, Allele2);

        if (TraitName.ToLower() == "taillength")
            return ResolveTailLengthPhenotype(Allele1, Allele2);
  
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
        if ((a1 == 'L' && a2 == 'l') || (a1 == 'S' && a2 == 'L')) return "Short";
        if (a1 == 'l' && a2 == 'l') return "No";
        return "Unknown";
    }




    private string ResolveCoatColorPhenotype(char allele1, char allele2)
    {
        string genotype = $"{allele1}/{allele2}";
        string reverseGenotype = $"{allele2}/{allele1}";

      
        // Green / Yellow
        if (genotype == "G/G")
            return "Green";
        if (genotype == "G/g" || reverseGenotype == "G/g")
            return "Green";
        if (genotype == "g/g")
            return "Yellow";

        return "Unknown";
    }

    // Trait-specific logic for Tail Length
    private string ResolveTailLengthPhenotype(char allele1, char allele2)
    {
        string genotype = $"{allele1}/{allele2}";
        string reverseGenotype = $"{allele2}/{allele1}";

        if (genotype == "L/L") 
            return "Long";
        if (genotype == "L/l" || reverseGenotype == "L/l") 
            return "Long";
        if (genotype == "l/l") 
            return "Short";

        return "Unknown";
    }

}
