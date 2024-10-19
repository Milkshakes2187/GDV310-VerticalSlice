using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Base Stats")]
    public float maxHealth;
    public float maxMoveSpeed;
    public float baseDamage;
    public float maxArmour;

    [Header("Current Stats")]
    public float health;
    public float moveSpeed;
    public float armour;

    [Header("Multipliers")]
    public float speedMultiplier = 1.0f;
    public float damageMultiplier = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        health = maxHealth;
    }

    /***********************************************
    * TakeDamage: Reduces character's health based on damage.
    * @author: Justhine Nisperos
    * @parameter: float
    * @return: void
    ************************************************/
    public void TakeDamage(float _fDamage)
    {
        // Calculate damage reduced based on armour
        // 1 armour = 1% less damage
        float damageReduction = (100.0f - armour) * 0.1f;
        health -= _fDamage * damageReduction;

        if (health <= 0)
        {
            TriggerDeath();
        }
    }

    /***********************************************
    * Heal: Increases character's health based on heal amount.
    * @author: Justhine Nisperos
    * @parameter: float
    * @return: void
    ************************************************/
    protected void Heal(float _fHealAmount)
    {
        health = health + _fHealAmount > maxHealth ? maxHealth : health + _fHealAmount;
    }

    /***********************************************
    * TriggerDeath: Run end-of-life functions and destroy game object.
    * @author: Justhine Nisperos
    * @parameter: 
    * @return: void
    ************************************************/
    protected virtual void TriggerDeath()
    {
        Debug.Log(gameObject.name + " HAS DIED.");

        // Trash mob/enemy to check for worldevent
    }
}
