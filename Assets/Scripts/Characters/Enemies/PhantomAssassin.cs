using UnityEngine;

public class PhantomAssassin : Ability
{
    public AbilitySO abyssalKnives;

    /***********************************************
    * UseSpellEffect: Overriden spell effect, creates a shadow assassin on targets position
    * @author: Juan Le Roux
    * @parameter:
    * @return: void
    ************************************************/
    public override void UseSpellEffect()
    {
        var ability = abyssalKnives.InitialiseAbility(owner, target, targetLocation);
        ability.GetComponent<Ability>().CastSpell();
    }

    /***********************************************
    * UseSpellEffect: Overriden inituial setup. Instantiates the indicator of where the shadow assassin will be placed
    * @author: Juan Le Roux
    * @parameter:
    * @return: void
    ************************************************/
    public override void InitialSetup()
    {
        
    }

    public void DestroyAssassin()
    {
        Destroy(gameObject);
    }
}
