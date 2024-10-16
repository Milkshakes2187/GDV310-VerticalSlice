using UnityEngine;
using UnityEngine.WSA;

[System.Serializable]
public class AbilityDataHolder : MonoBehaviour
{
    //Abilityholder variables
    public AbilitySO ability;
    public KeyCode keybind;
    public float currentCooldown;

    /***********************************************
  * AbilityHolder: Construtor
  * @author: George White
  * @parameter: Character , Character , Vector3 
  * @return: GameObject
  ************************************************/
    public AbilityDataHolder(AbilitySO _aso = null, KeyCode _code = KeyCode.None, float _cd = 0.0f)
    {
        ability = _aso;
        keybind = _code;
        currentCooldown = _cd;
    }

    /***********************************************
    * IsOnCooldown: Checks if the aility is on cooldown
    * @author: George White
    * @parameter: 
    * @return: bool
    ************************************************/
    public bool IsOnCooldown()
    {
        if (currentCooldown > 0.0f)
        {
            return true;
        }
        return false;
    }


    /***********************************************
    * TickCoolDown: Subtracts time from a cooldown, and stabilizes at 0.0f
    * @author: George White
    * @parameter: float 
    * @return: void
    ************************************************/
    public void TickCoolDown(float _time)
    {
        //If the item is on cooldown, ticks the ability data holder cooldown
        currentCooldown = currentCooldown - _time;

        //stabilises at 0.0f
        if (currentCooldown < 0.0f)
        {
            currentCooldown = 0.0f;
        }
    }

    /***********************************************
    * StartCooldown: Starts the cooldown according to the abilities cooldown value
    * @author: George White
    * @parameter:  
    * @return: void
    ************************************************/
    public void StartCooldown()
    {
        currentCooldown = ability.cooldown;
    }
    
}
