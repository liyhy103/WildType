using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;


public class LevelOneBreedingUIHandler : IBreedingUIHandler
{
    private LevelOneChallenge challenge;

    public LevelOneBreedingUIHandler(LevelOneChallenge challenge)
    {
        this.challenge = challenge;
    }

    public void ShowOffspring(BreedingUI ui, Creature offspring)
    {
        ui.greenOffspringDisplay?.SetActive(false);
        ui.yellowOffspringDisplay?.SetActive(false);

        string phenotype = offspring.GetPhenotype(Gene.Traits.CoatColor);

        if (string.IsNullOrEmpty(offspring.BodyColor))
        {
            offspring.BodyColor = phenotype;
            UnityEngine.Debug.Log($"[LevelOne] Assigned BodyColor={offspring.BodyColor} based on phenotype={phenotype}");
        }

        if (phenotype == Gene.Phenotypes.Green)
        {
            ui.greenOffspringDisplay?.SetActive(true);
        }
        else if (phenotype == Gene.Phenotypes.Yellow)
        {
            ui.yellowOffspringDisplay?.SetActive(true);
        }
        else
        {
            UnityEngine.Debug.LogWarning($"[LevelOne] Unknown phenotype: {phenotype}");
        }

        challenge?.SetResult(phenotype, offspring);
        UnityEngine.Debug.Log("[DEBUG] Sent result to challenge: " + phenotype);
    }




    public bool ValidateParents(BreedingUI ui, Creature p1, Creature p2, out string errorMessage)
    {
        if (p1 == p2)
        {
            errorMessage = "Please select two different parents!";
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
        return null;
    }

}
