using UnityEngine;

public class LevelFourBreedingUIHandler : IBreedingUIHandler
{
    private LevelFourChallenge challenge;

    public LevelFourBreedingUIHandler(LevelFourChallenge challenge)
    {
        this.challenge = challenge;
    }
    public void ShowOffspring(BreedingUI ui, Creature offspring)
    {
        string tailType = offspring.GetPhenotype("TailLength").ToLower();
        string bodyColor = offspring.GetPhenotype("CoatColor").ToLower();

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

            if (match)
            {
                if (challenge != null)
                {
                    challenge?.SetResult(tailType, bodyColor, offspring);
                    UnityEngine.Debug.Log("[DEBUG] Sent result to challenge: " + tailType + ", " + bodyColor);
                }
                else
                {
                    UnityEngine.Debug.LogError("Challenge is NULL in LevelFourBreedingUIHandler!");
                }
            }

            Debug.Log($"[Level4 Show] Matching: {obj.name} | Meta = {meta.BodyColor}/{meta.Phenotype}, Creature = {bodyColor}/{tailType}, Match = {match}");
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
        foreach (GameObject obj in ui.level4OffspringDisplayObjects)
        {
            if (obj.activeSelf)
            {
                var image = obj.GetComponent<UnityEngine.UI.Image>();
                if (image != null && image.sprite != null)
                {
                    return image.sprite;
                }
            }
        }

        Debug.LogWarning("[LevelFour] No active offspring sprite found.");
        return null;
    }
}