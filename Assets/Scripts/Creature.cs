using System.Collections.Generic;

public class Creature
{
    public const string GenderMale = "male";
    public const string GenderFemale = "female";

    public const string Tutorial = "TutorialLevel";
    public const string LevelOne = "LevelOne";
    public const string LevelTwo = "LevelTwo";
    public const string LevelThree = "LevelThree";
    public const string LevelFour = "LevelFour";

    public string CreatureName;
    public string Gender;
    public Dictionary<string, Gene> Genes;
    public string BodyColor;
    public string SourceLevel;

    public Creature(string name, string gender, List<Gene> geneList, string bodyColor = "")
    {
        this.CreatureName = name;
        this.Gender = gender;
        Genes = new Dictionary<string, Gene>();

        foreach (Gene gene in geneList)
        {
            if (!Genes.ContainsKey(gene.TraitName))
            {
                Genes.Add(gene.TraitName, gene);
            }
            else
            {
                UnityEngine.Debug.LogWarning($"[Creature] Duplicate gene for trait: {gene.TraitName} in {name}");
            }
        }

        this.BodyColor = string.IsNullOrEmpty(bodyColor) ? "unknown" : bodyColor;
    }

    public string GetGenotype(string trait)
    {
        if (Genes.TryGetValue(trait, out Gene gene))
        {
            return gene.GetGenotype();
        }

        UnityEngine.Debug.LogWarning($"[Creature] Trait '{trait}' not found in creature '{CreatureName}'");
        return null;
    }

    public string GetPhenotype(string trait)
    {
        if (Genes.TryGetValue(trait, out Gene gene))
        {
            return gene.GetPhenotype();
        }

        UnityEngine.Debug.LogWarning($"[Creature] Trait '{trait}' not found in creature '{CreatureName}'");
        return null;
    }

    public string GetFullDescription()
    {
        string description = $"{CreatureName} ({Gender})";

        foreach (var pair in Genes)
        {
            string trait = pair.Key;
            string genotype = pair.Value.GetGenotype();
            string phenotype = pair.Value.GetPhenotype();

            description += $"\n- {trait}: {phenotype} [{genotype}]";
        }

        return description;
    }

    public bool HasPhenotype(string trait, string phenotypeConstant)
    {
        return GetPhenotype(trait) == phenotypeConstant;
    }

    public bool HasBodyColor(string colorConstant)
    {
        return BodyColor == colorConstant;
    }

    public override bool Equals(object obj)
    {
        if (obj is not Creature other) return false;
        return CreatureName == other.CreatureName &&
               Gender == other.Gender &&
               SourceLevel == other.SourceLevel;
    }

    public override int GetHashCode()
    {
        return (CreatureName + Gender + SourceLevel).GetHashCode();
    }
}
