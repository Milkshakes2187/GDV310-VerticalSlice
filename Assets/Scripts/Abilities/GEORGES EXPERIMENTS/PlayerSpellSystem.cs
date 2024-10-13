using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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


[System.Serializable]
public struct S_SpellHolder
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
}


public class PlayerSpellSystem : MonoBehaviour
{

    public E_AbilityUseState abilityUseState = E_AbilityUseState.Ready;

    [Tab("Main spell tab")]

    [SerializeField] List<S_SpellHolder> abilityHolder = new List<S_SpellHolder>();
  
    
    [SerializeField] List<AbilitySO> basicAbilitySequence = new List<AbilitySO>();
    [ReadOnly] int currentSequenceIndex = 0;


    [SerializeField] float abilitySequenceResetTime = 3.0f;
    [ReadOnly] float currentAbilitySequenceResetTime = 3.0f;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        CheckInputs();

        //TickCooldowns();
    }



    public void CheckInputs()
    {
        if (abilityUseState != E_AbilityUseState.Ready) { return; }


       // if (Input.GetKeyDown(abilityKeyBasic))
       // {
       //     UseBasicAbility();
       // }


    }


    public void UseBasicAbility()
    {
        //cant cast spell if it is on cooldown
        //if (basicAbilitySequence[currentSequenceIndex].IsOnCooldown()) { return; }

        //basicAbilitySequence[currentSequenceIndex].Use(null, null, new Vector3 (transform.position.x, transform.position.y + 2 + (0.5f * currentSequenceIndex), transform.position.z));

        var aaAbility = basicAbilitySequence[currentSequenceIndex].InitialiseAbility(null,null,Vector3.zero);
        //aaAbility.use();
       // aaAbility.Destroy(); To be done in the script

        //increment the sequence
        currentSequenceIndex++;
        if(currentSequenceIndex > basicAbilitySequence.Count - 1)
        {
            currentSequenceIndex = 0;
        }

        //starts the cooldown of the next ability
       // basicAbilitySequence[currentSequenceIndex].StartCooldown();


        //start cooldowns for sequence resetting
        currentAbilitySequenceResetTime = abilitySequenceResetTime;

        //set ui
    }


    public void UseAbility(AbilitySO _abilityToUse)
    {
        if (!_abilityToUse) { return; }
       // if (_abilityToUse.IsOnCooldown()) { return; }



        //_abilityToUse.Use(null, null, Vector3.zero);

        //_abilityToUse.StartCooldown();
    }





   //
   // void TickCooldowns()
   // {
   //     //tick basic ability cooldown
   //     basicAbilitySequence[currentSequenceIndex].TickCooldown(Time.deltaTime);
   //     
   //     //tick all other cooldowns
   //
   //
   //
   //     //Ability sequence reset time
   //     if(currentAbilitySequenceResetTime > 0.0f)
   //     {
   //         currentAbilitySequenceResetTime -= Time.deltaTime;
   //
   //         if(currentAbilitySequenceResetTime <=0.0f)
   //         {
   //             currentAbilitySequenceResetTime = 0.0f;
   //
   //             //reset the sequence index
   //             currentSequenceIndex = 0;
   //
   //             //sequence UI
   //         }
   //     }
   // }
   //
   //
   //
   //
   //
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
