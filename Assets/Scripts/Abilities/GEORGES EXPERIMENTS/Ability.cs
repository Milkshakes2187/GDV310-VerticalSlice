using MPUIKIT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Ability : MonoBehaviour
{
    //assigned member variables
    [HideInInspector] public Character owner;
    [HideInInspector] public Character target;
    [HideInInspector] public Vector3 targetLocation;
    [HideInInspector] public AbilitySO abilityData;

    [Header("Base Ability Variables")]
    public bool canMoveWhileCasting = true;
    [HideInInspector] public float currentCastTime = 0.0f;

    //Coroutine to remember
    Coroutine castTimerCrouton;

    /***********************************************
    InitialSetup: Virtual function to perform inituial setup. Can be overridden, default does nothing.
    @author: George White
    @parameter:
    @return: virtual void
    ************************************************/
    public virtual void InitialSetup() { }

    /***********************************************
   * UseSpellEffect: Abstract function to use a spell's effect. Overridden by children
   * @author: George White
   * @parameter:
   * @return: abstract void
   ************************************************/
    public abstract void UseSpellEffect();

    /***********************************************
    * CastSpell: Calls "UseSpellEffect" either instantly, or after the required cast time
    * @author: George White
    * @parameter:
    * @return: void
    ************************************************/
    public void CastSpell()
    {
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
        StopCoroutine(castTimerCrouton);
        Destroy(gameObject);
    }

    /***********************************************
   * CastTimer: Coroutine to count down the spell's casting time, and casts the spell when done
   * @author: George White
   * @parameter:
   * @return: IEnumerator
   ************************************************/
    IEnumerator CastTimer()
    {
        while (currentCastTime > 0.0f)
        {
            currentCastTime -= Time.deltaTime;
            yield return new WaitForSeconds(0.0f);
        }

        //cast the spell
        UseSpellEffect();
    }
}
