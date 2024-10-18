using System;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public static WorldManager instance;

    public int worldLevel = 0;

    public event Action onWorldLevelUp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /***********************************************
    * WorldLevelUp: Increases world level and invokes all related functions accordingly.
    * @author: Justhine Nisperos
    * @parameter: 
    * @return: void
    ************************************************/
    public void WorldLevelUp()
    {
        // All functions that need to know when the world levels up is called here.
        onWorldLevelUp?.Invoke();

        worldLevel++;
    }
}
