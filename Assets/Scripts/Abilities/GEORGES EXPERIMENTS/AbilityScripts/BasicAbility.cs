//using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BasicAbility : Ability
{
    //Signle hitbox that will be used for a physics area check
    Collider hitbox = null;
    [SerializeField] GameObject VFX = null;

    private void Start()
    {
        hitbox = GetComponentInChildren<Collider>();
        hitbox.enabled = false;
    }

    public override void InitialSetup()
    {
        //start animation here

        //VFX spawning
        if(VFX)
        {
            var vfx = Instantiate(VFX, owner.gameObject.transform);
            Destroy(vfx,3);
        }
    }


    /***********************************************
   * UseSpellEffect: overridden function. Triggers after a "cast time". Single target damage.
   * @author: George White
   * @parameter:
   * @return: override void
   ************************************************/
    public override void UseSpellEffect()
    {

        //set the position of the spell to be at the location of the player
        gameObject.transform.position = owner.gameObject.transform.position;
        gameObject.transform.rotation = owner.gameObject.transform.rotation;


        //DEBUG LINE
        GetComponentInChildren<MeshRenderer>().enabled = true;


        //list to hold hit enemies
        List<GameObject> enemies = new List<GameObject>();

        //list to hold collisions
        List<Collider> collisions = new List<Collider>();


        //IF clause depending on type of collider
        //perform a spherecheck
        if (hitbox.GetType() == typeof(SphereCollider))
        {
            foreach (Collider c in Physics.OverlapSphere(hitbox.transform.position, hitbox.GetComponent<SphereCollider>().radius * hitbox.gameObject.transform.localScale.x))
            {
                collisions.Add(c) ;
                print(hitbox.GetComponent<SphereCollider>().radius * hitbox.gameObject.transform.localScale.x);
            }
        }
        //perform a boxcheck
        else if(hitbox.GetType() == typeof(BoxCollider))
        {
            foreach (Collider c in Physics.OverlapBox(hitbox.transform.position, hitbox.gameObject.transform.localScale * 0.5f, hitbox.gameObject.transform.rotation))
            {
                collisions.Add(c);
            }
        }
        //perform a capsulecheck
        else if (hitbox.GetType() == typeof(CapsuleCollider))
        {
            foreach (Collider c in Physics.OverlapCapsule(  hitbox.transform.position + new Vector3(0f, hitbox.GetComponent<CapsuleCollider>().height / 2f * hitbox.gameObject.transform.localScale.y) + hitbox.GetComponent<CapsuleCollider>().center,
                                                            hitbox.transform.position - new Vector3(0f, hitbox.GetComponent<CapsuleCollider>().height / 2f * hitbox.gameObject.transform.localScale.y) + hitbox.GetComponent<CapsuleCollider>().center,
                                                            hitbox.GetComponent<CapsuleCollider>().radius * hitbox.gameObject.transform.localScale.x))
            {
                collisions.Add(c);
            }
        }

        //adding enemies that havent been hit yet into the list
        foreach (Collider col in collisions)
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
            owner.GetComponent<Player>().spellSystem.RegenerateClassPower(abilityData.castingCostGain);
        }

        //Damage each enemy in the list
        foreach(GameObject enemy in enemies)
        {
            print("oh hi " + enemy.name);

            //DO THE DAMAGE
            //enemy.gameObject.GetComponent<Enemy>().TakeDamage(abilityData.primaryDamage * owner.baseDamage);
            print("Dealt " + abilityData.primaryDamage * owner.baseDamage + " damage");
        }

        //Destroy the ability
        Destroy(gameObject, 2);
    }
}
