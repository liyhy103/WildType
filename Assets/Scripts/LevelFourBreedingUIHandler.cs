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
        string tailType = offspring.GetPhenotype(Gene.Traits.TailLength);
        string bodyColor = offspring.GetPhenotype(Gene.Traits.CoatColor);

        foreach (GameObject obj in ui.level4OffspringDisplayObjects)
        {
            var meta = obj.GetComponent<DisplayMetadata>();
            if (meta == null)
            {
                obj.SetActive(false);
                continue;
            }

            bool match = meta.BodyColor == bodyColor && meta.Phenotype == tailType;

            obj.SetActive(match);

            if (match)
            {
                if (challenge != null)
                {
                    challenge?.SetResult(bodyColor, tailType, offspring); 
                    Debug.Log("[DEBUG] Sent result to challenge: " + tailType + ", " + bodyColor);
                }
                else
                {
                    Debug.LogError("Challenge is NULL in LevelFourBreedingUIHandler!");
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