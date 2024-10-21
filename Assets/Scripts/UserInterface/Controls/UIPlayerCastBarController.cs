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

    public void OnPlayerStartCasting(Ability ability)
    {
        DisableCastBar();
        EnableCastBar(ability);

        StartCoroutine(TickCastBar());
    }
    public IEnumerator TickCastBar()
    {
        while (true)
        {
            Debug.Log("ticking cast bar");
            float castFillAmount = Mathf.Abs(1 - castingAbility.currentCastTime / castingAbility.abilityData.timeToCast);
            castBarFill.fillAmount = castFillAmount;

            castTimeRemainingText.text = castingAbility.currentCastTime.ToString("f1");

            yield return null;
        }
    }

    public void OnCancelCast()
    {
        DisableCastBar();
        Debug.Log("canceled cast");
    }

    public void OnSuccessfulCast()
    {
        DisableCastBar();
        Debug.Log("successful cast");
    }

    private void EnableCastBar(Ability ability)
    {
        castingAbility = ability;

        castBarFrameHolder?.SetActive(true);
        castBarTextHolder?.SetActive(true);

        float castFillAmount = Mathf.Abs(1 - castingAbility.currentCastTime / castingAbility.abilityData.timeToCast);
        float _castTimeRemaining = castingAbility.abilityData.timeToCast - castingAbility.currentCastTime;

        castTimeRemainingText.text = _castTimeRemaining.ToString("f1");

        castNameText.text = ability.name;

        castingAbility.OnCancelCast += OnCancelCast;
        castingAbility.OnSuccessfulCast += OnSuccessfulCast;
    }

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
