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
        if (Player.instance)
        {
            player = Player.instance;
        }
        else
        {
            Debug.LogWarning("NO PLAYER IN SCENE.");
        }

        // Add this enemy to world manager's list of all enemies
        if (WorldManager.instance)
        {
            WorldManager.instance.UpdateEnemyList(this, true);
        }
        else
        {
            Debug.LogWarning("No world manager in scene");
        }
    }

    /***********************************************
    * TriggerDeath: Inform world manager of enemy list change and destroy this enemy.
    * @author: Justhine Nisperos
    * @parameter: 
    * @return: void
    ************************************************/
    protected override void TriggerDeath()
    {
        base.TriggerDeath();

        // Call world manager to update enemy list and destroy this enemy.
        if (WorldManager.instance)
        {
            WorldManager.instance.UpdateEnemyList(this, false);
        }
        else
        {
            Debug.LogWarning("No world manager in scene");
        }

        Destroy(gameObject);
    }

    protected void OnDestroy()
    {
        // Ensures that enemy still runs death functions properly when suddenly destroyed.
        TriggerDeath();
    }
}
