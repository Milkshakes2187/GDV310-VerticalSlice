using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using VInspector;



public enum E_AbilityUseState
{
    Ready = 0,
    Casting = 1,
    Silenced = 2,
    Inactive = 4,
}


[System.Serializable]
public class S_SpellHolder  //TODO - finalise (maybe not class?)
{
    public AbilitySO ability;
    public KeyCode keybind;
    public float currentCooldown;

    public S_SpellHolder(AbilitySO _aso = null, KeyCode _code = KeyCode.None, float _cd = 0.0f)
    {
        ability = _aso;
        keybind = _code;
        currentCooldown = _cd;
    }
    public void SetCooldown(float _cd)
    {
        currentCooldown = _cd;
    }
}


public class PlayerSpellSystem : MonoBehaviour
{

    public E_AbilityUseState abilityUseState = E_AbilityUseState.Ready;

    [Tab("Main spell tab")]

    [SerializeField] List<S_SpellHolder> abilityHolders = new List<S_SpellHolder>();
    [SerializeField] KeyCode basicKey = KeyCode.Q;
  
    
    [SerializeField] List<AbilitySO> basicAbilitySequence = new List<AbilitySO>();
    [ReadOnly] int currentSequenceIndex = 0;


    [SerializeField] float abilitySequenceResetTime = 3.0f;
    [ReadOnly] float currentAbilitySequenceResetTime = 3.0f;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //add the basic ability to the list of abilite holders
        var newHolder = new S_SpellHolder(basicAbilitySequence[0], basicKey, 0.0f);
        abilityHolders.Insert(0, newHolder);
    }

    // Update is called once per frame
    void Update()
    {


        CheckInputs();

        TickCooldowns();
    }



    public void CheckInputs()
    {
        if (abilityUseState != E_AbilityUseState.Ready) { return; }


        if (Input.GetKeyDown(abilityHolders[0].keybind))
        {
            UseBasicAbility();
        }


    }


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
        abilityHolders[0].SetCooldown(basicAbilitySequence[currentSequenceIndex].cooldown);

        //start cooldowns for sequence resetting
        currentAbilitySequenceResetTime = abilitySequenceResetTime;

        //set ui
    }


    public void UseAbility(AbilitySO _abilityToUse) //WiP
    {
        if (!_abilityToUse) { return; }
       // if (_abilityToUse.IsOnCooldown()) { return; }



        //_abilityToUse.Use(null, null, Vector3.zero);

        //_abilityToUse.StartCooldown();
    }





   
    void TickCooldowns()
    {

        foreach (S_SpellHolder sHolder in abilityHolders)
        {
            if(sHolder.currentCooldown > 0.0f)
            {
                sHolder.SetCooldown(sHolder.currentCooldown - Time.deltaTime);
                if(sHolder.currentCooldown < 0.0f)
                {
                    sHolder.SetCooldown(0.0f);
                }
            }
        }
   
   
   
        //Ability sequence reset time
        if(currentAbilitySequenceResetTime > 0.0f)
        {
            currentAbilitySequenceResetTime -= Time.deltaTime;
   
            if(currentAbilitySequenceResetTime <=0.0f)
            {
                currentAbilitySequenceResetTime = 0.0f;
   
                //reset the sequence index
                currentSequenceIndex = 0;
                abilityHolders[0].ability = basicAbilitySequence[currentSequenceIndex];

                //sequence UI
            }
        }
    }


 
   //
   // public void StartCooldown()
   // {
   //     currentCooldown = cooldownBase;
   // }
   //
   // public void SetCurrentCooldown(float _cooldown)
   // {
   //     currentCooldown = _cooldown;
   // }
   //
   // public void TickCooldown(float _cooldownTicked)
   // {
   //     //if ability is on cooldown, reduce by an amount
   //     if (currentCooldown > 0.0f)
   //     {
   //         currentCooldown -= _cooldownTicked;
   //
   //         //if cooldown is less than 0, set it to zero
   //         if (currentCooldown < 0.0f)
   //         {
   //             currentCooldown = 0.0f;
   //         }
   //     }
   //
   //
   //     //update ui
   // }
   //
   // public bool IsOnCooldown()
   // {
   //     if (currentCooldown > 0.0f)
   //     {
   //         return true;
   //     }
   //
   //     return false;
   // }
}
