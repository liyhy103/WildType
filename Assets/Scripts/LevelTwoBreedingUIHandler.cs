using UnityEngine;
using UnityEngine.UI;

public class LevelTwoBreedingUIHandler : IBreedingUIHandler
{
    private LevelTwoChallenge challenge;

    public LevelTwoBreedingUIHandler(LevelTwoChallenge challenge)
    {
        this.challenge = challenge;
    }

    public void ShowOffspring(BreedingUI ui, Creature offspring)
    {
        if (ui == null || offspring == null)
        {
            Debug.LogError("[LevelTwo] BreedingUI or Offspring is null!");
            return;
        }

        string phenotype = offspring.GetPhenotype(Gene.Traits.ShellColor);
        string gender = offspring.Gender;

        // Reset all displays
        ui.greenOffspringDisplay?.SetActive(false);
        ui.greenLightShellDisplay?.SetActive(false);

        Debug.Log($"[LevelTwo] Offspring: Gender={gender}, Phenotype={phenotype}");

        if (phenotype == Gene.Phenotypes.Dark)
        {
            ui.greenOffspringDisplay?.SetActive(true);
            Debug.Log("[LevelTwo] Showing greenOffspringDisplay (Dark Shell)");
        }
        else if (phenotype == Gene.Phenotypes.Light)
        {
            ui.greenLightShellDisplay?.SetActive(true);
            Debug.Log("[LevelTwo] Showing greenLightShellDisplay (Light Shell)");
        }
        else
        {
            Debug.LogWarning($"[LevelTwo] Unrecognized phenotype: {phenotype}");
        }

        challenge?.SetResult(phenotype, gender, offspring);
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
        if (ui.greenLightShellDisplay.activeSelf)
            return ui.greenLightShellDisplay.GetComponent<Image>().sprite;

        return null;
    }
}
