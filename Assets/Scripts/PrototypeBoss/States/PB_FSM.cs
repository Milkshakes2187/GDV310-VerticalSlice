using UnityEngine;

using UnityEngine.AI;
using System.Collections.Generic;

public class PB_FSM
{
    public enum STATE
    {
        IDLE, CHASE, BOULDERSPAWN, EXPLODE, LINEATTACK, DASH, CONEATTACK
    };

    public enum EVENT
    {
        ENTER, UPDATE, EXIT,
    }

    public enum ATTACKS
    {
        CONE, SPIN, DASH
    }

    public STATE stateName;
    protected EVENT stage;
    protected Animator anim;
    protected PB_FSM nextState;
    protected PB_FSM lastAttackState;
    protected NavMeshAgent agent;
    protected GameObject player;
    protected PrototypeBoss boss;
    protected bool bAttackInProgress = false;

    // combo stuff
    protected float elapsedComboTime = 0;
    protected bool comboDone = false;
    protected int currentComboAttack = 0;

    public PB_FSM(PrototypeBoss _boss, NavMeshAgent _agent, GameObject _player, Animator _anim)
    {
        // bind values
        boss = _boss;
        agent = _agent;
        player = _player;
        anim = _anim;

        stage = EVENT.ENTER;
    }

    #region Core Loop
    public virtual void Enter() 
    {
        stage = EVENT.UPDATE; 
    }
    public virtual void Update() 
    { 
        stage = EVENT.UPDATE;

        if (IsExplosionTime() && !boss.bIsAttacking && stateName != STATE.EXPLODE)
        {
            nextState = new PB_ExplodeLOS(boss, agent, player, anim);
            stage = EVENT.EXIT;
        }
    }
    public virtual void Exit() 
    {
        comboDone = false;
        stage = EVENT.EXIT; 
    }

    public PB_FSM Process()
    {
        if (stage == EVENT.ENTER) { Enter(); }
        if (stage == EVENT.UPDATE) { Update(); }
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }
        return this;
    }

    #endregion

    protected void RunCombo(BossCombo _bossCombo)
    {
        agent.SetDestination(player.transform.position);

        if (!boss.bIsAttacking)
        {
            if (!boss.bStopRotation)
            {
                boss.TurnToPlayer();
            }
            
            elapsedComboTime += Time.deltaTime;
        }

        if (currentComboAttack < _bossCombo.combo.Count)
        {
            if (elapsedComboTime > _bossCombo.combo[currentComboAttack].delay)
            {
                if (!boss.bAnimTriggered)
                {
                    agent.isStopped = true;

                    switch (_bossCombo.combo[currentComboAttack].attacks)
                    {
                        case ATTACKS.CONE:
                            anim.SetTrigger("_triggerCone");
                            break;
                        case ATTACKS.SPIN:
                            anim.SetTrigger("_triggerSpin");
                            break;
                        case ATTACKS.DASH:
                            anim.SetTrigger("_triggerDash");
                            break;
                    }

                    boss.bIsAttacking = true;
                    boss.bAnimTriggered = true;
                }

                if (!boss.bIsAttacking)
                {
                    elapsedComboTime = 0;
                    currentComboAttack++;
                    boss.bAnimTriggered = false;
                    agent.isStopped = false;
                }
            }
        }
        else
        {
            comboDone = true;
        }
    }

    protected float GetDistFromPlayer()
    {
        return Vector3.Distance(player.transform.position, agent.transform.position);
    }

    public float CalculatePercentage(float elapsedTime, float requiredTime)
    {
        // Ensure requiredTime is not zero to avoid division by zero
        if (requiredTime == 0)
        {
            Debug.LogWarning("Required time should not be zero.");
            return 0;
        }

        float percentage = (elapsedTime / requiredTime);
        return Mathf.Clamp(percentage, 0f, 1f); // Ensure the percentage is between 0 and 1
    }

    public bool IsExplosionTime()
    {
        if (boss.AoETimer.elapsedtime > boss.AoETimer.timeRequired)
        {
            return true;
        }

        return false;
    }

    public bool CheckForObjectInCone(GameObject _objectToCheck, float _fVisionDist, float _fHalfFOV)
    {
        Vector3 v3Dir = _objectToCheck.transform.position - boss.transform.position;
        float fAngle = Vector3.Angle(v3Dir, boss.transform.forward);

        Debug.DrawRay(boss.transform.position, boss.transform.forward * _fVisionDist, Color.cyan);

        Quaternion leftRayRotation = Quaternion.AngleAxis(-_fHalfFOV, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(_fHalfFOV, Vector3.up);

        // Cast rays to represent the FOV
        Vector3 leftRayDirection = leftRayRotation * boss.transform.forward;
        Vector3 rightRayDirection = rightRayRotation * boss.transform.forward;

        Debug.DrawRay(boss.transform.position, leftRayDirection * _fVisionDist, Color.red);
        Debug.DrawRay(boss.transform.position, rightRayDirection * _fVisionDist, Color.red);

        // if object in cone, return true
        if (v3Dir.magnitude < _fVisionDist && fAngle < _fHalfFOV)
        {
            return true;
        }

        return false;
    }

    public bool CheckForObjectInAoE(GameObject _objectToCheck, float _fRadius)
    {
        float distance = Vector3.Distance(_objectToCheck.transform.position, boss.transform.position);
        if (distance < _fRadius)
        {
            return true;
        }

        return false;
    }
}