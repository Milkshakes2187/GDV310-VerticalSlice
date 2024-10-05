using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;
using UnityEngine.VFX;


public enum E_ActiveSword
{
    LEFT,
    RIGHT,
    DUAL,
    HEAVY,
    NONE,
}

public enum E_AttackType
{
    LIGHT,
    HEAVY,
    ABILITY1,
    ABILITY2,
}

public class PlayerCombat : MonoBehaviour
{
    public static PlayerCombat instance;

    [Tab("Main")]
    [SerializeField] KeyCode Ability1Key = KeyCode.E;
    [SerializeField] KeyCode ChargeAttackKey = KeyCode.Mouse1;

    public bool bIsAttacking;
    public bool bIsSwinging = false;

    public bool bIsChargingHeavy = false;
    public bool bIsHeavyAttacking = false;

    bool heavyMouseIsReleased = false;
    bool heavyAnimQuickAttackThresholdReached = false;
    bool heavyAnimMinThresholdReached = false;

    public float fHeavyChargeTime = 0.0f;
    public float fHeavyChargeStage = 0.0f;
    public float fHeavySpinDamageMultiplier = 0.9f;
    public float fHeavySlashDamageMultiplier = 1.3f;

    public List<AttackSO> attackSequence;
    public AttackSO heavyCharge;
    public AttackSO heavyRelease;

    [SerializeField, ReadOnly]int iSequenceIndex = 0;

    [SerializeField, ReadOnly] bool bLightSwingFinished = true;

    float fPreviousInputTime;
    float fPreviousSequenceEnd;


    //[SerializeField] float SequenceEndDelay = 1.0f;
   // [SerializeField] float SequenceEndDelay = 1.0f;


    [Tab("References")]
    Animator animator;

    [SerializeField] Sword leftSword;
    [SerializeField] Sword rightSword;

    [SerializeField] GameObject HeavyChargeStage1Effect;
    [SerializeField] GameObject HeavyChargeStage2Effect;
    [SerializeField] GameObject HeavyChargeStage3Effect;
    [SerializeField] GameObject HeavySpinEffect;
    [SerializeField] GameObject HeavySlashEffect;

    [SerializeField] GameObject Ability1Effect;

    [Tab("Abilities")]
    //Ability 1 effect
    public bool bLifestealActive = false;
    public float fLifestealPercent = 0.3f;
    public float fAbility1EffectDurationMax = 5.0f;
    public float fAbility1EffectDurationCurrent = 0.0f;

    //Ability 1 cooldown
    public float fAbility1CooldownMax = 15.0f;
    public float fAbility1CooldownCurrent = 0.0f;
    public bool fAbility1OnCooldown = false;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(GetComponent<Animator>()!= null)
        {
            animator = GetComponent<Animator>();
        }

        bLifestealActive = false;

