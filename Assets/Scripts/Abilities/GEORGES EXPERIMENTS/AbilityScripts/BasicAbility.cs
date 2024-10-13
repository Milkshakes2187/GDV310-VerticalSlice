using UnityEngine;

public class BasicAbility : Ability
{

    [SerializeField] float damage = 0.0f;
    [SerializeField] GameObject tempAttack = null;


    //Temporary - use the players basic ability
    public override void UseSpellEffect()
    {
        
          //temporary - while we have no animations. spawns a a cube above the player
          if(tempAttack)
          {
              var tempobj =  Instantiate(tempAttack, new Vector3(targetLocation.x, targetLocation.y + damage, targetLocation.z), Quaternion.identity);
              Destroy(tempobj, 1);
          }


        //animation trigger, maybe do sword


        Destroy(gameObject);
    }

}
