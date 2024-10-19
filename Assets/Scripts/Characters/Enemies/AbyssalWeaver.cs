using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AbyssalWeaver : Enemy
{
    public enum STATES
    {
        IDLE,
        STUNNED,
        AGGRESSIVE,
        INTERWOVEN_THREADS,
        MFA,                // Marked for Assassination
        ENTWINED_ABYSS,
        THREADED_SLIP,
        INTERMISSION,       // Woven Reality
    }

    NavMeshAgent agent;

    GameObject currentAbility;
    public AbilitySO abyssalKnives;
    public AbilitySO markedForAssassination;
    public AbilitySO EntwinedAbyss;
    public AbilitySO ThreadedSlip;
    public GameObject threadManagerPF;
    ThreadManager threadManager;

    public float turnSpeed = 5f;
    float timeBetweenCasts = 5f;
    float tbcElapsed = 5f; // timeBetweenCasts elapsed
    int mfaCountThisRotation = 0;

    bool currentAbilityCreated = false;
    [HideInInspector] public List<PhantomAssassin> phantomAssassinList = new List<PhantomAssassin>();

    STATES currentState = STATES.IDLE;
    STATES nextState = STATES.INTERWOVEN_THREADS;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        var threadArena = Instantiate(threadManagerPF, transform.position, Quaternion.identity);
        threadManager = threadArena.GetComponent<ThreadManager>();
    }

    // Update is called once per frame
    void Update()
    {
        RunStateMachine();
    }

    /***********************************************
    * RunStateMachine: Handles the different states that the boss can transition into with a switch case
    * @author: Juan Le Roux
    * @parameter:
    * @return: void
    ************************************************/
    void RunStateMachine()
    {
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

        float distToPlayer = Vector3.Distance(transform.position, player.transform.position);

        switch (currentState)
        {
            // Boss has not been aggro'd
            case STATES.IDLE:

                IdleState(distToPlayer);
                break;

            // Boss has no movement and stunned timer ticks down
            case STATES.STUNNED:

                // Set state starting variables
                agent.isStopped = true;

                break;
            
            // During this state the boss chases and auto attacks the player
            case STATES.AGGRESSIVE:

                AggressiveState(distToPlayer);
                break;

            // Happens when threads are first created and after each intermission
            case STATES.INTERWOVEN_THREADS:

                InterwovenThreadState();
                break;

            // Marks the player for assassination, placing a marker above their head
            // This is a channeled ability so the boss shouldn't move until the cast goes off
            // Once the ability finishes a phantom assassin should be spawned where the player was when the ability finished
            case STATES.MFA:

                MFAState();
                break;

            // Beam which blasts out in front of the boss, boss will slowly rotate towards the player
            // Beam also triggers phantom assassins to become unstable, causing them to pulse with damage and then destroy themselves
            case STATES.ENTWINED_ABYSS:

                EntwinedAbyssState();
                break;

            // Dashes towards the player quickly, if the boss hits a thread they are stunned
            case STATES.THREADED_SLIP:

                ThreadedSlipState();
                break;

            // During intermission the boss will move to the center and will bind threads to pillars
            // Boss does not move during this state
            case STATES.INTERMISSION:
                break;
        }
    }

    /***********************************************
    * IdleState: During this state the boss has not been aggro'd and is idle in the center of his arena
    * @author: Juan Le Roux
    * @parameter: float
    * @return: void
    ************************************************/
    void IdleState(float _distToPlayer)
    {
        // Set state starting variables
        agent.isStopped = true;

        // if player is in aggro range begin the fight
        if (_distToPlayer <= 10)
        {
            currentState = nextState;
        }
    }

    /***********************************************
    * AggressiveState: During this state the boss will chase the player and auto attack
    * @author: Juan Le Roux
    * @parameter: float
    * @return: void
    ************************************************/
    void AggressiveState(float _distToPlayer)
    {
        agent.isStopped = false;
        agent.destination = player.transform.position;

        // tick down basic attack cooldown and time between next attack timers
        basicAttackElapsed -= Time.deltaTime;
        tbcElapsed -= Time.deltaTime;

        if (basicAttackElapsed <= 0)
        {
            if (_distToPlayer <= agent.stoppingDistance)
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
    }

    /***********************************************
    * InterwovenThreadState: Creates the interwoven threads and resets the mfa count
    * @author: Juan Le Roux
    * @parameter:
    * @return: void
    ************************************************/
    void InterwovenThreadState()
    {
        threadManager.InterwovenThreads();

        // at the start of each rotation after interwoven threads reset MFA count to 0;
        mfaCountThisRotation = 0;

        // Set state transitions
        currentState = STATES.AGGRESSIVE;
        nextState = STATES.MFA;
    }

    /***********************************************
    * MFAState: Handles the creation of the MFA ability and switches states based on how many times this ability has been case this rotation
    * @author: Juan Le Roux
    * @parameter:
    * @return: void
    ************************************************/
    void MFAState()
    {
        if (!currentAbilityCreated)
        {
            currentAbilityCreated = true;
            currentAbility = markedForAssassination.InitialiseAbility(this, player, player.transform.position);
            currentAbility.GetComponent<Ability>().InitialSetup();
            currentAbility.GetComponent<Ability>().CastSpell();
        }

        if (!currentAbility && currentAbilityCreated)
        {
            currentAbilityCreated = false;

            currentState = STATES.AGGRESSIVE;

            switch (mfaCountThisRotation)
            {
                case 0:
                    nextState = STATES.ENTWINED_ABYSS;
                    break;

                case 1:
                    nextState = STATES.THREADED_SLIP;
                    break;

                case 2:
                    nextState = STATES.INTERWOVEN_THREADS;
                    break;
            }

            mfaCountThisRotation++;
        }
    }

    /***********************************************
    * EntwinedAbyssState: Handles the creation of Entwined abyss ability and turning the boss to face the player during the ability
    * @author: Juan Le Roux
    * @parameter:
    * @return: void
    ************************************************/
    void EntwinedAbyssState()
    {
        // if the ability hasn't been created, create it first
        if (!currentAbilityCreated)
        {
            currentAbilityCreated = true;
            currentAbility = EntwinedAbyss.InitialiseAbility(this, player, transform.position);
            currentAbility.GetComponent<Ability>().InitialSetup();
            currentAbility.GetComponent<Ability>().CastSpell();
        }

        // if the ability has been created once during this state and it has not been destroyed yet
        if (currentAbility && currentAbilityCreated)
        {
            // rotate the boss slowly towards the player
            TurnTowardsPlayer();
        }

        // State transitions when the ability has been created once during this state
        // And the ability does not exist anymore
        if (!currentAbility && currentAbilityCreated)
        {
            currentAbilityCreated = false;
            nextState = STATES.MFA;
            currentState = STATES.AGGRESSIVE;
        }
    }

    /***********************************************
    * ThreadedSlipState: Handles the creation of Entwined abyss ability and turning the boss to face the player during the ability
    * @author: Juan Le Roux
    * @parameter:
    * @return: void
    ************************************************/
    void ThreadedSlipState()
    {
        // if the ability hasn't been created, create it first
        if (!currentAbilityCreated)
        {
            currentAbilityCreated = true;
            currentAbility = ThreadedSlip.InitialiseAbility(this, player, player.transform.position);
            currentAbility.GetComponent<Ability>().InitialSetup();
            currentAbility.GetComponent<Ability>().CastSpell();
        }

        // if the ability has been created once during this state and it has not been destroyed yet
        if (currentAbility && currentAbilityCreated)
        {
            // rotate the boss slowly towards the player
            TurnTowardsPlayer();
        }

        // State transitions when the ability has been created once during this state
        // And the ability does not exist anymore
        if (!currentAbility && currentAbilityCreated)
        {
            currentAbilityCreated = false;
            nextState = STATES.MFA;
            currentState = STATES.AGGRESSIVE;
        }
    }

    void CheckForTransition(STATES _nextState)
    {
        // State transitions when the ability has been created once during this state
        // And the ability does not exist anymore
        if (!currentAbility && currentAbilityCreated)
        {
            currentAbilityCreated = false;
            nextState = STATES.INTERWOVEN_THREADS;
            currentState = STATES.AGGRESSIVE;
        }
    }

    /***********************************************
    * TurnTowardsPlayer: Turns the boss towards the player
    * @author: Juan Le Roux
    * @parameter:
    * @return: void
    ************************************************/
    void TurnTowardsPlayer()
    {
        // get the player direction
        Vector3 direction = Player.instance.transform.position - transform.position;
        direction.y = 0;

        // set the desired look rotation towards the players look direction
        Quaternion rotation = Quaternion.LookRotation(direction);

        // Slerp the rotation towards the player at a designated turn speed
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * turnSpeed);
    }
}
