using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

public class UIAbilityHolderController : MonoBehaviour
{
    public enum AbilitySlot
    {
        Slot1,
        Slot2,
        Slot3,
        Slot4,
        Slot5,
        Slot6,
    }
    public AbilitySlot slot;

    // Locally assigned Variables
    private PlayerSpellSystem spellSystem;
    private AbilityDataHolder abilityData;

    [Header("Preassigned Variables")]
    public UIFillController cooldownFill;
    public UIFillController lockFill;
    public Image abilityImage;


    void Start()
    {
        if (!Player.instance) { Debug.LogWarning("Player does not exist, UI Ability Frame will not work"); return; }

        spellSystem = Player.instance.GetComponentInChildren<PlayerSpellSystem>();
    }

    public void Update()
    {
        AssignAbility();
        TickAbilityFrame();
    }


    /***********************************************
    * AssignAbility: Assigns the current ability data from the player spell systems slot
    * @author: Nathan Hunt
    * @return: void
    ************************************************/
    public void AssignAbility()
    {
        if (spellSystem == null) { Debug.LogWarning("Player spell system not found, UI Ability Frame will not update"); return; }

        abilityData = spellSystem.GetAbilityFromSlot((int)slot);

        if(abilityData == null)
        {
            Debug.LogWarning("Ability data could not be located for Slot: " + slot.ToString());
            ResetSlotUI();
        }
    }

    /***********************************************
    * ResetSlotUI: Resets the current spell slot UI to be default empty
    * @author: Nathan Hunt
    * @return: void
    ************************************************/
    public void ResetSlotUI()
    {
        cooldownFill.fillAmount = 0;
        lockFill.fillAmount = 1;
        abilityImage.sprite = null;
        abilityImage.enabled = false;
    }


    /***********************************************
    * TickAbilityFrame: Updates the spell slot UI every frame with current values from ability data. Eventually change to be event based when optimizing
    * @author: Nathan Hunt
    * @return: void
    ************************************************/
    public void TickAbilityFrame()
    {
        if (abilityData == null) { Debug.LogWarning("Ability data not found, UI Ability Frame will not update"); ResetSlotUI(); return; }

        // Cooldown
        cooldownFill.fillAmount = abilityData.currentCooldown/abilityData.abilitySO.cooldown;

        // Ability Image
        abilityImage.sprite = abilityData.abilitySO.image;
        if(!abilityImage.enabled) abilityImage.enabled = true;

        // Locked

        // Need coders to add a function to check if ability is able to be pressed
        // Might already exist but something like abilityData.CheckAbilityCastEligibility()
        // Ability should be "locked" if they dont have enough class power

        lockFill.fillAmount = 0;
    }
}
