using UnityEngine;
using UnityEngine.UI;

public class LevelThreeBreedingUIHandler : IBreedingUIHandler
{
    public void ShowOffspring(BreedingUI ui, Creature offspring)
    {
        string phenotype = offspring.GetPhenotype().ToLower();
        string bodyColor = offspring.BodyColor.ToLower();

        foreach (var obj in ui.level3OffspringDisplayObjects)
        {
            var meta = obj.GetComponent<DisplayMetadata>();
            if (meta == null)
            {
                obj.SetActive(false);
                continue;
            }

            bool match = meta.Phenotype.ToLower() == phenotype &&
                         meta.BodyColor.ToLower() == bodyColor;


            obj.SetActive(match);
            Debug.Log($"[Offspring Match Test] {obj.name} - Match: {match} | Phenotype: {meta.Phenotype}/{phenotype}, Color: {meta.BodyColor}/{bodyColor}");

        }
    }

    public bool ValidateParents(BreedingUI ui, Creature p1, Creature p2, out string errorMessage)
    {
        errorMessage = "";
        return true;
    }

    public Sprite GetOffspringSprite(BreedingUI ui)
    {
        foreach (var obj in ui.level3OffspringDisplayObjects)
        {
            if (obj.activeSelf)
                return obj.GetComponent<Image>().sprite;
        }
        return null;
    }

}
