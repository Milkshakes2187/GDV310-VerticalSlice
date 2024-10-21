using UnityEngine;

public abstract class Trinket : MonoBehaviour
{
    Player player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Player.instance)
        {
            player = Player.instance;
        }
        else
        {
            Debug.LogWarning("NO PLAYER IN SCENE.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /***********************************************
    * ApplyTrinketEffects: Applies the trinket's effects for the player.
    * @author: Justhine Nisperos
    * @parameter: 
    * @return: void
    ************************************************/
    public abstract void ApplyTrinketEffects();
}
