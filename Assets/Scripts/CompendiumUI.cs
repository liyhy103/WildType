using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CompendiumUI : MonoBehaviour
{
    public GameObject creatureEntryPrefab;
    public Transform creatureListParent;
    public GameObject levelHeaderPrefab;

    private void OnEnable()
    {
        RefreshDisplay();
    }

    public void RefreshDisplay()
    {
        if (CompendiumManager.Instance == null)
        {
            Debug.LogError("[CompendiumUI] No CompendiumManager found!");
            return;
        }

        ClearExistingEntries();// Clear to avoid stacking previous creatures on top of new refresh 

        foreach (var creature in CompendiumManager.Instance.compendium)  {
            if (string.IsNullOrWhiteSpace(creature.SourceLevel))
            {
                Debug.LogError($"[CompendiumUI] Creature {creature.CreatureName} has no SourceLevel assigned.");
                continue;
            }

            GameObject entry = Instantiate(creatureEntryPrefab, creatureListParent);// Instantiate as a compendium entry prefab 

            var clickHandler = entry.GetComponent<CompendiumEntryClickHandler>();
            if (clickHandler != null)
            {
                clickHandler.creatureData = creature;
            }

            TMP_Text text = entry.GetComponentInChildren<TMP_Text>();
            if (text != null)
                text.text = creature.GetFullDescription();

            Image image = entry.transform.Find("CreatureImage")?.GetComponent<Image>();
            if (image != null)
            {
                Sprite sprite = CompendiumManager.Instance.GetCreatureSprite(creature);
                if (sprite != null)
                {
                    image.sprite = sprite;
                    image.color = Color.white;
                    image.enabled = true;
                }
                else
                {
                    image.sprite = null;
                    image.color = new Color(1, 1, 1, 0);
                    image.enabled = false;
                }
            }

            Debug.Log($"[CompendiumUI] Displaying: {creature.GetFullDescription()}");
        }
    }


    private void ClearExistingEntries()
    {
        foreach (Transform child in creatureListParent)
        {
            Destroy(child.gameObject);
        }
    }
}
