//using MPUIKIT;
//using TMPro;
//using UnityEngine;

//public class UIElements : MonoBehaviour
//{
//    public MPImage playerHealth;
//    public MPImage bossHealth;
//    public MPImage dashCD;
//    public MPImage ability1CD;

//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {
        
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (GameManager.instance.bInMainMenu) return;

//        playerHealth.fillAmount = Player.instance.fHealth / Player.instance.fMaxHealth;
//        bossHealth.fillAmount = PrototypeBoss.instance.fHealth / PrototypeBoss.instance.fMaxHealth;

//        if (Player.instance.fDashCDTimer == 0)
//        {
//            dashCD.fillAmount = 0;
//        }
//        else
//        {
//            dashCD.fillAmount = 1 - (Player.instance.fDashCDTimer / Player.instance.fDashCooldown);
//        }

//        //Ability 1
//        if (PlayerCombat.instance.fAbility1CooldownCurrent == 0)
//        {
//            ability1CD.fillAmount = 0;
//        }
//        else
//        {
//            ability1CD.fillAmount = 1 - (PlayerCombat.instance.fAbility1CooldownCurrent / PlayerCombat.instance.fAbility1CooldownMax);
//        }
//    }  
//}
