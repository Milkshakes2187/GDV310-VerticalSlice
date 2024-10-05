using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using VInspector;

public class PrototypeBoss : MonoBehaviour
{
    public static PrototypeBoss instance;

    [Foldout("Combos")]
    public List<BossCombo> bossCombos;
    public bool bIsAttacking = false;
    public bool bAnimTriggered = false;
    public bool bStopRotation = false;
    public Timer exhaustionTimer;

    [Foldout("FSM Values")]
    [HideInInspector] public PB_FSM currentState;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator anim;
    public GameObject player;
    public GameObject arenaCenter;
    public GameObject boulderPF;
    public GameObject lineAttackPF;
    public bool bFightStarted = false;
    
    [Foldout("Stats")]
    public float fMaxHealth = 100.0f;
    public float fHealth;
    public float fTurnSpeed = 5;

    [Foldout("Attacks")]
    [Header("Boulders")]
    public int iBoulderDamage = 100;
    public int iBoulderSpawnTime = 3;
    public int iBoulderMinDist = 10;
    public int iBoulderClusterDist = 15;
    public int iArenaRadius = 30;
    public int iBoulderCount = 5;

    [Header("Explosion AoE")]
    public int iExplosionDamage = 10;
    public int iExplosionCastTime = 5;
    public Timer AoETimer;
    public bool bExplosionDone = false;
    public float fOriginalLavaAmount = 0.2f;
    public float fLavaAmount = 0.2f;
    public float fLavaIncreaseToExplosion = 0;
    public bool bResetAfterExplode = false;
    public Material rockyLava;

    [Header("Cone")]
    public int iConeDamage = 10;
    public float fConeRange = 15f;
    public float fConeHalfFov = 30f;

    [Header("Line Attack")]
    public int iLineDamage = 5;
    public Timer randomCast;
    public float fLineCastTime = 1.5f;
    public float fLineArenaRadius = 45f;
    public float fLineIntervalMin = 10f;
    public float fLineIntervalMax = 16f;

    [Header("Dash Attack")]
    public float fdashDamage = 50;
    public float fDistToDash = 0;
    public float fDashRotateTime = 1.5f;
    public float fMinDashDist = 20;
    public float fDashSpeed = 50;
    public float fDashChance = 50;
    public float fDashCompletion = 0;
    public bool bDashActive = false;

    [Header("Spin Attack")]
    public int iSpinDamage = 1;
    public float fSpinRadius = 10;
    public bool bSpinning = false;
    public bool bMoveSpin = false;
    public Timer spinDuration;

    [Foldout("Indicators")]
    public GameObject lineIndicator;
    public GameObject circleIndicator;
    public GameObject coneIndicator;
    public GameObject roomWideIndicator;


    public List<GameObject> spawnedBoulders = new List<GameObject>();

    private void OnApplicationQuit()
    {
        rockyLava.SetFloat("_LavaAmount", fOriginalLavaAmount);
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = Player.instance.gameObject;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        currentState = new PB_Idle(this, agent, player, anim);

        fHealth = fMaxHealth;
        randomCast.timeRequired = Random.Range(fLineIntervalMin, fLineIntervalMax);
    }

    // Update is called once per frame
    void Update()
    {
        // if the fight has started tick the AOETimer and run LineAttacks
        if (bFightStarted)
        {
            if (!bResetAfterExplode)
            {
                fLavaIncreaseToExplosion = Mathf.Lerp(0, 0.4f, AoETimer.elapsedtime / AoETimer.timeRequired);
                fLavaAmount = fLavaIncreaseToExplosion + fOriginalLavaAmount;

                rockyLava.SetFloat("_LavaAmount", fLavaAmount);

                if (fLavaAmount >= 0.5f)
                {
                    bResetAfterExplode = true;
                }
            }
            
            LineAttack();
            AoETimer.elapsedtime += Time.deltaTime;

            // Check if boss is exhausted before continuing process
            if (exhaustionTimer.elapsedtime < exhaustionTimer.timeRequired)
            {
                agent.isStopped = true;
                agent.SetDestination(player.transform.position);
                exhaustionTimer.elapsedtime += Time.deltaTime;
                return;
            }
        }

        currentState = currentState.Process();

        if (bSpinning)
        {
            SpinAttack();

            spinDuration.elapsedtime += Time.deltaTime;
            if (spinDuration.elapsedtime > spinDuration.timeRequired)
            {
                anim.SetTrigger("_triggerSpinDone");

                agent.isStopped = true;
                bSpinning = false;
                spinDuration.elapsedtime = 0;
            }
        }

        DashAttack();
    }

    [Button]
    void Spawn()
    {
        if(!Application.isPlaying) { return; }
        SpawnBoulders(iBoulderCount);
    }

