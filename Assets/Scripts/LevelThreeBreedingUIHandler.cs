using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class LevelThreeBreedingUIHandler : IBreedingUIHandler
{

    private LevelThreeChallenges challenge;

    public LevelThreeBreedingUIHandler(LevelThreeChallenges challenge)
    {
        this.challenge = challenge;
    }

    public void ShowOffspring(BreedingUI ui, Creature offspring)
    {
        string phenotype = offspring.GetPhenotype("hornlength").ToLower();
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
            UnityEngine.Debug.Log($"[Offspring Match Test] {obj.name} - Match: {match} | Phenotype: {meta.Phenotype}/{phenotype}, Color: {meta.BodyColor}/{bodyColor}");

            string result = offspring.GetPhenotype("hornlength").ToLower();

            UnityEngine.Debug.Log(result);

            if (match)
            {
                if (challenge != null)
                {
                    challenge?.SetResult(phenotype, offspring);
                    UnityEngine.Debug.Log("[DEBUG] Sent result to challenge: " + result);
                }
                else
                {
                    UnityEngine.Debug.LogError("Challenge is NULL in LevelThreeBreedingUIHandler!");
                }
            }

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
