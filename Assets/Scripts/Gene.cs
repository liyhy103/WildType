using UnityEngine;

public class Gene
{
    public string TraitName;
    public char Allele1;
    public char Allele2;

    public static class Traits
    {
        public const string CoatColor = "coatColor";
        public const string ShellColor = "shellColor";
        public const string HornLength = "hornLength";
        public const string TailLength = "tailLength";
    }

    public static class Phenotypes
    {
        public const string Green = "Green";
        public const string Yellow = "Yellow";

        public const string Light = "Light";
        public const string Dark = "Dark";

        public const string Long = "Long";
        public const string Short = "Short";
        public const string No = "No";
    }

    public Gene(string traitName, char allele1, char allele2)
    {
        this.TraitName = traitName;
        this.Allele1 = allele1;
        this.Allele2 = allele2;
    }

    public string GetGenotype()
    {
        return $"{Allele1}/{Allele2}";
    }

    public string GetPhenotype()
    {
        return TraitName switch
        {
            var t when t == Traits.CoatColor => ResolveCoatColorPhenotype(Allele1, Allele2),
            var t when t == Traits.TailLength => ResolveTailLengthPhenotype(Allele1, Allele2),
            var t when t == Traits.ShellColor => ResolveShellColorPhenotype(Allele1, Allele2),
            var t when t == Traits.HornLength => ResolveHornLengthPhenotype(Allele1, Allele2),
            _ => WarnUnknownTrait()
        };
    }

    private string WarnUnknownTrait()
    {
        Debug.LogWarning($"[Gene] Unknown trait: {TraitName}");
        return null;
    }

    private string ResolveShellColorPhenotype(char allele1, char allele2)
    {
        return (allele1 == 'B' || allele2 == 'B') ? Phenotypes.Dark : Phenotypes.Light;
    }

    private string ResolveHornLengthPhenotype(char allele1, char allele2)
    {
        if (allele1 == 'L' && allele2 == 'L')
            return Phenotypes.Long;
        if ((allele1 == 'L' && allele2 == 'l') || (allele1 == 'l' && allele2 == 'L'))
            return Phenotypes.Short;
        if (allele1 == 'l' && allele2 == 'l')
            return Phenotypes.No;

        Debug.LogWarning($"[Gene] Unknown HornLength genotype: {allele1}/{allele2}");
        return null;
    }

    private string ResolveCoatColorPhenotype(char allele1, char allele2)
    {
        string genotype = $"{allele1}/{allele2}";
        string reverseGenotype = $"{allele2}/{allele1}";

        if (genotype == "G/G" || genotype == "G/g" || reverseGenotype == "G/g")
            return Phenotypes.Green;
        if (genotype == "g/g")
            return Phenotypes.Yellow;

        Debug.LogWarning($"[Gene] Unknown CoatColor genotype: {allele1}/{allele2}");
        return null;
    }

    private string ResolveTailLengthPhenotype(char allele1, char allele2)
    {
        string genotype = $"{allele1}/{allele2}";
        string reverseGenotype = $"{allele2}/{allele1}";

        if (genotype == "L/L" || genotype == "L/l" || reverseGenotype == "L/l")
            return Phenotypes.Long;
        if (genotype == "l/l")
            return Phenotypes.Short;

        Debug.LogWarning($"[Gene] Unknown TailLength genotype: {allele1}/{allele2}");
        return null;
    }
}
