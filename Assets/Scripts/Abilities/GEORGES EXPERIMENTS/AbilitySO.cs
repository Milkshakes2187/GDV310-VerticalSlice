using MPUIKIT;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

[CreateAssetMenu(fileName = "AbilitySO", menuName = "Scriptable Objects/AbilitySO")]
public abstract class AbilitySO : ScriptableObject
{

    public string abilityName;
    public Sprite image;
    public AnimatorOverrideController animatorOverrideController;
    //Or at least a string with the ability animation override key?

    public float cooldownBase;
    //[HideInInspector] public float currentCooldown;

    //public float castTime;
    //public float activeTime;



    //ability
   // Ability abilityScript = null;
    public GameObject abilityPrefab = null;


    public GameObject InitialiseAbility(Character _owner, Character _target, Vector3 _targetLocation)
    {
        var ability = Instantiate(abilityPrefab, _owner.transform.position, Quaternion.identity);

        ability.owner = _owner;

        return ability;
    }


    //Ability = abilityPrefab.getCopoejiaodajoajw<Ability>()


    public abstract void Use(Character _ownerCharacter, Character _targetCharacter, Vector3 _targetLocation);







    public void StartCooldown()
    {
        currentCooldown = cooldownBase;
    }

    public void SetCurrentCooldown(float _cooldown)
    {
        currentCooldown = _cooldown;
    }

    public void TickCooldown(float _cooldownTicked)
    {
        //if ability is on cooldown, reduce by an amount
        if (currentCooldown > 0.0f)
        {
            currentCooldown -= _cooldownTicked;

            //if cooldown is less than 0, set it to zero
            if (currentCooldown < 0.0f)
            {
                currentCooldown = 0.0f;
            }
        }


        //update ui
    }

    public bool IsOnCooldown()
    {
        if(currentCooldown > 0.0f)
        {
           return true;
        }

        return false;
    }
}
