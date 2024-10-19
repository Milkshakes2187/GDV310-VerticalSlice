using UnityEngine;

public class Enemy : Character
{
    protected Player player;
    public float aggroRange;

    protected float basicAttackCD = 1f; //cooldown
    protected float basicAttackElapsed = 0f; // elapsedTime of basicAttack Cooldown

    protected override void Start()
    {
        base.Start();

        // find first object with player script and set it to the player var
        player = FindFirstObjectByType<Player>();

        // Add this enemy to world manager's list of all enemies
        WorldManager.instance.UpdateEnemyList(this, true);
    }

    protected override void TriggerDeath()
    {
        base.TriggerDeath();

        // Call world manager to update enemy list and destroy this enemy.
        WorldManager.instance.UpdateEnemyList(this, false);
        Destroy(gameObject);
    }
}
