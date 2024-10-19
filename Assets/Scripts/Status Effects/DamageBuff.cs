using UnityEngine;

public class DamageBuff : StatusEffect
{
    public float dmgMultiplier;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        // Check whether a damage buff is already applied
        // If so, destroy it (refreshes duration)
        DamageBuff existingDmgBuff = target.GetComponentInChildren<DamageBuff>();
        if (existingDmgBuff != null && existingDmgBuff != this)
        {
            Destroy(existingDmgBuff.gameObject);
            return;
        }

        ApplyEffect();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /***********************************************
    * ApplyEffect: Applies damage multiplier to target's current damage multiplier.
    * @author: Justhine Nisperos
    * @parameter:
    * @return: void
    ************************************************/
    protected override void ApplyEffect()
    {
        target.damageMultiplier *= dmgMultiplier;
    }

    /***********************************************
    * RemoveEffect: Remove the damage buff from target's damage multiplier and destroys this effect.
    * @author: Justhine Nisperos
    * @parameter:
    * @return: void
    ************************************************/
    protected override void RemoveEffect()
    {
        target.damageMultiplier /= dmgMultiplier;

        base.RemoveEffect();
    }
}
