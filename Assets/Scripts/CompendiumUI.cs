using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CompendiumUI : MonoBehaviour
{
    public GameObject entryPrefab;
    public Transform contentParent;

    private void OnEnable()
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        foreach (Creature creature in CompendiumManager.Instance.compendium)
        {
            GameObject entry = Instantiate(entryPrefab, contentParent);

            TMP_Text text = entry.transform.Find("CreatureText")?.GetComponent<TMP_Text>();
            if (text != null)
                text.text = creature.GetFullDescription();

            Image image = entry.transform.Find("CreatureImage")?.GetComponent<Image>();
            Sprite sprite = CompendiumManager.Instance.GetCreatureSprite(creature);

            if (image != null)
            {
                if (sprite != null)
                {
                    image.sprite = sprite;
                    image.color = Color.white;
                    image.enabled = true;
                }
                else
                {
                    image.sprite = null;
                    image.color = new Color(0, 0, 0, 0); // Fully transparent
                    image.enabled = false; // Hide completely
                }
            }
        }
    }

}
