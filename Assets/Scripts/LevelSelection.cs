using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    public void OnButtonClick()
    {
        SceneManager.LoadScene("LevelSelection");
    }
}
