using UnityEngine;

public class LevelOneBreedingUIHandler : IBreedingUIHandler
{
    public void ShowOffspring(BreedingUI ui, Creature offspring)
    {
        ui.greenOffspringDisplay?.SetActive(false);
        ui.yellowOffspringDisplay?.SetActive(false);

        if (offspring.GetPhenotype() == "Green")
        {
            ui.greenOffspringDisplay?.SetActive(true);
        }
        else if (offspring.GetPhenotype() == "Yellow")
        {
            ui.yellowOffspringDisplay?.SetActive(true);
        }
    }

    public bool ValidateParents(BreedingUI ui, Creature p1, Creature p2, out string errorMessage)
    {
        // Level 1 allows any two different parents
        if (p1 == p2)
        {
            errorMessage = "Please select two different parents!";
            return false;
        }

        errorMessage = "";
        return true;
    }
}
