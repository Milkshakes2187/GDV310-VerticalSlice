using UnityEngine;

public class PhantomAssassin : Ability
{
    public AbilitySO abyssalKnives;
    public float turnSpeed = 5f;

    GameObject knivesActive;
    bool intermissionState = false;

    public float knivesCastTimer = 5f;
    float elapsedTime = 0f;

    private void Update()
    {
        // If abyssal knives is not being cast turn towards player 
        if (!knivesActive)
        {
            TurnTowardsPlayer();
        }

        // if the owner is in their intermission state cast abyssalknives every x amount of seconds
        if (intermissionState)
        {
            if (elapsedTime > knivesCastTimer)
            {
                elapsedTime = 0f;

                UseSpellEffect();
            }
        }
    }

    /***********************************************
    * UseSpellEffect: Overriden spell effect, casts abyssal knives towards target location
    * @author: Juan Le Roux
    * @parameter:
    * @return: void
    ************************************************/
    public override void UseSpellEffect()
    {
        knivesActive = abyssalKnives.InitialiseAbility(owner, target, targetLocation);
        knivesActive.GetComponent<Ability>().InitialSetup();
        knivesActive.GetComponent<Ability>().CastSpell();
    }

    /***********************************************
    * UseSpellEffect: Overriden inituial setup. gets called once the boss enters intermission state
    * @author: Juan Le Roux
    * @parameter:
    * @return: void
    ************************************************/
    public override void InitialSetup()
    {
        intermissionState = true;
    }

    /***********************************************
    * DestroyAssassin: Callable function to destroy the phantom assassin
    * @author: Juan Le Roux
    * @parameter:
    * @return: void
    ************************************************/
    public void DestroyAssassin()
    {
        if (owner.GetComponent<AbyssalWeaver>())
        {
            owner.GetComponent<AbyssalWeaver>().phantomAssassinList.Remove(this);
        }

        Destroy(gameObject);
    }

    /***********************************************
    * TurnTowardsPlayer: Turns the character towards the player
    * @author: Juan Le Roux
    * @parameter:
    * @return: void
    ************************************************/
    void TurnTowardsPlayer()
    {
        // get the player direction
        Vector3 direction = Player.instance.transform.position - transform.position;
        direction.y = 0;

        // set the desired look rotation towards the players look direction
        Quaternion rotation = Quaternion.LookRotation(direction);

        // Slerp the rotation towards the player at a designated turn speed
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * turnSpeed);
    }
}
