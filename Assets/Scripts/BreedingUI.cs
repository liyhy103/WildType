using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class BreedingUI : MonoBehaviour
{
    // UI References
    public TMP_Dropdown Parent1;
    public TMP_Dropdown Parent2;
    public Button BreedButton;
    public TMP_Text OffspringText;
    private List<Creature> Creatures = new List<Creature>();

    void Start()
    {
        Creatures.Add(new Creature("BlueDad", "Male", new Gene("CoatColor", 'B', 'r')));
        Creatures.Add(new Creature("RedMom", "Female", new Gene("CoatColor", 'r', 'r')));

        //Setup UI
        PopulateDropdown(Parent1);
        PopulateDropdown(Parent2);
        BreedButton.onClick.AddListener(OnBreedClicked);
    }

    // Fill the given dropdown with the list of available items
    void PopulateDropdown(TMP_Dropdown dropdown){
        dropdown.ClearOptions(); // clear any existing items
        List<string> names = new List<string>();

        foreach (var creature in Creatures){
            names.Add($"{creature.CreatureName} ({creature.Gender})");
        }
        dropdown.AddOptions(names);
    }

    void OnBreedClicked(){
        int index1 = Parent1.value;
        int index2 = Parent2.value;

        // Avoid selecting the same creature twice
        if (index1 == index2){
            OffspringText.text = "Please select two different parents!";
            return;
        }

        Creature parent1 = Creatures[index1];
        Creature parent2 = Creatures[index2];
        Creature offspring = Breed(parent1, parent2);

        string result = $"Offspring Created!\n" +
                        $"- Name: {offspring.CreatureName}\n" +
                        $"- Gender: {offspring.Gender}\n" +
                        $"- Coat Color: {offspring.GetPhenotype()} [{offspring.GetGenotype()}]";

        OffspringText.text = result;
    }

    Creature Breed(Creature p1, Creature p2){
        // Get coatcolor genes from parents
        Gene g1 = p1.CoatColorGene;
        Gene g2 = p2.CoatColorGene;

        // Randomly select one allele from each parent
        char allele1 = Random.value < 0.5f ? g1.Allele1 : g1.Allele2;
        char allele2 = Random.value < 0.5f ? g2.Allele1 : g2.Allele2;

        Gene childGene = new Gene("CoatColor", allele1, allele2);
        string gender = Random.value < 0.5f ? "Male" : "Female";
        string name = "Offspring_" + Random.Range(1000, 9999);

        return new Creature(name, gender, childGene);
    }
}
