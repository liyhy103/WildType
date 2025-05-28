using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class CompendiumPopupManager : MonoBehaviour
{
    public static CompendiumPopupManager Instance;

    public GameObject popupPanel;
    public TMP_Text popupText;
    private Creature currentCreature;

    public GameObject incompatiblePopup;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("[CompendiumPopupManager] Singleton assigned.");
        }
        else
        {
            Destroy(gameObject);
        }
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
            if (incompatiblePopup != null)
            {
                incompatiblePopup.SetActive(true);
                StartCoroutine(HidePopupAfterDelay(3f));
            }
            return; 
        }

        CreatureTransfer.CreatureToAssign = currentCreature;
        CreatureTransfer.TargetParentIndex = 1;
        Debug.Log($"[Popup] Assigning {currentCreature.CreatureName} to Parent 1 and switching scene.");

        CompendiumNavigator.Instance.ReturnToPreviousScene();
        popupPanel.SetActive(false);
    }

    public void AssignToParent2()
    {
        if (!IsCreatureCompatible())
        {
            Debug.LogWarning("[Popup] Incompatible creature!");
            if (incompatiblePopup != null)
            {
                incompatiblePopup.SetActive(true);
                StartCoroutine(HidePopupAfterDelay(3f));
            }
            return; 
        }

        CreatureTransfer.CreatureToAssign = currentCreature;
        CreatureTransfer.TargetParentIndex = 2;
        Debug.Log($"[Popup] Assigning {currentCreature.CreatureName} to Parent 2 and switching scene.");

        CompendiumNavigator.Instance.ReturnToPreviousScene();
        popupPanel.SetActive(false);
    }


    private IEnumerator HidePopupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        incompatiblePopup.SetActive(false);
    }

    private bool IsCreatureCompatible()
    {
        return currentCreature?.SourceLevel == CompendiumManager.Instance.PreviousSceneName;
    }

    public void Cancel()
    {
        popupPanel.SetActive(false);
    }
}
