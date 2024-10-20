//using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BasicAbility : Ability
{
    //Gameobject that gets instantiated upon ability use
    [SerializeField] GameObject tempAttack = null;

    Collider hitbox = null;


    bool onHitEffectTriggered = false;

    private void Start()
    {
        hitbox = GetComponentInChildren<Collider>();
        //hitbox.enabled = false;
    }

    public override void InitialSetup()
    {
        //start animation here


    }



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

        gameObject.transform.position = owner.gameObject.transform.position;
        gameObject.transform.rotation = owner.gameObject.transform.rotation;
        hitbox.enabled = true;


        //perform a spherecheck
        var cols =  Physics.OverlapSphere(hitbox.transform.position, hitbox.transform.localScale.x);

        //list to hold hit enemies
        List<GameObject> enemies = new List<GameObject>();

        //adding enemies that havent been hit yet into the list
        foreach(Collider col in cols)
        {
            //checkign for enemies in collider
            if(col.gameObject.GetComponent<Enemy>())
            {
               if(!enemies.Contains(col.gameObject.GetComponent<Enemy>().gameObject))
               {
                    enemies.Add(col.gameObject.GetComponent<Enemy>().gameObject);
               }
            }
            //Checkign for enemies in parent of collider
            if (col.gameObject.GetComponentInParent<Enemy>())
            {
                if (!enemies.Contains(col.gameObject.GetComponentInParent<Enemy>().gameObject))
                {
                    enemies.Add(col.gameObject.GetComponentInParent<Enemy>().gameObject);
                }
            }

        }

        //add spellcharge if anything was hit
        if(enemies.Count > 0)
        {
            owner.GetComponent<Player>().spellSystem.RegenerateSpellCharge(abilityData.castingCostGain);
        }

        foreach(GameObject enemy in enemies)
        {
            print("oh hi " + enemy.name);

            //DO THE DAMAGE
            enemy.gameObject.GetComponent<Enemy>().TakeDamage(abilityData.primaryDamage * owner.baseDamage);
            print("Dealt " + abilityData.primaryDamage * owner.baseDamage + " damage");
        }


        //Destroy the ability
        //Destroy(gameObject);
    }



    
}
