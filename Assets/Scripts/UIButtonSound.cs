using UnityEngine;

public class UIButtonSound : MonoBehaviour
{
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found on " + gameObject.name);
        }
    }

    void Start()
    {
        Debug.Log(">> AudioTest: Attempting to play sound...");
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning(">> AudioTest: No clip or AudioSource missing!");
        }
    }

    public void PlayClickSound()
    {
        Debug.Log("Button sound triggered");
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}
