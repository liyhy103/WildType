using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class BreedingUI : MonoBehaviour
{
    // UI References
    public TMP_Dropdown Parent1;
    public TMP_Dropdown Parent2;
    public Button BreedButton;
    
    public GameObject[] parent1DisplayObjects;
    public GameObject[] parent2DisplayObjects;

    public GameObject greenOffspringDisplay;
    public GameObject yellowOffspringDisplay;

    public GameObject heartEffectPrefab;
    public Transform heartSpawn1;
    public Transform heartSpawn2;
    public float heartDuration = 2f;

    public TMP_Text OffspringText;
    private List<Creature> Creatures = new List<Creature>();

    void Start()
    {
        Creatures.Add(new Creature("GreenDad", "Male", new Gene("CoatColor", 'G', 'y')));
        Creatures.Add(new Creature("YellowMom", "Female", new Gene("CoatColor", 'y', 'y')));

        //Setup UI
        PopulateDropdown(Parent1);
        PopulateDropdown(Parent2);

        Parent1.onValueChanged.AddListener(UpdateCreatureDisplayParent1);
        Parent2.onValueChanged.AddListener(UpdateCreatureDisplayParent2);
        BreedButton.onClick.AddListener(OnBreedClicked);

        UpdateCreatureDisplayParent1(Parent1.value);
        UpdateCreatureDisplayParent2(Parent2.value);

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

    void UpdateCreatureDisplayParent1(int index)
    {
        for (int i = 0; i < parent1DisplayObjects.Length; i++)
        {
            parent1DisplayObjects[i].SetActive(i == index);
        }
    }

    void UpdateCreatureDisplayParent2(int index)
    {
        for (int i = 0; i < parent2DisplayObjects.Length; i++)
        {
            parent2DisplayObjects[i].SetActive(i == index);
        }
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

        PlayHeartEffect();

        OffspringText.text = result;

        string phenotype = offspring.GetPhenotype();

        greenOffspringDisplay.SetActive(false);
        yellowOffspringDisplay.SetActive(false);

        if (phenotype == "Green")
        {
            greenOffspringDisplay.SetActive(true);
        }
        else if (phenotype == "Yellow")
        {
            yellowOffspringDisplay.SetActive(true);
        }

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


    void PlayHeartEffect()
    {
        Instantiate(heartEffectPrefab, heartSpawn1.position, Quaternion.identity, heartSpawn1);
        Instantiate(heartEffectPrefab, heartSpawn2.position, Quaternion.identity, heartSpawn2);
    }
}
