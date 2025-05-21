using UnityEngine;

public class CompendiumBootstrapper : MonoBehaviour
{
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
