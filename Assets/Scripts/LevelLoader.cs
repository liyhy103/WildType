using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public string sceneName; // Scene to load when this button is clicked

    void Start(){
        GetComponent<Button>().onClick.AddListener(LoadLevel);
    }

    void LoadLevel(){
        if (!string.IsNullOrEmpty(sceneName)){
            Debug.Log($"[LevelLoader] Loading scene: {sceneName}");
            SceneManager.LoadScene(sceneName);
        }
        else{
            Debug.LogWarning("[LevelLoader] Scene name not set!");
        }
    }
}
