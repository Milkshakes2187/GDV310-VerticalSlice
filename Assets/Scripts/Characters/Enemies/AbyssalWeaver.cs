using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AbyssalWeaver : Enemy
{
    public enum STATES
    {
        IDLE,
        STUNNED,
        AGGRESSIVE,
        ABYSSAL_KNIVES,
        INTERWOVEN_THREADS,
        MFA,                // Marked for Assassination
        ENTWINED_ABYSS,
        THREADED_SLIP,
        INTERMISSION,       // Woven Reality
    }

    NavMeshAgent agent;

    [HideInInspector] public GameObject currentAbility;
    public AbilitySO abyssalKnives;
    public AbilitySO markedForAssassination;
    public AbilitySO EntwinedAbyss;
    public AbilitySO ThreadedSlip;
    public AbilitySO threadManagerAbility;
    ThreadManager threadManager;

    public float turnSpeed = 5f;

    float stunTime = 3f;
    float stunElapsed = 3f; 
    float timeBetweenCasts = 5f;
    float tbcElapsed = 5f; // timeBetweenCasts elapsed
    int mfaCountThisRotation = 0;

    bool currentAbilityCreated = false;
    [HideInInspector] public List<PhantomAssassin> phantomAssassinList = new List<PhantomAssassin>();

    // list of all active collisions
    [HideInInspector] public List<GameObject> collisions = new List<GameObject>();

    // Ability order list, abilities in this list will happen in the order placed, the count is used to determine what point in the list the boss is up to
    public List<STATES> abilityRotation = new List<STATES>();
    int rotationState = 0;

    [HideInInspector] public STATES currentState = STATES.IDLE;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        // create the thread manager
        var threadArena = threadManagerAbility.InitialiseAbility(this, null, transform.position);
        threadManager = threadArena.GetComponent<ThreadManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // update state machine every frame
        RunStateMachine();

        // Ensure the rotationState does not surpass the amount of abilities in the list
        if (rotationState >= abilityRotation.Count)
        {
            rotationState = 0;
        }
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
                StunnedState();

                break;
            
            // During this state the boss chases and auto attacks the player
            case STATES.AGGRESSIVE:

                AggressiveState(distToPlayer);
                break;

            case STATES.ABYSSAL_KNIVES:

                AbyssalKnivesState();
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
        if (_distToPlayer <= 20)
        {
            currentState = abilityRotation[rotationState];
            rotationState++;
        }
    }

    /***********************************************
    * IdleState: During this state the boss has not been aggro'd and is idle in the center of his arena
    * @author: Juan Le Roux
    * @parameter: float
    * @return: void
    ************************************************/
    void StunnedState()
    {
        // Set state starting variables
        agent.isStopped = true;

        stunElapsed -= Time.deltaTime;

        if (stunElapsed <= 0)
        {            
            stunElapsed = stunTime;
            currentState = STATES.AGGRESSIVE;
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
            currentState = abilityRotation[rotationState];
            rotationState++;
        }
    }

    /***********************************************
    * AbyssalKnivesState: State for when the boss uses the abyssal knives attack, this happens once between every attack
    * @author: Juan Le Roux
    * @parameter:
    * @return: void
    ************************************************/
    void AbyssalKnivesState()
    {
        if (!currentAbilityCreated)
        {
            currentAbilityCreated = true;
            currentAbility = abyssalKnives.InitialiseAbility(this, player, this.transform.position);
            currentAbility.GetComponent<Ability>().InitialSetup();
            currentAbility.GetComponent<Ability>().CastSpell();

            foreach (var assassin in phantomAssassinList)
            {
                assassin.GetComponent<Ability>().CastSpell();
            }
        }

        CheckForTransition();
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
        currentAbilityCreated = true;

        // at the start of each rotation after interwoven threads reset MFA count to 0;
        mfaCountThisRotation = 0;

        // Set state transitions
        CheckForTransition();
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

            // Increment the amount of times marked for assassination has been cast this rotation
            mfaCountThisRotation++;
        }

        CheckForTransition();
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

        CheckForTransition();
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

        CheckForTransition();
    }

    void CheckForTransition()
    {
        // State transitions when the ability has been created once during this state
        // And the ability does not exist anymore
        if (!currentAbility && currentAbilityCreated)
        {
            currentAbilityCreated = false;
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

    /***********************************************
    * OnTriggerEnter: Gets called whenever a collision occurs with another object
    * @author: Juan Le Roux
    * @parameter: Collision
    * @return: void
    ************************************************/
    private void OnTriggerEnter(Collider other)
    {
        // add the collision to the collisions list
        if (other.gameObject.GetComponentInParent<Thread>())
        {
            if (other.gameObject.GetComponentInParent<Thread>().isThreadActive)
            {
                collisions.Add(other.gameObject);
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        
    }

    /***********************************************
    * OnCollisionEnter: Gets called whenever the collider ends collision with another object
    * @author: Juan Le Roux
    * @parameter: Collision
    * @return: void
    ************************************************/
    private void OnCollisionExit(Collision collision)
    {
        // remove the collision from the collisions list
        collisions.Remove(collision.gameObject);
    }
}
