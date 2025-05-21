using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScript : MonoBehaviour
{
    public void OnButtonClick()
    {
        SceneManager.LoadScene("LevelOne");
    }
}