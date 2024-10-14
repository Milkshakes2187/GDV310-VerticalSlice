using UnityEngine;

public class BasicAbility : Ability
{
    //Gameobject that gets instantiated upon ability use
    [SerializeField] GameObject tempAttack = null;

    /***********************************************
   * UseSpellEffect: overridden function. Creates a sphere above the spellsystem temporarily
   * @author: George White
   * @parameter:
   * @return: override void
   ************************************************/
    public override void UseSpellEffect()
    {
         //temporary - while we have no animations. spawns a a cube above the player
         if(tempAttack)
         {
             var tempobj =  Instantiate(tempAttack, new Vector3(targetLocation.x, targetLocation.y + abilityData.primaryDamage, targetLocation.z), Quaternion.identity);
             Destroy(tempobj, 1);
         }

        //animation trigger, maybe do sword

        //Destroy the ability
        Destroy(gameObject);
    }

}
