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
        if (CompendiumManager.Instance == null)
        {
            Debug.LogError("[CompendiumUI] No CompendiumManager found!");
            return;
        }

        ClearExistingEntries();

        var groupedCreatures = new Dictionary<string, List<Creature>>();

        foreach (var creature in CompendiumManager.Instance.compendium)
        {
            string levelKey = string.IsNullOrEmpty(creature.SourceLevel) ? "Unknown Level" : creature.SourceLevel;

            if (!groupedCreatures.ContainsKey(levelKey))
                groupedCreatures[levelKey] = new List<Creature>();

            groupedCreatures[levelKey].Add(creature);
        }
        List<string> levelOrder = new List<string> { "LevelOne", "LevelTwo", "LevelThree", "LevelFour" };

        foreach (var level in levelOrder)
        {
            if (!groupedCreatures.ContainsKey(level)) continue;

            GameObject header = Instantiate(levelHeaderPrefab, creatureListParent);
            TMP_Text headerText = header.GetComponentInChildren<TMP_Text>();
            if (headerText != null)
            {
                Debug.Log($"[CompendiumUI] Creating header for: {level}");
                headerText.text = level;
                headerText.fontSize = 28;
                headerText.alignment = TextAlignmentOptions.Center;
            }

            foreach (var creature in groupedCreatures[level])
            {
                GameObject entry = Instantiate(creatureEntryPrefab, creatureListParent);
                var clickHandler = entry.GetComponent<CompendiumEntryClickHandler>();
                if (clickHandler != null)
                {
                    clickHandler.creatureData = creature;
                }
                TMP_Text text = entry.GetComponentInChildren<TMP_Text>();
                Image image = entry.transform.Find("CreatureImage")?.GetComponent<Image>();

                if (text != null)
                    text.text = creature.GetFullDescription();

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



        void ClearExistingEntries()
        {
            foreach (Transform child in creatureListParent)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
