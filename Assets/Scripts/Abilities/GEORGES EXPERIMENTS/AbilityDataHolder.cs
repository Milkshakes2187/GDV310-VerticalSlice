using UnityEngine;

[System.Serializable]
public class AbilityDataHolder 
{
    //Abilityholder variables
    public AbilitySO abilitySO;
    public GameObject iconHolder;
    public KeyCode keybind;
    public float currentCooldown;
    public bool active;

    /***********************************************
  * AbilityHolder: Construtor
  * @author: George White
  * @parameter: Character , Character , Vector3 
  * @return: GameObject
  ************************************************/
    public AbilityDataHolder(AbilitySO _aso = null, GameObject _iconHolder = null, KeyCode _code = KeyCode.None, float _cd = 0.0f, bool _active = true)
    {
        abilitySO = _aso;
        iconHolder = _iconHolder;
        keybind = _code;
        currentCooldown = _cd;
        active = _active;
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
    * @parameter: float , float
    * @return: void
    ************************************************/
    public void TickCoolDown(float _time, float _cooldownMax)
    {
        //If the item is on cooldown, ticks the ability data holder cooldown
        currentCooldown = currentCooldown - _time;

        //stabilises at 0.0f
        if (currentCooldown < 0.0f)
        {
            currentCooldown = 0.0f;
        }
        if(iconHolder)
        {
            //iconHolder.GetComponent<PlayerAbilityUI>().SetCooldownFill(Mathf.Abs(currentCooldown / _cooldownMax));
        }
    }

    /***********************************************
    * ToggleAbilityIconLock: changes the appearence of an ability ui icon depending on its state of activeness
    * @author: George White
    * @parameter: bool
    * @return: void
    ************************************************/
    public void ToggleAbilityIconLock(bool _shouldLock)
    {
        if (!iconHolder) { return; }
        if(_shouldLock)
        {
            iconHolder.GetComponent<PlayerAbilityUI>().SetlockedFill(1);
        }
        else
        {
            iconHolder.GetComponent<PlayerAbilityUI>().SetlockedFill(0);
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
        currentCooldown = abilitySO.cooldown;
    }
    
}
