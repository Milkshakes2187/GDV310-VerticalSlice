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
    * WorldLevelUp: Increases world level and calls all related functions.
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

    /***********************************************
    * DisplayLevelChoice: Called when event is completed. Displays level choice screen.
    * @author: Justhine Nisperos
    * @parameter: 
    * @return: void
    ************************************************/
    public void DisplayLevelChoice()
    {
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
            WorldLevelUp();
        }

        // TODO: Implement actual world level skip
        Debug.Log("WORLD LEVEL " + _level + " SELECTED.");
        GameManager.instance.ToggleFreeCursor(false);
    }
}
