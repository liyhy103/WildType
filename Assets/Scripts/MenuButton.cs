using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    public void OnButtonClick()
    {
        SceneManager.LoadScene("StartMenu");
    }
}

