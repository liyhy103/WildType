using UnityEngine;

public class Gene
{
    // Name of the trait this gene controls (e.g., "coatColor")
    public string TraitName;

    // Two alleles for this gene (e.g., 'G' and ''g)
    public char Allele1;
    public char Allele2;
    public static class Traits
    {
        public const string CoatColor = "coatColor";
        public const string ShellColor = "shellColor";
        public const string HornLength = "hornLength";
        public const string TailLength = "tailLength";
    }

    public Gene(string traitName, char allele1, char allele2)
    {
        this.TraitName = traitName;
        this.Allele1 = allele1;
        this.Allele2 = allele2;
    }

    // Returns the genotype as a string (e.g., "G/g")
    public string GetGenotype()
    {
        return $"{Allele1}/{Allele2}";
    }

    // Returns the phenotype based on genotype
    public string GetPhenotype()
    {
        if (TraitName == Traits.CoatColor)
        {
            return ResolveCoatColorPhenotype(Allele1, Allele2);           
        }

        if (TraitName == Traits.TailLength)
        { 
            return ResolveTailLengthPhenotype(Allele1, Allele2);            
        }

        if (TraitName == Traits.ShellColor)
        {
            return ResolveShellColorPhenotype(Allele1, Allele2);
        }

        if (TraitName == Traits.HornLength)
        {
            return ResolveHornLengthPhenotype(Allele1, Allele2);
        }

        Debug.LogWarning($"[Gene] Unknown trait: {TraitName}");
        return null;
    }

    // Trait logic for Shell Color
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

    // Trait logic for Horn Length
    private string ResolveHornLengthPhenotype(char allele1, char allele2)
    {
        if (allele1 == 'L' && allele2 == 'L')
            return "Long";
        if ((allele1 == 'L' && allele2 == 'l') || (allele1 == 'l' && allele2 == 'L'))
            return "Short";
        if (allele1 == 'l' && allele2 == 'l')
            return "No";

        Debug.LogWarning($"[Gene] Unknown HornLength genotype: {allele1}/{allele2}");
        return null;
    }



    // Trait logic for Coat Color
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

        Debug.LogWarning($"[Gene] Unknown CoatColor genotype: {allele1}/{allele2}");
        return null;
    }

    // Trait logic for Tail Length
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

        Debug.LogWarning($"[Gene] Unknown TailLength genotype: {allele1}/{allele2}");
        return null;
    }
}
