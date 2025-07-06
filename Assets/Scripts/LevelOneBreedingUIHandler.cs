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

        string result = offspring.GetPhenotype(Gene.Traits.CoatColor);

        if (phenotype == "Green")
        {
            ui.greenOffspringDisplay?.SetActive(true);
        }
        else if (phenotype == "Yellow")
        {
            ui.yellowOffspringDisplay?.SetActive(true);
        }
        else
        {
            UnityEngine.Debug.LogWarning($"[LevelOne] Unknown phenotype: {phenotype}");
        }
        if (challenge != null)
        {
            challenge?.SetResult(phenotype, offspring);
            UnityEngine.Debug.Log("[DEBUG] Sent result to challenge: " + result);
        }
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
