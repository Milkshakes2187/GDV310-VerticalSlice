using UnityEngine;
using UnityEngine.AI;
using VInspector;

public class TrashEnemy : Enemy
{
    [Header("Vars")]
    public float attackRange = 2.0f;
    public float deAggroRange;
    bool isAggroed = false;

    [Foldout("References")]
    public NavMeshAgent nmAgent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        deAggroRange = aggroRange * 3.0f;
        
        nmAgent = GetComponent<NavMeshAgent>();
        nmAgent.speed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Player.instance)
        {
            Debug.LogWarning("Player does not exist in scene");
            return;
        }

        float distFromPlayer = Vector3.Distance(transform.position, player.transform.position);

        // Check if the enemy has already been aggroed by the player.
        if (isAggroed) // ---------- ENEMY ALREADY AGGRO-ED ----------
        {
            // Check if enemy is within attack range
            if (distFromPlayer <= attackRange)
            {
                // Stop moving and start attacking
                nmAgent.speed = 0.0f;
                Debug.Log(gameObject.name + " is Attacking the Player.");
            }

            // Check if player is past de-aggro range
            else if (distFromPlayer > deAggroRange)
            {
                // Set enemy to stop chasing/attacking
                nmAgent.ResetPath();
                isAggroed = false;
                Debug.Log(gameObject.name + " deaggroed");
            }

            else
            {
                // Continue chasing the player
                nmAgent.speed = moveSpeed;
                nmAgent.SetDestination(player.transform.position);
            }
        }
        else // -------------------- ENEMY NOT AGGRO-ED ----------
        {
            // Check if player is within aggro range
            if (distFromPlayer <= aggroRange)
            {
                // Set enemy to chase the player
                nmAgent.speed = moveSpeed;
                nmAgent.SetDestination(player.transform.position);
                isAggroed = true;
                Debug.Log(gameObject.name + " aggroed.");
            }
        }
    }
}
