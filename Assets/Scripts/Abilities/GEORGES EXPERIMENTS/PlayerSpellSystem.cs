using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Playables;
using UnityEngine;
using VHierarchy.Libs;
using VInspector;

public enum E_AbilityUseState
{
    Ready = 0,
    Casting = 1,
    Silenced = 2,
    Inactive = 4,
}


public class PlayerSpellSystem : MonoBehaviour
{

    public E_AbilityUseState abilityUseState = E_AbilityUseState.Ready;

    [Tab("Main spell tab")]

    [SerializeField] List<AbilityDataHolder> abilityHolders = new List<AbilityDataHolder>();
    [SerializeField] KeyCode basicKey = KeyCode.Q;
  
    
    [SerializeField] List<AbilitySO> basicAbilitySequence = new List<AbilitySO>();
    [ReadOnly] int currentSequenceIndex = 0;


    [SerializeField] float abilitySequenceResetTime = 3.0f;
    [ReadOnly] float currentAbilitySequenceResetTime = 3.0f;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //add the basic ability to the list of abilite holders
        var newHolder = new AbilityDataHolder(basicAbilitySequence[0], basicKey, 0.0f);
        abilityHolders.Insert(0, newHolder);
    }

    // Update is called once per frame
    void Update()
    {
        //Deal with spell inputs
        CheckInputs();

        //tick all the cooldowns of spells
        TickCooldowns();
    }

    /***********************************************
    CheckInputs: Checks input states of all relevant ability keys
    @author: George White
    @parameter:
    @return:  void
    ************************************************/
    public void CheckInputs()
    {
        //returns if the player is not in a castable state
        if (abilityUseState != E_AbilityUseState.Ready) { return; }

        //checks JUST THE BASIC - to improve
        if (Input.GetKeyDown(abilityHolders[0].keybind))
        {
            UseBasicAbility();
        }
    }

    /***********************************************
    UseBasicAbility: Uses the basic ability, and swaps it out for the next one in the sequence
    @author: George White
    @parameter:
    @return: void
    ************************************************/
    public void UseBasicAbility()
    {
        //cant cast spell if it is on cooldown
        if (abilityHolders[0].currentCooldown > 0.0f) { return; }


        //Instantiate and use the ability
        var aaAbility = abilityHolders[0].ability.InitialiseAbility(null,null,transform.position);
        aaAbility.GetComponent<Ability>().CastSpell();


        //increment the sequence
        currentSequenceIndex++;
        if(currentSequenceIndex > basicAbilitySequence.Count - 1)
        {
            currentSequenceIndex = 0;
        }

        //set Ability0 to the current ability sequence
        abilityHolders[0].ability = basicAbilitySequence[currentSequenceIndex];

        //starts the cooldown of the next ability
        abilityHolders[0].currentCooldown = basicAbilitySequence[currentSequenceIndex].cooldown;

        //start cooldowns for sequence resetting
        currentAbilitySequenceResetTime = abilitySequenceResetTime;

        //set ui
    }

    /***********************************************
   UseAbility: Use a selected ability, then start it's cooldown
   @author: George White
   @parameter: AbilitySO _abilityToUse
   @return: void
   ************************************************/
    public void UseAbility(AbilityDataHolder _abilityData) //WiP
    {
        if (!_abilityData || !_abilityData.ability) { return; }
        if (_abilityData.IsOnCooldown()) { return; }


        //instantiate ability, use ability, and start the cooldown
        _abilityData.ability.InitialiseAbility(null, null, Vector3.zero); ;
        _abilityData.ability.GetComponent<Ability>().CastSpell();
        _abilityData.StartCooldown();
    }

    /***********************************************
    TickCooldowns: Ticks the cooldowns of all abilities, then updating the UI
    @author: George White
    @parameter: 
    @return: void
    ************************************************/
    void TickCooldowns()
    {
        //Iterates over all abilityDataHolder
        foreach (AbilityDataHolder sHolder in abilityHolders)
        {
            if(sHolder.IsOnCooldown())
            {
                sHolder.TickCoolDown(Time.deltaTime);
            }
        }
   
        //Tick basic sequence reset timer
        if(currentAbilitySequenceResetTime > 0.0f)
        {
            currentAbilitySequenceResetTime -= Time.deltaTime;

            // Resets the basic ability sequence if cooldown has expired
            if (currentAbilitySequenceResetTime <=0.0f)
            {
                currentAbilitySequenceResetTime = 0.0f;
   
                //reset the sequence index
                currentSequenceIndex = 0;
                abilityHolders[0].ability = basicAbilitySequence[currentSequenceIndex];

                //sequence UI
            }
        }
    }

}
