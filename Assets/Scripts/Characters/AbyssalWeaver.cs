using UnityEngine;
using UnityEngine.AI;

public class AbyssalWeaver : Enemy
{
    float timeBetweenCasts = 5f;
    float tbcElapsed = 0f; // timeBetweenCasts elapsed

    float abilityCastCD = 2f;
    float abilityCastElapsed = 0f;

    enum States
    {
        IDLE,
        STUNNED,
        AGGRESSIVE,     
        INTERWOVENTHREADS,
        MFA,            // Marked for Assassination
        ENTWINEDABYSS, 
        THREADEDSLIP, 
        INTERMISSION,   // Woven Reality
    }

    NavMeshAgent agent;
    public GameObject abyssalKnivesPF;
    public GameObject markedForAssassinationPF;
    public GameObject threadManagerPF;

    ThreadManager threadManager;

    States currentState = States.IDLE;
    States nextState = States.INTERWOVENTHREADS;

    private void Awake()
    {
        var threadArena = Instantiate(threadManagerPF, transform.position, Quaternion.identity);
        threadManager = threadArena.GetComponent<ThreadManager>();
    }

    // Update is called once per frame
    void Update()
    {
        float distToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // Boss rotation
        // 1. Interwoven Threads
        // 2. Marked for Assassination
        // 3. Entwined Abyss
        // 4. Marked for Assassination
        // 5. Threaded Slip
        // 6. Marked for Assassination
        // 7. Intermission
        // 
        // With a chance between each ability to cast abyssal Daggers
        switch (currentState)
        {
            // The boss has not been aggro'd and is idle in the center of his arena
            case States.IDLE:

                // Set state starting variables
                agent.isStopped = true;

                // if player is in aggro range begin the fight
                if (distToPlayer <= 10)
                {
                    currentState = nextState;
                }
                break;

            // Boss has no movement and stunned timer ticks down
            case States.STUNNED:

                // Set state starting variables
                agent.isStopped = true;

                break;

            // the state in which the boss will chase the player and auto attack
            case States.AGGRESSIVE:

                agent.isStopped = false;

                // tick down basic attack cooldown and time between next attack timers
                basicAttackElapsed -= Time.deltaTime;
                tbcElapsed -= Time.deltaTime;

                if (basicAttackElapsed <= 0)
                {
                    if (distToPlayer <= agent.stoppingDistance)
                    {
                        // activate basic attack animation

                        // deal damage and reset cd timer
                        player.TakeDamage(5);
                        basicAttackElapsed = basicAttackCD;
                    }
                }

                // when the time between casts cooldown finishes move to the state transition state
                if (tbcElapsed < 0)
                {
                    // reset elapsed Timers
                    tbcElapsed = 0;
                    abilityCastElapsed = 0;

                    // move to next state
                    currentState = nextState;
                }

                break;

            // Happens when threads are first created and after each intermission
            case States.INTERWOVENTHREADS:

                threadManager.InterwovenThreads();

                // move between 
                currentState = States.AGGRESSIVE;
                nextState = States.MFA;

                break;

            // Marks the player for assassination, placing a marker above their head
            // This is a channeled ability so the boss shouldn't move until the cast goes off
            // Once the ability finishes a phantom assassin should be spawned where the player was when the ability finished
            case States.MFA:

                Instantiate(markedForAssassinationPF, transform.position, Quaternion.identity);

                break;

            // Beam which blasts out in front of the boss, boss will slowly rotate towards the player
            // Beam also triggers phantom assassins to become unstable, causing them to pulse with damage and then destroy themselves
            case States.ENTWINEDABYSS:
                break;

            // Dashes towards the player quickly, if the boss hits a thread they are stunned
            case States.THREADEDSLIP:
                break;

            // During intermission the boss will move to the center and will bind threads to pillars
            // Boss does not move during this state
            case States.INTERMISSION:
                break;
        }
    }
}
