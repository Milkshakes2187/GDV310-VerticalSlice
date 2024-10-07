using UnityEngine;

public class Slow : StatusEffect
{
    public float slowMultiplier = 0.5f;

    protected override void Start()
    {
        base.Start();

        // Check whether a slow is already applied
        // If so, destroy it (refreshes duration)
        Slow existingSlow = target.GetComponentInChildren<Slow>();
        if (existingSlow != null && existingSlow != this)
        {
            Destroy(existingSlow.gameObject);
        }

        ApplyEffect();
    }

    /***********************************************
    * ApplyEffect: Applies the status effect to target.
    * @author: Justhine Nisperos
    * @parameter:
    * @return: void
    ************************************************/
    protected override void ApplyEffect()
    {
        target.speedMultiplier *= slowMultiplier;
    }

    /***********************************************
    * RemoveEffect: Remove status effect from target and destroy this object.
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
