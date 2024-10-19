using System.Collections;
using TMPro;
using UnityEngine;

public class ThreadedSlip : Ability
{
    public float dashTime = 0.5f;
    public bool isDashing = false;

    private void Update()
    {
        if (!isDashing)
        {
            // update the target location every frame
            targetLocation = target.transform.position;
        }
    }

    /***********************************************
    * UseSpellEffect: Overriden spell effect, Starts dash coroutine
    * @author: Juan Le Roux
    * @parameter:
    * @return: void
    ************************************************/
    public override void UseSpellEffect()
    {
        StartCoroutine(Dash());
    }

    /***********************************************
    * UseSpellEffect: Overriden inituial setup. 
    * @author: Juan Le Roux
    * @parameter:
    * @return: void
    ************************************************/
    public override void InitialSetup()
    {
        
    }

    /***********************************************
    * Dash: Moves the owning character forwards towards a target location
    * @author: Juan Le Roux
    * @parameter:
    * @return: IEnumerator
    ************************************************/
    IEnumerator Dash()
    {
        isDashing = true;
        Vector3 startPosition = owner.transform.position;
        float elapsedTime = 0f;

        // lerp the character to their dash destination over the course of dashTime
        while (elapsedTime < dashTime)
        {
            owner.transform.position = Vector3.Lerp(startPosition, targetLocation, elapsedTime / dashTime);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Snap to the target position at the end its dash
        owner.transform.position = targetLocation;  
        isDashing = false;

        Destroy(gameObject);
    }

}
