using UnityEngine;


public class QuitScript : MonoBehaviour
{
    //Quit game when player clicks button

    public void OnButtonClick()
    {
        Application.Quit();

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
