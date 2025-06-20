using UnityEngine;

public class CompendiumNavigator : MonoBehaviour
{
    public static CompendiumNavigator Instance;
    public GameObject compendiumPanel;

    private void Awake()
    {
        Instance = this;
        Debug.Log("[CompendiumNavigator] Instance assigned for this scene.");
    }

    public void GoToCompendium()
    {
        if (compendiumPanel == null)
        {
            Debug.LogWarning("[Navigator] Compendium panel not set!");
            return;
        }

        compendiumPanel.SetActive(true);
        var ui = compendiumPanel.GetComponent<CompendiumUI>();
        if (ui != null)
        {
            ui.RefreshDisplay();
        }
    }

    public void ReturnToPreviousScene()
    {
        if (compendiumPanel != null)
        {
            compendiumPanel.SetActive(false);
        }
    }
}
