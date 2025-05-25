using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;


public class LevelOneBreedingUIHandler : IBreedingUIHandler
{
    private Challenge challenge;

    public void Awake()
    {
        challenge = GameObject.FindObjectOfType<Challenge>();
        if (challenge == null)
        {
            UnityEngine.Debug.LogError("Challenge instance not found in scene!");
        }
    }
    public LevelOneBreedingUIHandler()
    {
        challenge = GameObject.FindObjectOfType<Challenge>();

    }

    public void ShowOffspring(BreedingUI ui, Creature offspring)
    {
        ui.greenOffspringDisplay?.SetActive(false);
        ui.yellowOffspringDisplay?.SetActive(false);

        if (offspring.GetPhenotype("coatcolor") == "Green")
        {
            challenge.SetResult(offspring.GetPhenotype(), offspring);
            ui.greenOffspringDisplay?.SetActive(true);
        }
        else if (offspring.GetPhenotype("coatcolor") == "Yellow")
        {
            challenge.SetResult(offspring.GetPhenotype(), offspring);
            ui.yellowOffspringDisplay?.SetActive(true);
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
