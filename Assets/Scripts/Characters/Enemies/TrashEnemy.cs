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
        float distFromPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (isAggroed)      // ENEMY ALREADY AGGRO-ED
        {
            // Check if enemy is within attack range
            //if (nmAgent.remainingDistance <= attackRange)
            if (distFromPlayer <= attackRange)
            {
                // Stop moving and start attacking
                nmAgent.speed = 0.0f;
                Debug.Log(gameObject.name + " is Attacking the Player.");
            }
            else
            {
                // Continue chasing the player
                nmAgent.speed = moveSpeed;
                nmAgent.SetDestination(player.transform.position);
            }

            // Check if player is past de-aggro range
            if (distFromPlayer > deAggroRange)
            {
                // Set enemy to stop chasing/attacking
                nmAgent.speed = moveSpeed;
                Debug.Log(gameObject.name + " deaggroed");
            }
        }
        else                // ENEMY NOT AGGRO-ED
        {
            // Check if player is within aggro range
            if (distFromPlayer <= aggroRange)
            {
                // Set enemy to chase the player
                nmAgent.speed = moveSpeed;
                nmAgent.SetDestination(player.transform.position);
                isAggroed = true;
            }
        }
    }

}
