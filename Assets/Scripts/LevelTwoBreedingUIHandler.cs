using UnityEngine;

public class LevelTwoBreedingUIHandler : IBreedingUIHandler
{
    public void HandleBreed(Creature p1, Creature p2, BreedingUI context)
    {
        throw new System.NotImplementedException();
    }

    public void ShowOffspring(BreedingUI ui, Creature offspring)
    {
        string phenotype = offspring.GetPhenotype();
        string gender = offspring.Gender;

        // Hide all first
        ui.greenOffspringDisplay?.SetActive(false);
        ui.yellowOffspringDisplay?.SetActive(false);
        ui.greenLightShellDisplay?.SetActive(false);
        ui.yellowLightShellDisplay?.SetActive(false);

        // Show based on phenotype and gender
        if (phenotype == "Dark" && gender == "Male")
        {
            ui.greenOffspringDisplay?.SetActive(true);
        }
        else if (phenotype == "Dark" && gender == "Female")
        {
            ui.yellowOffspringDisplay?.SetActive(true);
        }
        else if (phenotype == "Light" && gender == "Male")
        {
            ui.greenLightShellDisplay?.SetActive(true);
        }
        else if (phenotype == "Light" && gender == "Female")
        {
            ui.yellowLightShellDisplay?.SetActive(true);
        }
    }

    public bool ValidateParents(BreedingUI ui, Creature p1, Creature p2, out string errorMessage)
    {
        if (p1.Gender == p2.Gender)
        {
            errorMessage = "Breeding requires one male and one female parent!";
            return false;
        }

        errorMessage = "";
        return true;
    }
}
