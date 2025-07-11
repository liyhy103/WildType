using UnityEngine;

public class CompendiumBootstrapper : MonoBehaviour
{// Intansiates the game object as compedium manager in title screen 
    public GameObject compendiumManagerPrefab;

    void Awake()
    {
        if (CompendiumManager.Instance == null)
        {
            GameObject manager = Instantiate(compendiumManagerPrefab);
            manager.name = "CompendiumManager";
        }
    }
}
