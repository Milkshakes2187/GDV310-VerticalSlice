using Unity.VisualScripting;
using UnityEngine;

public class Slow : StatusEffect
{
    public float slowMultiplier;

    protected override void Start()
    {
        base.Start();

        // Check whether a slow is already applied
        // If so, destroy it (refreshes duration)
        Slow existingSlow = target.GetComponentInChildren<Slow>();
        if (existingSlow != null && existingSlow != this)
        {
            Destroy(existingSlow.gameObject);
            return;
        }

        ApplyEffect();
    }

    /***********************************************
    * ApplyEffect: Applies slow multiplier to target's current speed multiplier.
    * @author: Justhine Nisperos
    * @parameter:
    * @return: void
    ************************************************/
    protected override void ApplyEffect()
    {
        // Apply this multiplier to current speed multiplier
        // Allows speed-related status effects to stack
        target.speedMultiplier *= slowMultiplier;
    }

    /***********************************************
    * RemoveEffect: Remove the slow multiplier from target's speed multiplier and destroys this effect.
    * @author: Justhine Nisperos
    * @parameter:
    * @return: void
    ************************************************/
    protected override void RemoveEffect()
    {
        target.speedMultiplier /= slowMultiplier;  // Reset target's movespeed multiplier

        base.RemoveEffect();
    }
}
