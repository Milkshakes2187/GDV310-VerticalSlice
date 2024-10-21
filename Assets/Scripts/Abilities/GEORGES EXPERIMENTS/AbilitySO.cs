using MPUIKIT;
using Unity.VisualScripting;
using UnityEngine;
using VInspector;

[CreateAssetMenu(fileName = "AbilitySO", menuName = "Scriptable Objects/AbilitySO")]
public  class AbilitySO : ScriptableObject
{
    [Header("----Core Ability Assets----")]
    public string abilityName;
    public Sprite image;
    public AnimatorOverrideController animatorOverrideController;   //Or at least a string with the ability animation override key?]

    //prefab of ability that the scriptable object references
    public GameObject abilityPrefab = null;

    [Header("----Ability variables----")]
    [Header("Time")]
    public float cooldown;
    public float timeToCast;
    public float timeToChannel;

    [Header("Cost")]
    public float castingCost;
    public float castingCostGain;
    public float CastingCostMultiplier;

    [Header("Damage")]
    public float primaryDamage; 
    public float secondaryDamage; 
    public float tertiaryDamage;
    [Header("Effect Duration")]
    public float primaryEffectDuration;
    public float secondaryEffectDuration;
    public float tertiaryEffectDuration;
    [Header("Effect Strength")]
    public float primaryEffectStrength;
    public float secondaryEffectStrength;
    public float tertiaryEffectStrength;
    [Header("Range")]
    public float primaryRange;
    public float secondaryRange;
    [Header("Text")]
    public string info;

    /***********************************************
    * InitialiseAbility: Instantiates an ability so it is ready to use, assigning relevant variables
    * @author: George White
    * @parameter: Character , Character , Vector3 
    * @return: GameObject
    ************************************************/
    public GameObject InitialiseAbility(Character _owner, Character _target, Vector3 _targetLocation)
    {
        //instantiating ability
        var ability = Instantiate(abilityPrefab, _targetLocation, Quaternion.identity);

        //initialising member variables
        ability.GetComponent<Ability>().owner = _owner;
        ability.GetComponent<Ability>().target = _target;
        ability.GetComponent<Ability>().targetLocation = _targetLocation;
        ability.GetComponent<Ability>().abilityData = this;


        //return the gameobject
        return ability;
    }
}
