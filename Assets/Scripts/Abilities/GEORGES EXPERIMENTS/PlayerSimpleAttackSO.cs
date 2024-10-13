using System.Data.Common;
using UnityEngine;
using VHierarchy.Libs;


[CreateAssetMenu(fileName = "PlayerSimpleAttackSO", menuName = "Scriptable Objects/PlayerSimpleAttackSO")]
public class PlayerSimpleAttackSO : AbilitySO
{

    [Header("SimpleAttackVariables")]
    [SerializeField] float damage = 0.0f;
    [SerializeField] GameObject tempAttack = null;




   // public override void Use(Character _ownerCharacter, Character _targetCharacter, Vector3 _targetLocation)
   // {
   //
   //     //temporary - while we have no animations. spawns a a cube above the player
   //     if(tempAttack)
   //     {
   //         var tempobj =  Instantiate(tempAttack, _targetLocation, Quaternion.identity);
   //         Destroy(tempobj, 1);
   //     }
   //
   //
   //     damage++;
   // }

    



}
