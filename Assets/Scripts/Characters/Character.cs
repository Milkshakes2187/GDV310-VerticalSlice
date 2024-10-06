using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Base Stats")]
    public float maxHealth;
    public float maxMoveSpeed;
    public float baseDamage;

    [Header("Current Stats")]
    public float health;
    public float moveSpeed;

    [Header("Multipliers")]
    public float speedMultiplier;
    public float damageMultiplier;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(float _fDamage)
    {
        health -= _fDamage;

        if (health <= 0)
        {
            TriggerDeath();
        }
    }

    protected void Heal(float _fHealAmount)
    {
        health = health + _fHealAmount > maxHealth ? maxHealth : health + _fHealAmount;
    }

    protected void TriggerDeath()
    {
        Debug.Log(gameObject.name + " HAS DIED.");
    }
}
