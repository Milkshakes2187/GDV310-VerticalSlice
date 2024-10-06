using UnityEngine;




[CreateAssetMenu(fileName = "AttackSO", menuName = "Scriptable Objects/AttackSO")]
public class AttackSO : ScriptableObject
{
    //animation overrider
    public AnimatorOverrideController animOverrider;

    // Animation speed
    public float animSpeed;

    //Slash animation
    public GameObject attackEffect;

    //damage for the attack
    public float fAttackDamage;

    //Which sword to use
    //public E_ActiveSword eActiveSword;
}
