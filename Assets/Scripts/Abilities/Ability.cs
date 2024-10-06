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

    float fCastTime = 0.0f;
    float fCooldown = 0.0f;

    E_CastStyle castStyle = E_CastStyle.CAST_TIME;


    //strength of spell?
    float fStrength = 0.0f;


    //ability icon
    [SerializeField] MPImage AbilityIcon = null;








    public void CastSpell()
    {

    }



    public bool CheckSpellFinishedCasting(float _currentCastTime)
    {
        if(_currentCastTime >= fCastTime)
        {
            return true;
        }

        return false;
    }
}
