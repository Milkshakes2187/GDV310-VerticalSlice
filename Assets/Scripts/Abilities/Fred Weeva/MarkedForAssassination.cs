using UnityEngine;

public class MarkedForAssassination : Ability
{
    public GameObject indicatorPF;
    public AbilitySO phantomAssassin;

    GameObject indicator;

    private void Update()
    {
        // if the indicator exists, keep it tracked above the player
        if (indicator)
        {
            indicator.transform.position = new Vector3(target.transform.position.x, target.transform.position.y + 4, target.transform.position.z);
        }
    }

    /***********************************************
    * UseSpellEffect: Overriden spell effect, creates a shadow assassin on targets position
    * @author: Juan Le Roux
    * @parameter:
    * @return: void
    ************************************************/
    public override void UseSpellEffect()
    {
        phantomAssassin.InitialiseAbility(owner, target, targetLocation);

        Destroy(gameObject);
    }

    /***********************************************
    * UseSpellEffect: Overriden inituial setup. Instantiates the indicator of where the shadow assassin will be placed
    * @author: Juan Le Roux
    * @parameter:
    * @return: void
    ************************************************/
    public override void InitialSetup()
    {
        indicator = Instantiate(indicatorPF, target.transform.position, Quaternion.identity);
        Destroy(indicator, abilityData.timeToCast);
    }
}
