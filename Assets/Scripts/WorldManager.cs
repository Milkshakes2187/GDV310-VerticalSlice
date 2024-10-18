using System;
using UnityEngine;
using VInspector;

public class WorldManager : MonoBehaviour
{
    public static WorldManager instance;

    public int worldLevel = 1;

    public event Action onWorldLevelUp;

    [Foldout("UI References")]
    public GameObject levelChoiceUI;

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
    * WorldLevelUp: Called when event is completed. Displays level choice screen.
    * @author: Justhine Nisperos
    * @parameter: 
    * @return: void
    ************************************************/
    public void WorldLevelUp()
    {
        // Doesn't level up the world in vertical slice as the player gets to choose the level.

        // All functions that need to know when the world levels up is called here.
        //onWorldLevelUp?.Invoke();
        //worldLevel++;

        //------------------------------------------------

        // Display level choice UI
        levelChoiceUI.SetActive(true);
        GameManager.instance.ToggleFreeCursor(true);
    }

    /***********************************************
    * SkipToLevel: Continuously levels the world until the desired level.
    * @author: Justhine Nisperos
    * @parameter: int
    * @return: void
    ************************************************/
    public void SkipToLevel(int _level)
    {
        // Invoke world level up until desired level.
        for (int i = worldLevel; i < _level; i++)
        {
            onWorldLevelUp?.Invoke();

            worldLevel++;
        }

        GameManager.instance.ToggleFreeCursor(false);
    }
}
