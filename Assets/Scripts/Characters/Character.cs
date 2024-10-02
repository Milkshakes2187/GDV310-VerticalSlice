using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Base Stats")]
    public float fMaxHealth;
    public float fMaxMoveSpeed;
    public float fBaseDamage;

    [Header("Current Stats")]
    public float fHealth;
    public float fMoveSpeed;

    [Header("Multipliers")]
    public float fSpeedMultiplier;
    public float fDamageMultiplier;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        fHealth = fMaxHealth;
    }

    public void TakeDamage(float _fDamage)
    {
        fHealth -= _fDamage;

        if (fHealth <= 0)
        {
            Debug.Log("YOU HAVE DIED.");
        }
    }

    protected void Heal(float _fHealAmount)
    {
        fHealth = fHealth + _fHealAmount > fMaxHealth ? fMaxHealth : fHealth + _fHealAmount;
    }
}
