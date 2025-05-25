using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialNextLevel : MonoBehaviour
{

    public string nextLevelName; // The name of the next level scene

    


    public void OnButtonClick()
    {       
        // Load the next level scene
        SceneManager.LoadScene(nextLevelName);
    }

}
