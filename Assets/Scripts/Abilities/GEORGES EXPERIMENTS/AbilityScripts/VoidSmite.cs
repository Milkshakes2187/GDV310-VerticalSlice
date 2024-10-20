using UnityEngine;

public class VoidSmite : Ability
{

    //VFX
    [SerializeField] GameObject smiteVFX = null;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        requiresTarget = true;
    }


    public override bool VerifyTarget()
    {
        //if target DNE or is not an enemy, return false
        if (target == null || !target.gameObject.GetComponent<Enemy>()) { return false; }

        //range check
        if (Vector3.Distance(target.transform.position, owner.transform.position) > abilityData.primaryRange) 
        { 
            //out of range effect?

            return false; 
        }

        return true;
    }


    public override void InitialSetup()
    {
        //start animation here

        
        
    }



    public override void UseSpellEffect()
    {
        //armour buff vfx
        if (smiteVFX)
        {
            var vfx = Instantiate(smiteVFX, owner.gameObject.transform);
            Destroy(vfx, 3);
        }

        //DO THE DAMAGE
        target.gameObject.GetComponent<Enemy>().TakeDamage(abilityData.primaryDamage * owner.baseDamage);
        print("Dealt " + abilityData.primaryDamage * owner.baseDamage + " damage");
    }
}
