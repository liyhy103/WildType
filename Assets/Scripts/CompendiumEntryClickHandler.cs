using UnityEngine;
using UnityEngine.EventSystems;

public class CompendiumEntryClickHandler : MonoBehaviour, IPointerClickHandler
{
    public Creature creatureData;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked compendium entry: " + (creatureData != null ? creatureData.CreatureName : "NULL"));

        if (CompendiumPopupManager.Instance == null)
        {
            Debug.LogWarning("[ClickHandler] Popup Manager instance is null!");
            return;
        }

        if (creatureData == null)
        {
            Debug.LogWarning("[ClickHandler] Creature data is null!");
            return;
        }

        CompendiumPopupManager.Instance.ShowPopup(creatureData);
    }
}
