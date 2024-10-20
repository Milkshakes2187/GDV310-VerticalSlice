using MPUIKIT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;


public abstract class Ability : MonoBehaviour
{
    //assigned member variables
    [HideInInspector] public Character owner;
    [HideInInspector] public Character target;
    [HideInInspector] public Vector3 targetLocation;
    [HideInInspector] public AbilitySO abilityData;

    [Header("Base Ability Variables")]
    public bool canMoveWhileCasting = true;
    public bool requiresTarget = false;
    [HideInInspector] public float currentCastTime = 0.0f;
    [HideInInspector] public float currentChannelTime = 0.0f;

    //Coroutine to remember
    Coroutine castTimerCrouton;
    Coroutine channelTimerCrouton;

    /***********************************************
    InitialSetup: Virtual function to perform inituial setup. Can be overridden, default does nothing.
    @author: George White
    @parameter:
    @return: virtual void
    ************************************************/
    public virtual void InitialSetup() { }

    /***********************************************
    VerifyTarget: Virtual function to verify that the target is correct
    @author: George White
    @parameter:
    @return: virtual bool
    ************************************************/
    public virtual bool VerifyTarget() { return true; }

    /***********************************************
    * OnDestroy: virtual function that occurs when the ability is destroyed
    * @author: George White
    * @parameter:
    * @return: virtual void
    ************************************************/
    public virtual void OnDestroy() { }



    /***********************************************
    * UseSpellEffect: Abstract function to use a spell's effect. Overridden by children
    * @author: George White
    * @parameter:
    * @return: abstract void
    ************************************************/
    public abstract void UseSpellEffect();

  


    /***********************************************
    * CastSpell: Calls "UseSpellEffect" either instantly, or after the required cast time. Can try to use a spell cost if needed.
    * @author: George White
    * @parameter: bool _useSpellCost = false
    * @return: void
    ************************************************/
    public bool CastSpell(bool _useSpellCost = false)
    {
        //cant cast spell if the target is invalid
        if (requiresTarget && !VerifyTarget()) { return false; }


        if(_useSpellCost)
        {
            if(owner.GetComponent<Player>().spellSystem)
            {
                //remove the casting cost from the player spell system, if possible.
                if(owner.GetComponent<Player>().spellSystem.SpendClassPower(abilityData.castingCost))
                {
                    //ability successfully paid for
                }
                else
                {
                    //ability cost spending failed - not enough charge

                    //effect for failiure?
                    return false;
                }
                
            }
        }

        InitialSetup();

        if (abilityData.timeToCast == 0.0f)
        {
            //instantly use the spell effect if there is no time to cast
            UseSpellEffect();
        }
        else
        {
            //start the cast timer coroutine
            currentCastTime = abilityData.timeToCast;
            castTimerCrouton = StartCoroutine(CastTimer());
        }

        return true;
    }

    /***********************************************
    * IsCasting: Returns wether the spell is currently being cast or not
    * @author: George White
    * @parameter:
    * @return: bool
    ************************************************/
    public bool IsCasting()
    {
        if(currentCastTime > 0.0f)
        {
            return true;
        }
        return false;
    }

    /***********************************************
    * Interrupt: Stops the spell from casting, and destroys the ability gameobject
    * @author: George White
    * @parameter:
    * @return: void
    ************************************************/
    public void Interrupt()
    {
        if (castTimerCrouton != null) { StopCoroutine(castTimerCrouton); }
        if (channelTimerCrouton != null) { StopCoroutine(channelTimerCrouton); }

        if (owner.GetComponent<Player>())
        {
            owner.GetComponent<Player>().spellSystem.abilityUseState = E_AbilityUseState.Ready;
        }

        Destroy(gameObject);
    }

    /***********************************************
    * CastTimer: Coroutine to count down the spell's casting time, and casts the spell when done. unique effects for player
    * @author: George White
    * @parameter:
    * @return: IEnumerator
    ************************************************/
    IEnumerator CastTimer()
    {
        Vector3 ownerCastLocation = owner.gameObject.transform.position;

        if(owner.GetComponent<Player>())
        {
            owner.GetComponent<Player>().spellSystem.abilityUseState = E_AbilityUseState.Casting;
        }

        while (currentCastTime > 0.0f)
        {
            currentCastTime -= Time.deltaTime;

            //interrupts if player cant move while casting
            if(!canMoveWhileCasting && owner.gameObject.transform.position != ownerCastLocation)
            {
                Interrupt();
            }
            yield return new WaitForSeconds(0.0f);
        }

        //reset abilitystate
        if (owner.GetComponent<Player>())
        {
            owner.GetComponent<Player>().spellSystem.abilityUseState = E_AbilityUseState.Ready;
        }

        //cast the spell
        UseSpellEffect();
    }

    /***********************************************
    * CastSpell: Activates the channel timer
    * @author: Juan Le Roux
    * @parameter:
    * @return: void
    ************************************************/
    public void StartChannel()
    {
        currentChannelTime = abilityData.timeToChannel;
        channelTimerCrouton = StartCoroutine(ChannelTimer());
    }

    /***********************************************
    * ChannelTimer: Courtine to count down the spell's channel time, destroys the spell when the channel is doen
    * @author: Juan Le Roux
    * @parameter:
    * @return: IEnumerator
    ************************************************/
    IEnumerator ChannelTimer()
    {
        while (currentChannelTime > 0.0f)
        {
            currentChannelTime -= Time.deltaTime;
            yield return new WaitForSeconds(0.0f);
        }

        //cast the spell
        Destroy(gameObject);
    }
}
