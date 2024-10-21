using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using VInspector;

public class UIPlayerCastBarController : MonoBehaviour
{

    // Locally assigned Variables
    private PlayerSpellSystem spellSystem;
    private Ability castingAbility;

    [Header("Preassigned Variables")]
    public UIFillController castBarFill;

    public GameObject castBarFrameHolder;
    public GameObject castBarTextHolder;

    public TMPro.TextMeshProUGUI castTimeRemainingText;
    public TMPro.TextMeshProUGUI castNameText;

    void Start()
    {
        DisableCastBar();

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
    /***********************************************
    * OnPlayerStartCasting: Attached to event OnPlayerStartCastAbility from PlayerSpellSystem.cs, will activate whenever the player starts casting
    * @author: Nathan Hunt
    * @parameter: Ability
    * @return: void
    ************************************************/
    public void OnPlayerStartCasting(Ability ability)
    {
        DisableCastBar();
        EnableCastBar(ability);

        StartCoroutine(TickCastBar());
    }

    /***********************************************
    * OnPlayerStartCasting: Manages cast bar UI settings while player is casting
    * @author: Nathan Hunt
    * @return: IEnumerator
    ************************************************/
    public IEnumerator TickCastBar()
    {
        while (castingAbility != null)
        {
            float castFillAmount = Mathf.Abs(1 - castingAbility.currentCastTime / castingAbility.abilityData.timeToCast);
            castBarFill.fillAmount = castFillAmount;

            castTimeRemainingText.text = castingAbility.currentCastTime.ToString("f1");

            yield return null;
        }
    }

    /***********************************************
    * OnCancelCast: Disables cast bar when cast is interrupted, attached to event OnCancelCast in Ability.cs, to be animated in future
    * @author: Nathan Hunt
    * @return: void
    ************************************************/
    public void OnCancelCast()
    {
        DisableCastBar();
    }

    /***********************************************
    * OnSuccessfulCast: Disables cast bar when cast is successful, attached to event OnSuccessfulCast in Ability.cs, to be animated in future
    * @author: Nathan Hunt
    * @return: void
    ************************************************/
    public void OnSuccessfulCast()
    {
        DisableCastBar();
    }

    /***********************************************
    * EnableCastBar: Setups cast bar with values from new ability and attaches events to be invoked with Ability.cs
    * @author: Nathan Hunt
    * @parameter: Ability
    * @return: void
    ************************************************/
    private void EnableCastBar(Ability ability)
    {
        if (ability.abilityData.timeToCast == 0) return;

        castingAbility = ability;

        castBarFrameHolder?.SetActive(true);
        castBarTextHolder?.SetActive(true);

        castNameText.text = ability.name;

        castingAbility.OnCancelCast += OnCancelCast;
        castingAbility.OnSuccessfulCast += OnSuccessfulCast;
    }

    /***********************************************
    * DisableCastBar: Cleans up cast bar event handlers and disables the cast bar UI
    * @author: Nathan Hunt
    * @return: void
    ************************************************/
    private void DisableCastBar()
    {
        StopAllCoroutines();

        castBarFrameHolder?.SetActive(false);
        castBarTextHolder?.SetActive(false);

        try
        {
            castingAbility.OnCancelCast -= OnCancelCast;
            castingAbility.OnSuccessfulCast -= OnSuccessfulCast;
        }
        catch { }

        castingAbility = null;
    }
}
