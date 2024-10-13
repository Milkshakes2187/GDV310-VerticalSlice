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

public class PlayerSpellSystem : MonoBehaviour
{

    public E_AbilityUseState abilityUseState = E_AbilityUseState.Ready;

    [Tab("Main spell tab")]

    [SerializeField] List<AbilitySO> basicAbilitySequence = new List<AbilitySO>();
    int currentSequenceIndex = 0;


    [SerializeField] AbilitySO ability1;
    [SerializeField] AbilitySO ability3;
    [SerializeField] AbilitySO ability2;
    [SerializeField] AbilitySO ability4;


    [Header("Keybinds")]
    [SerializeField] KeyCode abilityKeyBasic = KeyCode.Q;
    [SerializeField] KeyCode abilityKey1 = KeyCode.Alpha1;
    [SerializeField] KeyCode abilityKey2 = KeyCode.Alpha2;
    [SerializeField] KeyCode abilityKey3 = KeyCode.Alpha3;
    [SerializeField] KeyCode abilityKey4 = KeyCode.Alpha4;


    [SerializeField] float abilitySequenceResetTime = 3.0f;
     float currentAbilitySequenceResetTime = 3.0f;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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


        if (Input.GetKeyDown(abilityKeyBasic))
        {
            UseBasicAbility();
        }


        ability1.abilityPrefab.Use();
    }


    public void UseBasicAbility()
    {
        //cant cast spell if it is on cooldown
        if (basicAbilitySequence[currentSequenceIndex].IsOnCooldown()) { return; }

        basicAbilitySequence[currentSequenceIndex].Use(null, null, new Vector3 (transform.position.x, transform.position.y + 2 + (0.5f * currentSequenceIndex), transform.position.z));

        var aaAbility = basicAbilitySequence[currentSequenceIndex].InitialiseAbility();
        aaAbility.use();
       // aaAbility.Destroy(); To be done in the script

        //increment the sequence
        currentSequenceIndex++;
        if(currentSequenceIndex > basicAbilitySequence.Count - 1)
        {
            currentSequenceIndex = 0;
        }

        //starts the cooldown of the next ability
        basicAbilitySequence[currentSequenceIndex].StartCooldown();


        //start cooldowns for sequence resetting
        currentAbilitySequenceResetTime = abilitySequenceResetTime;

        //set ui
    }


    public void UseAbility(AbilitySO _abilityToUse)
    {
        if (!_abilityToUse) { return; }
        if (_abilityToUse.IsOnCooldown()) { return; }



        _abilityToUse.Use(null, null, Vector3.zero);

        _abilityToUse.StartCooldown();
    }


    void TickCooldowns()
    {
        //tick basic ability cooldown
        basicAbilitySequence[currentSequenceIndex].TickCooldown(Time.deltaTime);
        
        //tick all other cooldowns



        //Ability sequence reset time
        if(currentAbilitySequenceResetTime > 0.0f)
        {
            currentAbilitySequenceResetTime -= Time.deltaTime;

            if(currentAbilitySequenceResetTime <=0.0f)
            {
                currentAbilitySequenceResetTime = 0.0f;

                //reset the sequence index
                currentSequenceIndex = 0;

                //sequence UI
            }
        }
    }


    void interruptcastings()
    {
       // CancelInvoke castings ll''
    }

    IEnumerator CastChannelledSpell()
    {
        yield return null;
    }
}
