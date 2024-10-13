using MPUIKIT;
using UnityEngine;


public enum E_CastStyle
{
    INSTANT,
    CAST_TIME,
    CHANNEL,
    TOGGLE,
}

public class Ability : MonoBehaviour
{

    bool bCastWhileMoving = false;

    public float castTime = 0.0f;

    public float cooldown = 0.0f;
    float currentCooldown = 0.0f;

    E_CastStyle castStyle = E_CastStyle.CAST_TIME;


    public Character owner;
    public Character target;
    public Vector3 targetLocation;




    public void RequestCastSpell()
    {
        


    }


    public virtual void CastSpell()
    {

    }



    public bool CheckSpellFinishedCasting(float _currentCastTime)
    {
        if(_currentCastTime >= castTime)
        {
            return true;
        }

        return false;
    }





    

    public void SetCooldown(float _cooldown)
    {
        currentCooldown = _cooldown;
    }


    public void TickCooldown(float _cooldownTicked)
    {
        //if ability is on cooldown, reduce by an amount
        if(currentCooldown > 0.0f)
        {
            currentCooldown -= _cooldownTicked;

            //if cooldown is less than 0, set it to zero
            if(currentCooldown < 0.0f)
            {
                currentCooldown = 0.0f;
            }
        }


        //update ui
    }


}
