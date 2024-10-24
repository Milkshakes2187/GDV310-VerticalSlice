using UnityEngine;

public class UIClassPowerController_AbyssalKnight : MonoBehaviour
{
    private PlayerSpellSystem spellSystem;

    [Header("Preassigned Variables")]
    public UIFillController classPowerFill;
    public TMPro.TextMeshProUGUI classPowerText;

    void Start()
    {
        spellSystem = Player.instance.GetComponentInChildren<PlayerSpellSystem>();
        if (!spellSystem)
        {
            Debug.LogError("Player spell system does not exist, Class power will not update");
            enabled = false;
            return;
        }
    }


    void Update()
    {
        classPowerFill.fillAmount = spellSystem.classPowerCurrent / spellSystem.classPowerMax;
        classPowerText.text = Mathf.Floor(spellSystem.classPowerCurrent).ToString("f0");
    }
}
