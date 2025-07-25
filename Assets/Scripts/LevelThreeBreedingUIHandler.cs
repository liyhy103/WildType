using System.Collections.Generic;
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
        string phenotype = offspring.GetPhenotype(Gene.Traits.HornLength);
        string bodyColor = offspring.BodyColor;

        foreach (var obj in ui.level3OffspringDisplayObjects)
        {
            var meta = obj.GetComponent<DisplayMetadata>();
            if (meta == null)
            {
                obj.SetActive(false);
                continue;
            }
            UnityEngine.Debug.Log($"[MatchCheck] Offspring phenotype: {phenotype}, bodyColor: {bodyColor}");
            UnityEngine.Debug.Log($"[MatchCheck] Prefab meta: {meta.Phenotype}, {meta.BodyColor}");

            bool match = meta.Phenotype.Trim().ToLower() == phenotype.Trim().ToLower()
                      && meta.BodyColor.Trim().ToLower() == bodyColor.Trim().ToLower();

            obj.SetActive(match);
            UnityEngine.Debug.Log($"[Offspring Match Test] {obj.name} - Match: {match} | Phenotype: {meta.Phenotype}/{phenotype}, Color: {meta.BodyColor}/{bodyColor}");

            string result = offspring.GetPhenotype(Gene.Traits.HornLength).ToLower();

            UnityEngine.Debug.Log(result);

            if (match)
            {
                if (challenge != null)
                {
                    challenge?.SetResult(new List<string> { offspring.GetPhenotype(Gene.Traits.HornLength) }, offspring);
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
