using UnityEngine;

public class EntwinedAbyss : Ability
{
    public GameObject frontalBeam;

    private void Update()
    {
        transform.forward = owner.transform.forward;
    }

    /***********************************************
    * UseSpellEffect: Overriden spell effect, Creates a frontal beam of constant damage
    * @author: Juan Le Roux
    * @parameter:
    * @return: void
    ************************************************/
    public override void UseSpellEffect()
    {
        frontalBeam.SetActive(true);

        StartChannel();
    }

    /***********************************************
    * UseSpellEffect: Overriden inituial setup. 
    * @author: Juan Le Roux
    * @parameter:
    * @return: void
    ************************************************/
    public override void InitialSetup()
    {
        
    }
}
