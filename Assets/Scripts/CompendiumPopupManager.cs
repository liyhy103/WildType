using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CompendiumPopupManager : MonoBehaviour
{
    public static CompendiumPopupManager Instance;

    public GameObject popupPanel;
    public TMP_Text popupText;
    private Creature currentCreature;

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
        CreatureTransfer.CreatureToAssign = currentCreature;
        CreatureTransfer.TargetParentIndex = 1;
        Debug.Log($"[Popup] Assigning {currentCreature.CreatureName} to Parent 1 and switching scene.");

        CompendiumNavigator.Instance.ReturnToPreviousScene();
        popupPanel.SetActive(false);
    }

    public void AssignToParent2()
    {
        CreatureTransfer.CreatureToAssign = currentCreature;
        CreatureTransfer.TargetParentIndex = 2;
        Debug.Log($"[Popup] Assigning {currentCreature.CreatureName} to Parent 2 and switching scene.");

        CompendiumNavigator.Instance.ReturnToPreviousScene();
        popupPanel.SetActive(false);
    }






    public void Cancel()
    {
        popupPanel.SetActive(false);
    }
}
