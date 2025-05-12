using UnityEngine;

public class Creature
{
    public string CreatureName;
    public string Gender;
    public Gene CoatColorGene;
    public string BodyColor;

    public Creature(string name, string gender, Gene coatGene, string bodyColor = "")
    {
        this.CreatureName = name;
        this.Gender = gender;
        this.CoatColorGene = coatGene;

        // Automatically set BodyColor to match phenotype if not explicitly given
        this.BodyColor = string.IsNullOrEmpty(bodyColor) ? coatGene.GetPhenotype() : bodyColor;
    }

    public string GetGenotype() => CoatColorGene.GetGenotype();
    public string GetPhenotype() => CoatColorGene.GetPhenotype();

    public string GetFullDescription()
    {
        return $"{CreatureName} ({Gender}) - Coat: {GetPhenotype()} [{GetGenotype()}]";
    }
}
