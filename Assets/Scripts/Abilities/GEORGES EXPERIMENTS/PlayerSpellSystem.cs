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
    [Header("Main References")]
    [SerializeField, ReadOnly] Player owner = null;
    [SerializeField, ReadOnly] public GameObject currentAbilityCast = null;

    public E_AbilityUseState abilityUseState = E_AbilityUseState.Ready;

    [Header("Core Spell Variables")]
    public float classPowerCurrent = 0.0f;
    public float classPowerMax = 100.0f;
    public float GCD = 0.5f;
    [SerializeField, ReadOnly] float currentGCD = 0.0f;

    [Header("List of Ability Holders")]
    [SerializeField] List<AbilityDataHolder> abilityHolders = new List<AbilityDataHolder>();
    [SerializeField] KeyCode basicKey = KeyCode.Q;

    [Header("Basic Ability Sequence")]
    [SerializeField] List<AbilitySO> basicAbilitySequence = new List<AbilitySO>();
    [ReadOnly] int currentSequenceIndex = 0;

    [SerializeField] float abilitySequenceResetTime = 3.0f;
    [ReadOnly] float currentAbilitySequenceResetTime = 3.0f;

    [Header("Target Selection")]
    [SerializeField, ReadOnly] public Character target = null;
    [SerializeField, ReadOnly] public Vector3 targettedLocation = Vector3.zero;
    public RaycastHit predictionHit;
    public LayerMask enemyMask;
    public float predictionSphereCastRadius;
    public Transform predictionPoint;
    public float maxCastRange = 100.0f;


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


        //LINK WITH UI!!!!!
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

        //checks JUST THE BASIC - to improve
        if(abilityHolders.Count > 0)
        {
            if (Input.GetKeyDown(abilityHolders[0].keybind))
            {
                UseBasicAbility();
            }

            for(int i = 1; i < abilityHolders.Count; i++)
            {
                if (Input.GetKeyDown(abilityHolders[i].keybind))
                {
                    UseAbility(abilityHolders[i]);
                }
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
        if (abilityHolders[0].currentCooldown > 0.0f || currentGCD > 0.0f || !abilityHolders[0].active) { return; }


        //Instantiate and use the ability
        currentAbilityCast = abilityHolders[0].abilitySO.InitialiseAbility(owner,target,transform.position);
        
        //attempt to use basic ability sequence
        if (currentAbilityCast.GetComponent<Ability>().CastSpell(true))
        {

            //increment the sequence
            currentSequenceIndex++;
            if (currentSequenceIndex > basicAbilitySequence.Count - 1)
            {
                currentSequenceIndex = 0;
            }

            //set Ability0 to the current ability sequence
            abilityHolders[0].abilitySO = basicAbilitySequence[currentSequenceIndex];

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
        if (_abilityData == null || !_abilityData.abilitySO || !_abilityData.active) { return; }
        if (_abilityData.IsOnCooldown() || currentGCD > 0.0f) { return; }


        //instantiate ability, use ability, and start the cooldown
        currentAbilityCast = _abilityData.abilitySO.InitialiseAbility(owner, target, transform.position);
        
        //Attempt to cast the spell
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
            if (sHolder.IsOnCooldown())
            {
                sHolder.TickCoolDown(Time.deltaTime);
            }
        }
   
        //Basic attack sequence reset timer
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
                abilityHolders[0].abilitySO = basicAbilitySequence[currentSequenceIndex];

                //sequence UI
            }
        }
    }

    /***********************************************
   SpendClassPower: Spends class power if an ability requires it, and only if enough
   @author: George White
   @parameter: float _cost
   @return: bool
   ************************************************/
    public bool SpendClassPower(float _cost)
    {
        if(_cost > classPowerCurrent)
        {
            return false;
        }
        else
        {
            classPowerCurrent -= _cost;
            //update UI

            return true;
        }
    }

    /***********************************************
   RegenerateClassPower: Regenerates class power
   @author: George White
   @parameter: float _charge
   @return: void
   ************************************************/
    public void RegenerateClassPower(float _charge)
    {
        classPowerCurrent += _charge;

        if (classPowerCurrent > classPowerMax)
        {
            classPowerCurrent = classPowerMax;
        }

        //update UI
    }

    /***********************************************
   AssignTarget: Assigns target by raycasting/spherecasting
   @author: George White
   @tutorial: https://youtu.be/HPjuTK91MA8?si=9Eo0dKizRC-ol4pn&t=330
   @parameter: 
   @return: void
   ************************************************/
    public void AssignTarget()
    {
        if (abilityUseState != E_AbilityUseState.Ready) { return; }





        RaycastHit spherecastHit;
        Physics.SphereCast(owner.GetComponent<Player>().CMvcam.transform.position, predictionSphereCastRadius, owner.GetComponent<Player>().CMvcam.transform.forward, out spherecastHit, maxCastRange, enemyMask);

        RaycastHit raycastHit;
        Physics.Raycast(owner.GetComponent<Player>().CMvcam.transform.position, owner.GetComponent<Player>().CMvcam.transform.forward, out raycastHit, maxCastRange, enemyMask);

        Vector3 realHitPoint;

        //Direct hit onto enemy
        if(raycastHit.point != Vector3.zero && raycastHit.transform.tag != "Enemy")
        {
            realHitPoint = raycastHit.point;

            //assign ground target location
        }
        //Indirect hit onto enemy
        else if(spherecastHit.point != Vector3.zero && raycastHit.transform.tag != "Enemy")
        {
            realHitPoint = spherecastHit.point;

            //assign ground target location
        }
        //floor location of hit
        else if(raycastHit.point!= Vector3.zero )
        {
            realHitPoint = raycastHit.point;
        }
        //miss entirely
        else
        {
            realHitPoint = Vector3.zero;
        }


        if(realHitPoint != Vector3.zero)
        {
            predictionPoint.gameObject.SetActive(true);
            predictionPoint.position = realHitPoint;
        }
        else
        {
            predictionPoint.gameObject.SetActive(false);
        }

        predictionHit = raycastHit.point == Vector3.zero ? spherecastHit : raycastHit;
    }






}
