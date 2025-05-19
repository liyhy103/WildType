using UnityEngine;

using UnityEngine.UI;

public interface IBreedingUIHandler
{
    void ShowOffspring(BreedingUI ui, Creature offspring);
    bool ValidateParents(BreedingUI ui, Creature p1, Creature p2, out string errorMessage);
    Sprite GetOffspringSprite(BreedingUI ui);

}