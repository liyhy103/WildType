using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public void OnButtonClick()
    {
        //load level one scene
        SceneManager.LoadScene("LevelOne");
    }
}
