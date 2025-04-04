using UnityEngine;

public class BreedingManager : MonoBehaviour
{
    void Start(){
        // Create two parent creatures with predefined genotypes
        Gene gene1 = new Gene("CoatColor", 'B', 'r');
        Gene gene2 = new Gene("CoatColor", 'r', 'r');

        Creature parent1 = new Creature("Parent1", "Male", gene1);
        Creature parent2 = new Creature("Parent2", "Female", gene2);

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
            // Randomly select one allele from each parent
            char allele1 = Random.value < 0.5f ? parent1.CoatColorGene.Allele1 : parent1.CoatColorGene.Allele2;
            char allele2 = Random.value < 0.5f ? parent2.CoatColorGene.Allele1 : parent2.CoatColorGene.Allele2;

            // Create new gene for offspring
            Gene childGene = new Gene("CoatColor", allele1, allele2);

            // Randomly assign gender
            string gender = Random.value < 0.5f ? "Male" : "Female";

            // Generate random name for offspring
            string name = "Offspring_" + Random.Range(1000, 9999);

            // Return the new Creature instance
            return new Creature(name, gender, childGene);
    }
}
