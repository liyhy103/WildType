using UnityEngine;
using UnityEngine.UI;


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


        ui.greenOffspringDisplay?.SetActive(false);
        ui.yellowOffspringDisplay?.SetActive(false);
        ui.greenLightShellDisplay?.SetActive(false);
        ui.yellowLightShellDisplay?.SetActive(false);

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

    public Sprite GetOffspringSprite(BreedingUI ui)
    {
        if (ui.greenOffspringDisplay.activeSelf)
            return ui.greenOffspringDisplay.GetComponent<Image>().sprite;
        if (ui.yellowOffspringDisplay.activeSelf)
            return ui.yellowOffspringDisplay.GetComponent<Image>().sprite;
        if (ui.greenLightShellDisplay.activeSelf)
            return ui.greenLightShellDisplay.GetComponent<Image>().sprite;
        if (ui.yellowLightShellDisplay.activeSelf)
            return ui.yellowLightShellDisplay.GetComponent<Image>().sprite;
        return null;
    }

}
