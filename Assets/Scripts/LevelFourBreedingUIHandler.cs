using UnityEngine;

public class LevelFourBreedingUIHandler : IBreedingUIHandler
{
    public void ShowOffspring(BreedingUI ui, Creature offspring)
    {
        string tailType = offspring.GetPhenotype("TailLength").ToLower();
        string bodyColor = offspring.BodyColor.ToLower();

        foreach (GameObject obj in ui.level4OffspringDisplayObjects)
        {
            var meta = obj.GetComponent<DisplayMetadata>();
            if (meta == null)
            {
                obj.SetActive(false);
                continue;
            }

            bool match = meta.BodyColor.ToLower() == bodyColor &&
                         meta.Phenotype.ToLower() == tailType;

            obj.SetActive(match);
        }
    }

    public bool ValidateParents(BreedingUI ui, Creature p1, Creature p2, out string errorMessage)
    {
        if (p1 == p2)
        {
            errorMessage = "Please select two different parents!";
            return false;
        }
        errorMessage = null;
        return true;
    }

    public Sprite GetOffspringSprite(BreedingUI ui)
    {
        return null;
    }
}