using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CompendiumPopupManager : MonoBehaviour
{
    public static CompendiumPopupManager Instance;

    public GameObject popupPanel;
    public TMP_Text popupText;

    private Creature currentCreature;

    private void Awake()
    {
        Instance = this;
        Debug.Log("[CompendiumPopupManager] Instance assigned for this scene.");
    }

    public void ShowPopup(Creature creature)
    {
        currentCreature = creature;
        popupText.text = $"Use {creature.CreatureName}?";
        popupPanel.SetActive(true);
    }

    public void AssignToParent1()
    {
        if (!IsCreatureCompatible())
        {
            Debug.LogWarning("[Popup] Incompatible creature!");
            return;
        }

        Object.FindFirstObjectByType<BreedingUI>()?.AssignCompendiumCreature(1, currentCreature);
        StartCoroutine(DisableButtonAfterDelay(0.3f));
    }

    public void AssignToParent2()
    {
        if (!IsCreatureCompatible())
        {
            Debug.LogWarning("[Popup] Incompatible creature!");
            return;
        }

        Object.FindFirstObjectByType<BreedingUI>()?.AssignCompendiumCreature(2, currentCreature);
        StartCoroutine(DisableButtonAfterDelay(0.3f));
    }

    private IEnumerator DisableButtonAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        popupPanel.SetActive(false);
    }


    private bool IsCreatureCompatible()
    {
        return currentCreature?.SourceLevel == SceneManager.GetActiveScene().name;
    }

    public void Cancel()
    {
        StartCoroutine(DisableButtonAfterDelay(0.3f));
    }
}
