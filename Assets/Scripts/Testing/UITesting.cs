using TMPro;
using UnityEngine;

public class UITesting : MonoBehaviour
{
    public UIFillController CastBar;
    public TMP_Text castName;
    AbyssalWeaver weaver;

    private void Start()
    {
        weaver = FindAnyObjectByType<AbyssalWeaver>();
    }

    private void Update()
    {
        if (weaver.currentAbility)
        {
            castName.text = weaver.currentAbility.GetComponent<Ability>().abilityData.abilityName;
            CastBar.fillAmount = 1 - (weaver.currentAbility.GetComponent<Ability>().currentCastTime / weaver.currentAbility.GetComponent<Ability>().abilityData.timeToCast);
        }
    }
}
