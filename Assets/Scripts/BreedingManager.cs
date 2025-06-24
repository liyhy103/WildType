using UnityEngine;
using System.Collections.Generic;

public class BreedingManager : MonoBehaviour
{
    void Start(){
        // Create two parent creatures with coatColor and tailLength traits
        Gene gene1 = new Gene(Gene.Traits.CoatColor, 'G', 'g');
        Gene gene1_2 = new Gene(Gene.Traits.TailLength, 'L', 'l');
        Gene gene2 = new Gene(Gene.Traits.CoatColor, 'g', 'g');
        Gene gene2_2 = new Gene(Gene.Traits.TailLength, 'l', 'l');

        Creature parent1 = new Creature("Parent1", "Male", new List<Gene> { gene1, gene1_2 });
        Creature parent2 = new Creature("Parent2", "Female", new List<Gene> { gene2, gene2_2 });


        Debug.Log("=== Breeding Test Start ===");
        Debug.Log("Parent 1: " + parent1.GetFullDescription());
        Debug.Log("Parent 2: " + parent2.GetFullDescription());

        // Perform breeding
        Creature offspring = Breed(parent1, parent2);
        Debug.Log("Offspring: " + offspring.GetFullDescription());
        Debug.Log("===============");
    }

        // Breeds two creatures and returns the resulting offspring
        public Creature Breed(Creature parent1, Creature parent2){
            List<Gene> childGenes = new List<Gene>();

            foreach (var trait in parent1.Genes.Keys){
                if (!parent2.Genes.ContainsKey(trait))
                    continue;
                Gene gene1 = parent1.Genes[trait];
                Gene gene2 = parent2.Genes[trait];

                // Randomly select one allele from each parent
                char allele1 = Random.value < 0.5f ? gene1.Allele1 : gene1.Allele2;
                char allele2 = Random.value < 0.5f ? gene2.Allele1 : gene2.Allele2;

                Gene childGene = new Gene(trait, allele1, allele2);
                childGenes.Add(childGene);
            }

            // Randomly assign gender
            string gender = Random.value < 0.5f ? "Male" : "Female";

            // Generate random name for offspring
            string name = "Offspring_" + Random.Range(1000, 9999);

            // Return the new Creature instance
            return new Creature(name, gender, childGenes);
    }
}
