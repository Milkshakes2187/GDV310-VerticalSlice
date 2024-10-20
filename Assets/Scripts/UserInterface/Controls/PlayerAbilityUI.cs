using MPUIKIT;
using UnityEngine;

public class PlayerAbilityUI
{
    public GameObject cooldownFill;
    public GameObject inactiveFill;
    public GameObject lockedFill;
    public GameObject imageIconSlot;

    public void SetCooldownFill(float _fill)
    {
        cooldownFill.GetComponent<UIFillController>().fillAmount = _fill;
    }

    public void SetinactiveFill(float _fill)
    {
        inactiveFill.GetComponent<UIFillController>().fillAmount = _fill;
    }

    public void SetlockedFill(float _fill)
    {
        lockedFill.GetComponent<UIFillController>().fillAmount = _fill;
    }

    public void SetImageIcon(Sprite _sprite)
    {
        imageIconSlot.GetComponent<SpriteRenderer>().sprite = _sprite;
    }
}
