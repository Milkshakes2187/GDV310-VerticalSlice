using UnityEngine;
using VInspector;

public class UIPlayerCastBarController : MonoBehaviour
{

    // Locally assigned Variables
    private PlayerSpellSystem spellSystem;
    private Ability castingAbility;

    [Header("Preassigned Variables")]
    public UIFillController castBarFill;

    void Start()
    {
        if (!Player.instance) { Debug.LogWarning("Player does not exist, UI Ability Frame will not work"); return; }

        spellSystem = Player.instance.GetComponentInChildren<PlayerSpellSystem>();
        if(!spellSystem)
        {
            Debug.LogError("Player spell system does not exist, disabling castbar");
            gameObject.SetActive(false);
            return;
        }

        spellSystem.OnPlayerCastAbility += OnPlayerStartCasting;
    }

    public void OnPlayerStartCasting(Ability ability)
    {
        Debug.Log(ability.name);
        castingAbility = ability;
    }

    // Update is called once per frame
    void Update()
    {
        if (castingAbility != null)
        {
            float castFillAmount = Mathf.Abs(1 - castingAbility.currentCastTime / castingAbility.abilityData.timeToCast);
            castBarFill.fillAmount = castFillAmount;
        }
    }
}
