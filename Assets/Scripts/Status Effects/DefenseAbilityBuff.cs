using UnityEngine;

public class DefenseAbuilityBuff : StatusEffect
{
    [Tooltip("1 Armour = 1% less damage.")]
    public float addedArmour;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        // Check whether a tanky effect is already applied
        // If so, destroy it (refreshes duration)
        DefenseAbuilityBuff existingDAbuff = target.GetComponentInChildren<DefenseAbuilityBuff>();
        if (existingDAbuff != null && existingDAbuff != this)
        {
            Destroy(existingDAbuff.gameObject);
            return;
        }

        ApplyEffect();
    }

    /***********************************************
    * ApplyEffect: Applies added armour to target's current armour.
    * @author: Justhine Nisperos
    * @parameter:
    * @return: void
    ************************************************/
    protected override void ApplyEffect()
    {
        target.armour += addedArmour;
    }

    /***********************************************
    * RemoveEffect: Removes added armour from target's current armour and destroys this effect.
    * @author: Justhine Nisperos
    * @parameter:
    * @return: void
    ************************************************/
    protected override void RemoveEffect()
    {
        target.armour -= addedArmour;

        base.RemoveEffect();
    }
}
