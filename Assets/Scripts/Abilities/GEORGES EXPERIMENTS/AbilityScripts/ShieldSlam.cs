using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.Rendering.DebugUI;

public class ShieldSlam : Ability
{
    //Gameobject that gets instantiated upon ability use
    [SerializeField] GameObject defenceStatusPF = null;

    List<Collider> colliders = new List<Collider>();


    bool onHitEffectTriggered = false;

    private void Start()
    {
        foreach (Collider collider in GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
            colliders.Add(collider);
        }
    }

    public override void InitialSetup()
    {
        //start animation here

        var slowStatus = Instantiate(defenceStatusPF, owner.gameObject.transform);

        defenceStatusPF.GetComponent<DefenseAbuilityBuff>().duration = abilityData.primaryEffectDuration;
        defenceStatusPF.GetComponent<DefenseAbuilityBuff>().addedArmour = abilityData.primaryEffectStrength;
    }



    /***********************************************
   * UseSpellEffect: overridden function. Creates a sphere above the spellsystem temporarily
   * @author: George White
   * @parameter:
   * @return: override void
   ************************************************/
    public override void UseSpellEffect()
    {

        //setting origin of ability
        gameObject.transform.position = owner.gameObject.transform.position;
        gameObject.transform.rotation = owner.gameObject.transform.rotation;


        //DEBUG LINE
        foreach(MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
        {
            mr.enabled = true;
        }


        //list to hold hit enemies
        List<GameObject> enemies = new List<GameObject>();
        List<Collider> collisions = new List<Collider>();


        //performs a physics area check for all colliders in ability
        foreach (Collider col in colliders)
        {
            //IF clause depending on type of collider
            //perform a spherecheck
            if (col.GetType() == typeof(SphereCollider))
            {
                foreach (Collider c in Physics.OverlapSphere(col.transform.position, col.GetComponent<SphereCollider>().radius * col.gameObject.transform.localScale.x))
                {
                    collisions.Add(c);
                    print(col.GetComponent<SphereCollider>().radius * col.gameObject.transform.localScale.x);
                }
            }
            //perform a boxcheck
            else if (col.GetType() == typeof(BoxCollider))
            {
                foreach (Collider c in Physics.OverlapBox(col.transform.position, col.gameObject.transform.localScale * 0.5f, col.gameObject.transform.rotation))
                {
                    collisions.Add(c);
                }
            }
            //perform a capsulecheck
            else if (col.GetType() == typeof(CapsuleCollider))
            {
                foreach (Collider c in Physics.OverlapCapsule(col.transform.position + new Vector3(0f, col.GetComponent<CapsuleCollider>().height / 2f * col.gameObject.transform.localScale.y) + col.GetComponent<CapsuleCollider>().center,
                col.transform.position - new Vector3(0f, col.GetComponent<CapsuleCollider>().height / 2f * col.gameObject.transform.localScale.y) + col.GetComponent<CapsuleCollider>().center,
                col.GetComponent<CapsuleCollider>().radius * col.gameObject.transform.localScale.x))
                {
                    collisions.Add(c);
                }
            }
        }

        
        //Deal the damage
        foreach (GameObject enemy in enemies)
        {
            print("oh hi " + enemy.name);

            //DO THE DAMAGE
            enemy.gameObject.GetComponent<Enemy>().TakeDamage(abilityData.primaryDamage * owner.baseDamage);
            print("Dealt " + abilityData.primaryDamage * owner.baseDamage + " damage");
        }


        //Destroy the ability
        Destroy(gameObject, 2);
    }
}
