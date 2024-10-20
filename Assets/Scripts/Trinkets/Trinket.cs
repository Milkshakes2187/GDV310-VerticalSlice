using UnityEngine;

public abstract class Trinket : MonoBehaviour
{
    Player player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = Player.instance;
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
