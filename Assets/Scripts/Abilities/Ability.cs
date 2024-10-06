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

    float fStrength = 0.0f;

    // cooldown
    // damage
    // etc




}
