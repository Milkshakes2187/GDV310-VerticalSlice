using UnityEngine;

public class StatusEffect : MonoBehaviour
{
    public float duration;
    float elapsedTime = 0.0f;

    Character target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = transform.root.GetComponent<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        Tick();
        ApplyEffect();
    }

    /***********************************************
    * Tick: Ticks the duration timer every frame.
    * @author: Justhine Nisperos
    * @parameter:
    * @return: void
    ************************************************/
    protected virtual void Tick()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= duration)
        {
            // Destroy this effect
            // Reset any effects on target here.

            Destroy(gameObject);
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
        // To be overwritten by the actual status effects
    }
}
