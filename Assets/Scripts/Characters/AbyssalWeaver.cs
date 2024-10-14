using UnityEngine;
using UnityEngine.AI;

public class AbyssalWeaver : Enemy
{
    float timeBetweenCasts = 5f;
    float tbcElapsed = 5f; // timeBetweenCasts elapsed

    int mfaCountThisRotation = 0;

    bool mfaCast = false;

    public enum States
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
    public AbilitySO abyssalKnives;
    public AbilitySO markedForAssassination;
    public GameObject threadManagerPF;

    ThreadManager threadManager;

    [HideInInspector] public States currentState = States.IDLE;
    [HideInInspector] public States nextState = States.INTERWOVENTHREADS;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

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

                //var ak = abyssalKnives.InitialiseAbility(this, player, this.transform.position);
                //ak.GetComponent<Ability>().CastSpell();

                break;

            // Boss has no movement and stunned timer ticks down
            case States.STUNNED:

                // Set state starting variables
                agent.isStopped = true;

                break;

            // the state in which the boss will chase the player and auto attack
            case States.AGGRESSIVE:

                agent.isStopped = false;
                agent.destination = player.transform.position;

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
                if (tbcElapsed <= 0)
                {
                    // set the agents destination to its current position to stop it from finishing its current destination command
                    agent.destination = transform.position;

                    // reset elapsed Timers
                    tbcElapsed = timeBetweenCasts;

                    // move to next state
                    currentState = nextState;
                }

                break;

            // Happens when threads are first created and after each intermission
            case States.INTERWOVENTHREADS:
                threadManager.InterwovenThreads();

                // at the start of each rotation after interwoven threads reset MFA count to 0;
                mfaCountThisRotation = 0;

                // Set state transitions
                currentState = States.AGGRESSIVE;
                nextState = States.MFA;

                break;

            // Marks the player for assassination, placing a marker above their head
            // This is a channeled ability so the boss shouldn't move until the cast goes off
            // Once the ability finishes a phantom assassin should be spawned where the player was when the ability finished
            case States.MFA:

                if (!mfaCast)
                {
                    mfaCast = true;
                    var mfa = markedForAssassination.InitialiseAbility(this, player, player.transform.position);
                    mfa.GetComponent<Ability>().InitialSetup();
                    mfa.GetComponent<Ability>().CastSpell();
                }

                //currentState = States.AGGRESSIVE;

                //mfaCountThisRotation++;
                //
                //switch (mfaCountThisRotation)
                //{
                //    case 0:
                //        nextState = States.ENTWINEDABYSS;
                //        break;
                //
                //    case 1:
                //        nextState = States.THREADEDSLIP;
                //        break;
                //
                //    case 2:
                //        nextState = States.INTERMISSION;
                //        break;
                //}
                //
                //mfaCast = false;
                
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
