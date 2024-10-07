using MPUIKIT;
using UnityEngine;

[CreateAssetMenu(fileName = "NewScriptableObjectScript", menuName = "Scriptable Objects/NewScriptableObjectScript")]
public class AbilitySO : ScriptableObject
{

    public string abilityName;
    public Sprite image;

    public float cooldownTime;
    public float castTime;
    public float activeTime;


    public Ability ability;

    public void ActivateAbility()
    {
        
    }

    public void Spell()
    {
        ability.CastSpell();
    }
}
