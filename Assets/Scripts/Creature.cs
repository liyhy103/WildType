using UnityEngine;

public class Creature
{
    // Name of the creature
    public string CreatureName;

    // Gender of the creature ("Male" or "Female")
    public string Gender;

    // Gene controlling coat color (Mendelian inheritance)
    public Gene CoatColorGene;

    //Gene controlling horn length (Incomplete Dominance inheritance)
    public Gene HornLengthGene;

    public Creature(string name, string gender, Gene coatGene){
        this.CreatureName = name;
        this.Gender = gender;
        this.CoatColorGene = coatGene;
    }

    // Returns the genotype from the gene
    public string GetGenotype(){
        return CoatColorGene.GetGenotype();
    }

    // Returns the phenotype from the gene
    public string GetPhenotype(){
        return CoatColorGene.GetPhenotype();
    }

    // Full description of the creature including phenotype and genotype
    public string GetFullDescription(){
        return $"{CreatureName} ({Gender}) - Coat: {GetPhenotype()} [{GetGenotype()}]";
    }
}