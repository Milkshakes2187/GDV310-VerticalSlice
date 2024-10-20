using UnityEngine;

public class BasicAbility : Ability
{
    //Gameobject that gets instantiated upon ability use
    [SerializeField] GameObject tempAttack = null;

    Collider hitbox = null;


    bool onHitEffectTriggered = false;

    private void Start()
    {
        //hitbox = GetComponent<Collider>();
       // hitbox.enabled = false;
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
             //var tempobj =  Instantiate(tempAttack, new Vector3(targetLocation.x, targetLocation.y + abilityData.primaryDamage, targetLocation.z), Quaternion.identity);
            // Destroy(tempobj, 1);


            

         }

        //hitbox.enabled = true;



        //Destroy the ability
        Destroy(gameObject);
    }



    private void OnCollisionEnter(Collision collision)
    {
        print("HIT AN ENEMY");

        if(!onHitEffectTriggered)
        {
            owner.GetComponent<Player>().spellSystem.RegenerateSpellCharge(abilityData.castingCostGain);
            onHitEffectTriggered =true;
        }
        

        //if the hit collider is a character and not a player
        if(collision.gameObject.GetComponentInParent<Character>() && !collision.gameObject.GetComponentInParent<Player>())
        {
            collision.gameObject.GetComponentInParent<Character>().TakeDamage(abilityData.primaryDamage * owner.baseDamage);
        }
    }


}
