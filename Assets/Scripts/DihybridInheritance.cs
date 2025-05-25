using UnityEngine;
using System.Collections.Generic;

public class DihybridInheritance : IBreedingStrategy{
    public Creature Breed(Creature p1, Creature p2){
        List<Gene> childGenes = new List<Gene>();


        if (p1.Genes.ContainsKey("coatcolor") && p2.Genes.ContainsKey("coatcolor")){
            Gene g1 = p1.Genes["coatcolor"];
            Gene g2 = p2.Genes["coatcolor"];
            char allele1 = Random.value < 0.5f ? g1.Allele1 : g1.Allele2;
            char allele2 = Random.value < 0.5f ? g2.Allele1 : g2.Allele2;
            childGenes.Add(new Gene("CoatColor", allele1, allele2));
        }

        if (p1.Genes.ContainsKey("taillength") && p2.Genes.ContainsKey("taillength")){
            Gene g1 = p1.Genes["taillength"];
            Gene g2 = p2.Genes["taillength"];
            char allele1 = Random.value < 0.5f ? g1.Allele1 : g1.Allele2;
            char allele2 = Random.value < 0.5f ? g2.Allele1 : g2.Allele2;
            childGenes.Add(new Gene("TailLength", allele1, allele2));
        }

        string gender = Random.value < 0.5f ? "Male" : "Female";
        string name = "Offspring_" + Random.Range(1000, 9999);
        string bodyColor = p2.BodyColor;

        return new Creature(name, gender, childGenes, bodyColor);
    }
}
