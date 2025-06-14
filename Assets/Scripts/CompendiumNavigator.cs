using UnityEngine.SceneManagement;
using UnityEngine;

public class CompendiumNavigator : MonoBehaviour
{
    public static CompendiumNavigator Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional if you need it to persist across scenes
            Debug.Log("[CompendiumNavigator] Singleton instance assigned.");
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void GoToCompendium()
    {
        CompendiumManager.Instance.PreviousSceneName = SceneManager.GetActiveScene().name;
        Debug.Log("[SceneTracker] Previous scene saved as: " + CompendiumManager.Instance.PreviousSceneName);
        SceneManager.LoadScene("CompendiumScene");
    }

    public void ReturnToPreviousScene()
    {
        Debug.Log("[SceneTracker] Return button was clicked!");
        string previous = CompendiumManager.Instance.PreviousSceneName;
        Debug.Log("[SceneTracker] Trying to return to previous scene: " + previous);

        if (!string.IsNullOrEmpty(previous))
        {
            SceneManager.LoadScene(previous);
        }
        else
        {
            Debug.LogWarning("[SceneTracker] No scene to return to. Defaulting to LevelOne.");
            SceneManager.LoadScene("LevelOne");
        }
    }
}
