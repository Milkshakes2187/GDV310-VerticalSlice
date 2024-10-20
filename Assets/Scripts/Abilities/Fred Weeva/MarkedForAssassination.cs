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
    * UseSpellEffect: Overriden spell effect, creates a phantom assassin on targets position
    * @author: Juan Le Roux
    * @parameter:
    * @return: void
    ************************************************/
    public override void UseSpellEffect()
    {
        Vector3 modifiedPosition = target.transform.position;
        modifiedPosition.y = owner.transform.position.y;

        var ability = phantomAssassin.InitialiseAbility(owner, target, modifiedPosition);

        if (owner.GetComponent<AbyssalWeaver>())
        {
            owner.GetComponent<AbyssalWeaver>().phantomAssassinList.Add(ability.GetComponent<PhantomAssassin>());
        }
        
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
