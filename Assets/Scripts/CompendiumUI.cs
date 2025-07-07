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

        ClearExistingEntries();

        var groupedCreatures = new Dictionary<string, List<Creature>>();

        foreach (var creature in CompendiumManager.Instance.compendium)
        {
            if (string.IsNullOrWhiteSpace(creature.SourceLevel))
            {
                Debug.LogError($"[CompendiumUI] Creature {creature.CreatureName} has no SourceLevel assigned.");
                continue; 
            }

            if (!groupedCreatures.ContainsKey(creature.SourceLevel))
                groupedCreatures[creature.SourceLevel] = new List<Creature>();

            groupedCreatures[creature.SourceLevel].Add(creature);
        }
        
        List<string> levelOrder = new()
            {
                Creature.Tutorial,
                Creature.LevelOne,
                Creature.LevelTwo,
                Creature.LevelThree,
                Creature.LevelFour
            };
        foreach (var level in levelOrder)
        {
            if (!groupedCreatures.TryGetValue(level, out var creatures))
                continue;

            GameObject header = Instantiate(levelHeaderPrefab, creatureListParent);
            TMP_Text headerText = header.GetComponentInChildren<TMP_Text>();
            if (headerText != null)
            {
                headerText.text = level;
                headerText.fontSize = 28;
                headerText.alignment = TextAlignmentOptions.Center;
            }

            foreach (var creature in creatures)
            {
                GameObject entry = Instantiate(creatureEntryPrefab, creatureListParent);

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
    }

    private void ClearExistingEntries()
    {
        foreach (Transform child in creatureListParent)
        {
            Destroy(child.gameObject);
        }
    }
}
