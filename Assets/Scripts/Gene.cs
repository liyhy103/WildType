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
            return "Pink"; ;

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

        // Orange / Green (O/g or g/g)
        if (genotype == "O/O")
            return "Orange";
        if (genotype == "O/g" || reverseGenotype == "O/g")
            return "Orange";
        if (genotype == "g/g")
            return "Green";

        // Brown / Orange (N/o)
        if (genotype == "N/N")
            return "Brown";
        if (genotype == "N/o" || reverseGenotype == "N/o")
            return "Brown";
        if (genotype == "o/o")
            return "Orange";

        // Orange / Yellow (O/y)
        if (genotype == "O/y" || reverseGenotype == "O/y")
            return "Orange";
        if (genotype == "y/y")
            return "Yellow";

        // Brown / Yellow (N/y)
        if (genotype == "N/y" || reverseGenotype == "N/y")
            return "Yellow";

        // Green / Brown (G/n)
        if (genotype == "G/n" || reverseGenotype == "G/n")
            return "Green";

        // Brown (n/n)
        if (genotype == "n/n")
            return "Brown";

        return "Unknown";
    }
}