        //turning off the swords
        ActivateSwordHitboxes(E_ActiveSword.NONE);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.B) )
        {
           // AudioSource1.PlayOneShot(AudioSource1.clip, 1);
           // print("oh");
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
           // AudioSource2.PlayOneShot(AudioSource2.clip, 1);
        }
        

        //charge heavy time
        if (bIsChargingHeavy)
        {
            fHeavyChargeTime += Time.deltaTime;
        }

        if (Input.GetButtonDown("Fire1") || Input.GetButton("Fire1"))
        {
            LightAttack();
        }

        if (Input.GetKeyDown(ChargeAttackKey))
        {
            InitiateHeavyAttack();
        }

        if(Input.GetKeyUp(ChargeAttackKey))
        {
            heavyMouseIsReleased = true;

            ReleaseHeavyAttack();
           
        }

        if (Input.GetKeyDown(Ability1Key))
        {
            Ability1();
        }

        EndAttack();
    }

   // alter the weapon's damage according to the sequence
    void AssignWeaponDamage(E_ActiveSword _activeSword)
    {
    
        switch (_activeSword)
        {
            case E_ActiveSword.LEFT:
                leftSword.fBaseDamage = attackSequence[iSequenceIndex].fAttackDamage;
                break;
    
            case E_ActiveSword.RIGHT:
                rightSword.fBaseDamage = attackSequence[iSequenceIndex].fAttackDamage;
                break;
            case E_ActiveSword.DUAL:
                leftSword.fBaseDamage = attackSequence[iSequenceIndex].fAttackDamage;
                rightSword.fBaseDamage = attackSequence[iSequenceIndex].fAttackDamage;

                break;
            case E_ActiveSword.HEAVY:
                leftSword.fBaseDamage = attackSequence[iSequenceIndex].fAttackDamage;
                rightSword.fBaseDamage = attackSequence[iSequenceIndex].fAttackDamage;
                break;
            default:
                break;
    
        }
    
    }

    void ActivateSwordHitboxes(E_ActiveSword _activeSword)
    {
        switch (_activeSword)
        {
            case E_ActiveSword.LEFT:
                leftSword.EnableHitbox();

                rightSword.DisableHitbox();
                break;

            case E_ActiveSword.RIGHT:
                rightSword.EnableHitbox();

                leftSword.DisableHitbox();
                break;
            case E_ActiveSword.DUAL:

                leftSword.EnableHitbox();
                rightSword.EnableHitbox();

                break;
            case E_ActiveSword.HEAVY:
                leftSword.EnableHitbox();
                rightSword.EnableHitbox();
                break;
            case E_ActiveSword.NONE:

                leftSword.DisableHitbox();
                rightSword.DisableHitbox();

                break;
            default:
                break;

        }
    }

    //perform a light attack (sequenced)
    void LightAttack()
    {
        //checking if the sequence is still active
        //if (Time.time - fPreviousSequenceEnd > 0.15f && iSequenceIndex < attackSequence.Count)
        if (iSequenceIndex < attackSequence.Count && !bIsChargingHeavy && !bIsHeavyAttacking && bLightSwingFinished)
        {

            Player.instance.TurnOffCanMove();
            //prevents sequence from ending
            //CancelEnd();
            CancelInvoke("EndSequence");

            //perform the attack, checkign the timing from the last input
            // if (Time.time - fPreviousInputTime >= 0.6f)  //TIME GATES BETWEEN ATTACKS
            if (!bIsSwinging)  //TIME GATES BETWEEN ATTACKS
            {

                bIsSwinging = true;

                //play selected animation according to the current attack sequence animation
                animator.runtimeAnimatorController = attackSequence[iSequenceIndex].animOverrider;
                animator.SetFloat("AnimMultiplier", attackSequence[iSequenceIndex].animSpeed);
                animator.Play("Attack", 0, 0);
                bLightSwingFinished = false;


                //assign the current weapon damage according to the atack sequence value
                AssignWeaponDamage(attackSequence[iSequenceIndex].eActiveSword);
               

                //increase sequence count
                iSequenceIndex++;
                fPreviousInputTime = Time.time;

                //reset sequence if max strike is performed
                if(iSequenceIndex >= attackSequence.Count)
                {
                    iSequenceIndex = 0;
                }
            }
        }

        else if(bIsChargingHeavy && heavyAnimQuickAttackThresholdReached)
        {
            QuickReleaseHeavyAttack();
        }
    }

    #region Heavy
    void InitiateHeavyAttack()
    {
        if(!bIsSwinging)
        {
            Player.instance.TurnOffCanMove();

            fHeavyChargeTime = 0.0f;
            fHeavyChargeStage = 0;
            bIsChargingHeavy = true;

            bIsSwinging = true;
            heavyMouseIsReleased = false;

            //Animation threshold gates
            heavyAnimMinThresholdReached = false;
            heavyAnimQuickAttackThresholdReached = false;


            //play selected animation according to the current attack sequence animation
            animator.runtimeAnimatorController = heavyCharge.animOverrider;
            animator.SetFloat("AnimMultiplier", heavyCharge.animSpeed);
            animator.Play("Attack", 0, 0);

        }
    }

    public void AnimQueryQuickRelease()
    {
        heavyAnimQuickAttackThresholdReached = true;

        if(heavyMouseIsReleased)
        {
            QuickReleaseHeavyAttack();
        }

    }

    //animation event checks if the mouse is released, then attempts to end the heavy charge
    public void AnimQueryHeavyRelease()
    {
        //slow animation for last half of charge
        animator.SetFloat("AnimMultiplier", 0.7f);

        heavyAnimMinThresholdReached = true;
    }

    public void ReleaseHeavyAttack()
    {
        //skips if already heavy attacking
        if (!bIsChargingHeavy) {return; }
        if (!heavyAnimMinThresholdReached)
        {
            QuickReleaseHeavyAttack();
            return;
        }
        
        Player.instance.TurnOffCanMove();
        CancelInvoke("EndSequence");

        animator.SetFloat("AnimMultiplier", heavyCharge.animSpeed);
        heavyMouseIsReleased = false;
        heavyAnimMinThresholdReached = false;
        heavyAnimQuickAttackThresholdReached = false;

        bIsChargingHeavy = false;
        bIsHeavyAttacking = true;
        
        //play selected animation according to the current attack sequence animation
        animator.runtimeAnimatorController = heavyRelease.animOverrider;
        animator.SetFloat("AnimMultiplier", heavyRelease.animSpeed);
        animator.Play("Attack", 0, 0);

        bLightSwingFinished = false;

    }


    public void QuickReleaseHeavyAttack()
    {
        //resetting ALL POSSIBLE VALUES SO I DONT GET TRIPPPED UP SOMEWHERE
        bIsSwinging = false;
        heavyMouseIsReleased = false;
        heavyAnimQuickAttackThresholdReached = false;
        heavyAnimMinThresholdReached = false;
        bIsChargingHeavy = false;

        //Setting up the correct light attack
        iSequenceIndex = 3;
        LightAttack();
    }


    //plays effects and changes charge up damage according to charge time
    public void AnimHeavyChargeStage(int _stage)
    {
        
        if (_stage == 1)
        {
            fHeavyChargeStage = 1.0f;

            //TEDDY SOUND Charge up stage 1
            if (AudioLibrary.instance.audioChargedStage1.clip)
            {
                AudioLibrary.instance.audioChargedStage1.PlayOneShot(AudioLibrary.instance.audioChargedStage1.clip, 1);
            }

            //spawning a slash effect
            if (HeavyChargeStage1Effect != null)
            {
                var effect = Instantiate(HeavyChargeStage1Effect, transform.position, transform.rotation);
                Destroy(effect, 5);
            }
        }
        else if(_stage == 2)
        {
            fHeavyChargeStage = 2.0f;

            //TEDDY SOUND charge up stage 2
            if (AudioLibrary.instance.audioChargedStage2.clip)
            {
                AudioLibrary.instance.audioChargedStage2.PlayOneShot(AudioLibrary.instance.audioChargedStage2.clip, 1);
            }

            //spawning a slash effect
            if (HeavyChargeStage2Effect != null)
            {
                var effect = Instantiate(HeavyChargeStage2Effect, transform.position, transform.rotation);
                Destroy(effect, 5);
            }
        }
        else if(_stage == 3)
        {
            fHeavyChargeStage = 3.0f;

            //TEDDY SOUND charge up stage 3
            if (AudioLibrary.instance.audioChargedStage3.clip)
            {
                AudioLibrary.instance.audioChargedStage3.PlayOneShot(AudioLibrary.instance.audioChargedStage3.clip, 1);
            }

            //spawning a slash effect
            if (HeavyChargeStage3Effect != null)
            {
                var effect = Instantiate(HeavyChargeStage3Effect, transform.position, transform.rotation);
                Destroy(effect, 5);
            }
        }
    }

    //activate slash effect and sword hitboxes/damage for first half of heavy attack
    public void AnimTriggerHeavySpin()
    {
        float modifiedDamage = heavyRelease.fAttackDamage * fHeavyChargeStage * fHeavySpinDamageMultiplier;
        leftSword.fBaseDamage = modifiedDamage;
        rightSword.fBaseDamage = modifiedDamage;

        leftSword.ScaleUpHitbox();
        rightSword.ScaleUpHitbox();


        //activating the sword hitboxes
        ActivateSwordHitboxes(heavyRelease.eActiveSword);

        //SOUND heavy attack spinnnn
        if (AudioLibrary.instance.audioChargedSpinAttack.clip)
        {
            AudioLibrary.instance.audioChargedSpinAttack.PlayOneShot(AudioLibrary.instance.audioChargedSpinAttack.clip, 1);
        }
        

        //effect
        if (HeavySpinEffect != null)
        {
            var effect = Instantiate(HeavySpinEffect, transform.position, transform.rotation);
            Destroy(effect, 6);
        }
    }

    //activate slash effect and sword hitboxes/damage for second half of heavy attack
    public void AnimTriggerHeavySlash()
    {


        Player.instance.TurnOffCanMove();
        float modifiedDamage = heavyRelease.fAttackDamage * fHeavyChargeStage * fHeavySpinDamageMultiplier;
        leftSword.fBaseDamage = modifiedDamage;
        rightSword.fBaseDamage = modifiedDamage;

        leftSword.ScaleUpHitbox();
        rightSword.ScaleUpHitbox();

        //activating the sword hitboxes
        ActivateSwordHitboxes(heavyRelease.eActiveSword);

        if(fHeavyChargeStage == 3)
        {
            //SOUND heavy attack bigoldslash
            if (AudioLibrary.instance.audioHeavyImpact.clip)
            {
                AudioLibrary.instance.audioHeavyImpact.PlayOneShot(AudioLibrary.instance.audioHeavyImpact.clip, 1);
            }
        }
        else
        {
            //SOUND heavy attack bigoldslash
            if (AudioLibrary.instance.audioChargedSlashAttack.clip)
            {
                AudioLibrary.instance.audioChargedSlashAttack.PlayOneShot(AudioLibrary.instance.audioChargedSlashAttack.clip, 1);
            }
        }
        

        //effect
        if (HeavySlashEffect != null)
        {
            var effect = Instantiate(HeavySlashEffect, transform.position, transform.rotation);
            Destroy(effect, 6);
        }
    }

    #endregion


    //making a slashing effect
    public void SlashEffect()
    {
        Player.instance.TurnOffCanMove();
        //selectin the correct current sequence
        int CurrentSequence = 0;
        if (iSequenceIndex == 0)
        {
            CurrentSequence = attackSequence.Count - 1 ;
        }
        else
        {
            CurrentSequence = iSequenceIndex - 1;
        }

        //activating the sword hitboxes
        ActivateSwordHitboxes(attackSequence[CurrentSequence].eActiveSword);


        // SOUND
        if (AudioLibrary.instance.audioLightAttacks[CurrentSequence].clip)
        {
            AudioLibrary.instance.audioLightAttacks[CurrentSequence].PlayOneShot(AudioLibrary.instance.audioLightAttacks[CurrentSequence].clip, 1);
        }



        //spawning a slash effect
        if (attackSequence[CurrentSequence].attackEffect != null)
        {
            var effect = Instantiate(attackSequence[CurrentSequence].attackEffect, transform.position, transform.rotation);
            Destroy(effect, 5);
        }
    }

    
    void Ability1()
    {
        if(!fAbility1OnCooldown )
        {
            // TEDDY SOUND Ability 1 sound
            if (AudioLibrary.instance.audioAbility1.clip)
            {
                AudioLibrary.instance.audioAbility1.PlayOneShot(AudioLibrary.instance.audioAbility1.clip, 1);
            }

            //start the effects
            StartCoroutine(Ability1EffectDuration());
            //start the cooldown
            StartCoroutine(AbilityCooldown(1));
        }
    }


    void EndAttack()
    {
        if(bIsChargingHeavy)
        {
            CancelInvoke("EndSequence");
            return;
        }
        else if(bIsHeavyAttacking)
        {
            print(animator.GetCurrentAnimatorStateInfo(0).normalizedTime);

            //BANDAID FIX, look for better for VERT
            if (bLightSwingFinished)
            {
                EndHeavyAttack();
            }
            
        }
        // Check if animation is the attack anim and is over 90% done
       //else if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f && 
       //    animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")  )
       else if(bLightSwingFinished)
        {
            //allow player movement again
            bIsSwinging = false;
            ActivateMove();
            // End attack sequence after 1sec for leeway after animation
            Invoke("EndSequence", 0.58f);
        }
    }
    
    public void EndHeavyAttack()
    {
        EndSequence();
        ActivateMove();
        animator.Play("anim_pIdle", 0, 0);
        CancelInvoke("EndSequence");
    }

    

    void EndSequence()
    {
        bIsChargingHeavy = false;
        bIsHeavyAttacking = false;
        bIsSwinging = false;
        bLightSwingFinished = true;
        ActivateMove();
        
        iSequenceIndex = 0;
        fPreviousSequenceEnd = Time.time;
        ActivateSwordHitboxes(E_ActiveSword.NONE);
    }

    

    public void EndSwing()
    {
        bIsSwinging = false;
        bLightSwingFinished = true;
        animator.Play("anim_pIdle", 0, 0);
    }


    //turns off sword hitboxes early - animation event
    public void AnimTriggerTurnOffSwordHitbox()
    {
        ActivateSwordHitboxes(E_ActiveSword.NONE);
    }

    public void InterruptAttack()
    {

    }


    void ActivateMove()
    {
          if(!bIsSwinging)
          {
            Player.instance.bCanMove = true;
           
        }
        
    }


    #region AbilityCoroutines
    IEnumerator Ability1EffectDuration()
    {
        bLifestealActive = true;


        // SOUND
        if (AudioLibrary.instance.audioAbility1.clip)
        {
            AudioLibrary.instance.audioAbility1.PlayOneShot(AudioLibrary.instance.audioAbility1.clip, 1);
        }


        //spawning ability efefct
        if (Ability1Effect != null)
        {
            Ability1Effect.GetComponent<VisualEffect>().Play();
        }
        else
        {
            print("no effect:(");
        }

        fAbility1EffectDurationCurrent = 0.0f;


        while (fAbility1EffectDurationCurrent < fAbility1EffectDurationMax)
        {
            fAbility1EffectDurationCurrent += Time.deltaTime;
            yield return null;
        }

        fAbility1EffectDurationCurrent = fAbility1EffectDurationMax;


        bLifestealActive = false;
        Ability1Effect.GetComponent<VisualEffect>().Stop();
    }

    IEnumerator AbilityCooldown(int _abilityNumber)
    {

        if(_abilityNumber == 1)
        {
            
            fAbility1OnCooldown = true;
            fAbility1CooldownCurrent = 0.0f;
            
            
            while(fAbility1CooldownCurrent < fAbility1CooldownMax)
            {
                fAbility1CooldownCurrent += Time.deltaTime;
                yield return null;
            }
            
            fAbility1CooldownCurrent = fAbility1CooldownMax;
            
            fAbility1OnCooldown = false;
            Debug.Log("Ability1OffCooldown");
        }
    }
    #endregion
}
