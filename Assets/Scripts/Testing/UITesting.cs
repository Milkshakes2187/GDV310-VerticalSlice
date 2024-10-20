using TMPro;
using UnityEngine;

public class UITesting : MonoBehaviour
{
    public GameObject castBar;
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
            castBar.SetActive(true);
            castName.text = weaver.currentAbility.GetComponent<Ability>().abilityData.abilityName;
            castBar.GetComponent<UIFillController>().fillAmount = 1 - (weaver.currentAbility.GetComponent<Ability>().currentCastTime / weaver.currentAbility.GetComponent<Ability>().abilityData.timeToCast);
        }
        else
        {
            castBar.SetActive(false);
        }
    }
}
