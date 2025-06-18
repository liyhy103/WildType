using UnityEngine;

public class CompendiumNavigator : MonoBehaviour
{
    public GameObject compendiumPanel;

    public static CompendiumNavigator Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("[CompendiumNavigator] Singleton instance assigned.");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GoToCompendium()
    {
        Debug.Log("[CompendiumNavigator] GoToCompendium() called!");
        if (compendiumPanel != null)
        {
            
            compendiumPanel.SetActive(true);

            CompendiumUI ui = compendiumPanel.GetComponent<CompendiumUI>();
            if (ui != null)
            {
                Debug.Log("[CompendiumNavigator] Refreshing compendium");
                ui.RefreshDisplay();
            }
        }
        else
        {
            Debug.LogWarning("[CompendiumNavigator] No compendium panel set.");
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
