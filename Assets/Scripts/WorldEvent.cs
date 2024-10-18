using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using VInspector;

public class WorldEvent : MonoBehaviour
{
    public enum TYPE
    {
        Enemies,
    }

    public TYPE eventType = TYPE.Enemies;

    [ShowIf("eventType", TYPE.Enemies)]
    public List<GameObject> enemyList = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        switch(eventType)
        {
            case TYPE.Enemies:
                {
                    if (enemyList.Count <= 0)
                    {
                        Debug.LogError("No enemies assigned to event.");
                    }
                    else
                    {
                        // Assign this event to enemy's "WorldEvent" var
                    }

                }
                
                break;
        }
    }

    /***********************************************
    * CheckComplete: Checks the condition for completing the event based on event type.
    * @author: Justhine Nisperos
    * @parameter: 
    * @return: void
    ************************************************/
    public void CheckComplete()
    {
        switch (eventType)
        {
            case TYPE.Enemies:
                {
                    if (enemyList.Count <= 0)
                    {
                        EventComplete();
                    }
                }
                break;
        }
    }

    /***********************************************
    * EventComplete: Trigger world level up and destroys this event.
    * @author: Justhine Nisperos
    * @parameter: 
    * @return: void
    ************************************************/
    void EventComplete()
    {
        WorldManager.instance.WorldLevelUp();

        Destroy(gameObject);
    }
}
