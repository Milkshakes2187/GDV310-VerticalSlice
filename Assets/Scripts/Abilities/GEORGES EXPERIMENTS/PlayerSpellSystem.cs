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
    [SerializeField, ReadOnly] Player owner = null;
    [SerializeField, ReadOnly] Character target = null;
    [SerializeField, ReadOnly] public GameObject currentAbilityCast = null;

    public E_AbilityUseState abilityUseState = E_AbilityUseState.Ready;

    [Tab("Main spell tab")]

    public float spellCharge = 0.0f;
    public float spellChargeMax = 100.0f;
    public float GCD = 0.5f;
    [SerializeField, ReadOnly] float currentGCD = 0.0f;

    [SerializeField] List<AbilityDataHolder> abilityHolders = new List<AbilityDataHolder>();
    [SerializeField] KeyCode basicKey = KeyCode.Q;
  
    
    [SerializeField] List<AbilitySO> basicAbilitySequence = new List<AbilitySO>();
    [ReadOnly] int currentSequenceIndex = 0;


    [SerializeField] float abilitySequenceResetTime = 3.0f;
    [ReadOnly] float currentAbilitySequenceResetTime = 3.0f;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(basicAbilitySequence.Count > 0)
        {
            //add the basic ability to the list of abilite holders
            var newHolder = new AbilityDataHolder(basicAbilitySequence[0], basicKey, 0.0f);
            abilityHolders.Insert(0, newHolder);
        }
       

        //assigning player
        if(GetComponentInParent<Player>())
        {
            owner = GetComponentInParent<Player>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //raycast to select target
        AssignTarget();

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
        if (currentGCD > 0.0f) { return; }

        //checks JUST THE BASIC - to improve
        if(abilityHolders.Count > 0)
        {
            if (Input.GetKeyDown(abilityHolders[0].keybind))
            {
                UseBasicAbility();
            }
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
        if (abilityHolders[0].currentCooldown > 0.0f || currentGCD > 0.0f) { return; }


        //Instantiate and use the ability
        currentAbilityCast = abilityHolders[0].ability.InitialiseAbility(owner,target,transform.position);
        
        if (currentAbilityCast.GetComponent<Ability>().CastSpell(true))
        {

            //increment the sequence
            currentSequenceIndex++;
            if (currentSequenceIndex > basicAbilitySequence.Count - 1)
            {
                currentSequenceIndex = 0;
            }

            //set Ability0 to the current ability sequence
            abilityHolders[0].ability = basicAbilitySequence[currentSequenceIndex];

            //starts the cooldown of the next ability
            abilityHolders[0].currentCooldown = basicAbilitySequence[currentSequenceIndex].cooldown;
            currentGCD = GCD;


            //start cooldowns for sequence resetting
            currentAbilitySequenceResetTime = abilitySequenceResetTime;

            //set ui
        }
        else
        {
            Destroy(currentAbilityCast);
        }


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
        if (_abilityData.IsOnCooldown() || currentGCD > 0.0f) { return; }


        //instantiate ability, use ability, and start the cooldown
        currentAbilityCast = _abilityData.ability.InitialiseAbility(owner, target, Vector3.zero); ;
        

        if (currentAbilityCast.GetComponent<Ability>().CastSpell(true))
        {
            
            _abilityData.StartCooldown();
            currentGCD = GCD;
        }
        else
        {
            Destroy(currentAbilityCast);
        }
    }

    /***********************************************
    TickCooldowns: Ticks the cooldowns of all abilities, then updating the UI
    @author: George White
    @parameter: 
    @return: void
    ************************************************/
    void TickCooldowns()
    {
        //GCD
        if (currentGCD > 0.0f)
        {
            currentGCD -= Time.deltaTime;
            if(currentGCD < 0.0f)
            {
                currentGCD = 0.0f;
            }
        }


        //Iterates over all abilityDataHolder
        foreach (AbilityDataHolder sHolder in abilityHolders)
        {
            if(sHolder.IsOnCooldown())
            {
                sHolder.TickCoolDown(Time.deltaTime);
            }
        }
   

        if(abilityHolders.Count > 0)
        {
            //Tick basic sequence reset timer
            if (currentAbilitySequenceResetTime > 0.0f)
            {
                currentAbilitySequenceResetTime -= Time.deltaTime;
            }

            // Resets the basic ability sequence if cooldown has expired
            if (currentAbilitySequenceResetTime <= 0.0f)
            {
                currentAbilitySequenceResetTime = 0.0f;

                //reset the sequence index
                currentSequenceIndex = 0;
                abilityHolders[0].ability = basicAbilitySequence[currentSequenceIndex];

                //sequence UI
            }
        }
        
    }


    public bool SpendSpellCharge(float _cost)
    {
        if(_cost > spellCharge)
        {
            return false;
        }
        else
        {
            spellCharge -= _cost;
            //update UI

            return true;
        }
    }


    public void RegenerateSpellCharge(float _charge)
    {
        spellCharge += _charge;

        if (spellCharge > spellChargeMax)
        {
            spellCharge = spellChargeMax;
        }

        //update UI
    }


    public void AssignTarget()
    {
        //spherical raycast here!
    }






}