    public void SpawnBoulders(int _boulderCount)
    {
        int iAttempts = 0;
        int iMaxAttempts = 1000;

        for (int i = 0; i < _boulderCount; i++)
        {
            Vector3 v3RandomPoint = Vector3.zero;
            bool bValidPosition = false;

            while (!bValidPosition && iAttempts < iMaxAttempts)
            {
                v3RandomPoint = GetRandomPoint();
                bValidPosition = true;

                if (Vector3.Distance(v3RandomPoint, Vector3.zero) < iBoulderMinDist)
                {
                    bValidPosition = false;
                }

                foreach (var spawnedBoulder in spawnedBoulders)
                {
                    if (Vector3.Distance(v3RandomPoint, spawnedBoulder.transform.position) < iBoulderClusterDist)
                    {
                        bValidPosition = false;
                        break;
                    }
                }

                iAttempts++;
            }

            if (bValidPosition)
            {
                var newBoulder = Instantiate(boulderPF, v3RandomPoint, Quaternion.identity);
                newBoulder.GetComponent<Boulder>().Damage = iBoulderDamage;
                newBoulder.GetComponent<Boulder>().spawnTimer.timeRequired = iBoulderSpawnTime;
                spawnedBoulders.Add(newBoulder);
            }
            else
            {
                Debug.Log("Could not place all boulders.");
                break;
            }
        }
    }

    public void DestroyBoulders(List<GameObject> _bouldersToRemove)
    {
        foreach (var boulder in _bouldersToRemove)
        {
            spawnedBoulders.Remove(boulder);
            Destroy(boulder);
        }
    }

    public void ActivateBoulders(List<GameObject> _bouldersToRemove)
    {
        foreach (var boulder in _bouldersToRemove)
        {
            spawnedBoulders.Remove(boulder);
            boulder.GetComponent<Boulder>().detonationTriggered = true;
        }
    }

    Vector3 GetRandomPoint()
    {
        Vector2 randomPoint2D = Random.insideUnitCircle * iArenaRadius;
        return new Vector3(randomPoint2D.x, 0, randomPoint2D.y);
    }

    public void TakeDamage(float _dmg)
    {
        fHealth -= _dmg;

        if (fHealth <= 0.0f)
        {
            GameManager.instance.GameOver(true);
        }
    }

    public void LineAttack()
    {
        randomCast.elapsedtime += Time.deltaTime;

        if (randomCast.elapsedtime > randomCast.timeRequired)
        {
            var newLineAttack = Instantiate(lineAttackPF);

            newLineAttack.GetComponent<LineAttack>().castTimer.timeRequired = fLineCastTime;

            randomCast.timeRequired = Random.Range(fLineIntervalMin, fLineIntervalMax);
            randomCast.elapsedtime = 0;
        }
    }

    public IEnumerator Dash(float _fDashDistance)
    {
        lineIndicator.SetActive(false);
        float fElapsedTime = 0.0f;
        Vector3 origPos = transform.position;
        Vector3 targetPos = origPos + transform.forward * _fDashDistance;

        float fDashTime = _fDashDistance / fDashSpeed;

        while (fElapsedTime < fDashTime)
        {
            fDashCompletion = fElapsedTime / fDashTime;

            transform.position = Vector3.Lerp(origPos, targetPos, fElapsedTime / fDashTime);
            fElapsedTime += Time.deltaTime;

            yield return null;
        }
    }

    public IEnumerator LowerLava()
    {
        bResetAfterExplode = true;
        float fElapsedTime = 0.0f;
        float fOriginal = fLavaAmount;

        while (fElapsedTime < 7)
        {
            fLavaAmount = Mathf.Lerp(fOriginal, fOriginalLavaAmount, fElapsedTime / 7);
            rockyLava.SetFloat("_LavaAmount", fLavaAmount);
            fElapsedTime += Time.deltaTime;

            yield return null;
        }

        bResetAfterExplode = false;
    }

    public void SpinAttack()
    {
        agent.SetDestination(player.transform.position);
        print("Agent stopped: " + agent.isStopped);

        if (CheckForObjectInAoE(player.gameObject, fSpinRadius))
        {
            Player.instance.TakeDamage(iSpinDamage);
        }

        // Check if boulders are hit
        List<GameObject> bouldersToRemove = new List<GameObject>();
        foreach (var boulder in spawnedBoulders)
        {
            if (CheckForObjectInAoE(boulder, fSpinRadius))
            {
                bouldersToRemove.Add(boulder);
                boulder.GetComponent<Boulder>().DetonateRock();
            }
        }
        ActivateBoulders(bouldersToRemove);
    }

    public bool CheckForObjectInAoE(GameObject _objectToCheck, float _fRadius)
    {
        float distance = Vector3.Distance(_objectToCheck.transform.position, transform.position);
        if (distance < _fRadius)
        {
            return true;
        }

        return false;
    }

    public void DashAttack()
    {
        if (!bDashActive)
        {
            if (!bStopRotation)
            {
                TurnToPlayer();
            }
            
            fDistToDash = Vector3.Distance(transform.position, player.transform.position);
            fDistToDash = Mathf.Clamp(fDistToDash, fMinDashDist, 999);
        }

        anim.SetFloat("_dashPos", fDashCompletion);
    }

    public void TurnToPlayer()
    {
        Vector3 direction = Player.instance.transform.position - transform.position;
        direction.y = 0;

        Quaternion rotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * fTurnSpeed);
    }

    public bool HasLOS()
    {
        // Ray cast debug lines for AoE Attack
        Vector3 playerDir = player.GetComponentInChildren<Collider>().bounds.center - transform.position;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, playerDir, out hit, 100))
        {
            Debug.DrawLine(transform.position, hit.point, Color.red);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject == Player.instance.gameObject)
                {
                    return true;
                }
            }
        }
        else
        {
            Debug.DrawLine(transform.position, playerDir * 100, Color.black);
        }

        return false;
    }
}
