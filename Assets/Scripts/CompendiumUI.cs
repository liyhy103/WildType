using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CompendiumUI : MonoBehaviour
{
    public GameObject creatureEntryPrefab;
    public Transform creatureListParent;

    private void OnEnable()
    {
        if (CompendiumManager.Instance == null)
        {
            Debug.LogError("[CompendiumUI] No CompendiumManager found!");
            return;
        }

        ClearExistingEntries();

        foreach (Creature creature in CompendiumManager.Instance.compendium)
        {
            GameObject entry = Instantiate(creatureEntryPrefab, creatureListParent);
            TMP_Text text = entry.GetComponentInChildren<TMP_Text>();
            Image image = entry.transform.Find("CreatureImage")?.GetComponent<Image>();

            if (text != null)
                text.text = creature.GetFullDescription();

            if (image != null)
            {

                Sprite sprite = CompendiumManager.Instance.GetCreatureSprite(creature);
                Debug.Log($"[CompendiumUI] Loaded sprite: {sprite.name}");
                if (sprite != null)
                {
                    Debug.Log($"[Assigning Sprite] Creature: {creature.CreatureName}, Sprite: {sprite.name}");
                    image.sprite = sprite;
                    image.color = Color.white;
                    image.enabled = true;
                }
                else
                {
                    Debug.LogWarning($"[CompendiumUI] No sprite found for {creature.CreatureName}, hiding image.");

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
