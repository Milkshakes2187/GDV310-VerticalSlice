using MPUIKIT;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

[CreateAssetMenu(fileName = "AbilitySO", menuName = "Scriptable Objects/AbilitySO")]
public abstract class AbilitySO : ScriptableObject
{
    //Basic variables
    public string abilityName;
    public Sprite image;
    public AnimatorOverrideController animatorOverrideController;   //Or at least a string with the ability animation override key?


    public float cooldown; //TODO is this needed?


    //prefab of ability that the scriptable object references
    public GameObject abilityPrefab = null;


    /***********************************************
   * InitialiseAbility: Instantiates an ability so it is ready to use, assigning relevant variables
   * @author: George White
   * @parameter: Character , Character , Vector3 
   * @return: GameObject
   ************************************************/
    public GameObject InitialiseAbility(Character _owner, Character _target, Vector3 _targetLocation)
    {
        //instantiating ability
        var ability = Instantiate(abilityPrefab, _owner.transform.position, Quaternion.identity);

        //initialising member variables
        ability.GetComponent<Ability>().owner = _owner;
        ability.GetComponent<Ability>().target = _target;
        ability.GetComponent<Ability>().targetLocation = _targetLocation;
        ability.GetComponent<Ability>().abilityData = this;

        //return the gameobject
        return ability;
    }
}
