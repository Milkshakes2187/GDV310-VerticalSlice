using UnityEngine;

public class StatusEffect : MonoBehaviour
{
    public float duration;
    float elapsedTime = 0.0f;

    protected Character target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        if (transform.root.GetComponent<Character>())
        {
            target = transform.root.GetComponent<Character>();
        }
        else
        {
            Debug.LogWarning("No Character parent.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        TickDuration();
    }

    /***********************************************
    * Tick: Ticks the duration timer every frame.
    * @author: Justhine Nisperos
    * @parameter:
    * @return: void
    ************************************************/
    protected virtual void TickDuration()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= duration)
        {
            RemoveEffect();
        }
    }

    /***********************************************
    * ApplyEffect: Applies the status effect to target.
    * @author: Justhine Nisperos
    * @parameter:
    * @return: void
    ************************************************/
    protected virtual void ApplyEffect()
    {
        // To be overwritten by the specific status effects
    }

    /***********************************************
    * RemoveEffect: Remove status effect from target and destroy this object.
    * @author: Justhine Nisperos
    * @parameter:
    * @return: void
    ************************************************/
    protected virtual void RemoveEffect()
    {
        // Destroy this effect
        // Reset any effects on target here.

        Destroy(gameObject);
    }
}
