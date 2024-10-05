using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PB_ExplodeLOS : PB_FSM
{
    bool bCenterReached = false;
    bool bExplosionStarted = false;

    public PB_ExplodeLOS(PrototypeBoss _boss, NavMeshAgent _agent, GameObject _player, Animator _anim)
        : base(_boss, _agent, _player, _anim)
    {
        stateName = STATE.EXPLODE;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        base.Enter();

        boss.circleIndicator.SetActive(false);
        boss.coneIndicator.SetActive(false);
        boss.lineIndicator.SetActive(false);
        boss.roomWideIndicator.SetActive(false);
        boss.bStopRotation = true;
    }

    public override void Update()
    {
        base.Update();

        TickRoomWideAoE();

        if (bExplosionStarted)
        {
            boss.fLavaAmount += Time.deltaTime * 0.15f;
            boss.fLavaAmount = Mathf.Clamp(boss.fLavaAmount, 0, 1);
            boss.rockyLava.SetFloat("_LavaAmount", boss.fLavaAmount);

            if (boss.fLavaAmount >= 0.99)
            {
                bExplosionStarted = false;
            }
        }

        if (boss.bExplosionDone)
        {
            boss.bExplosionDone = false;
            nextState = new PB_BoulderSpawn(boss, agent, player, anim);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
        boss.bStopRotation = false;
        boss.AoETimer.elapsedtime = 0;
    }

    public void TickRoomWideAoE()
    {
        if (!bCenterReached)
        {
            agent.SetDestination(Vector3.zero);

            float distFromDestination = Vector3.Distance(boss.transform.position, new(0, boss.transform.position.y, 0));

            if (distFromDestination <= 0.5f)
            {
                boss.roomWideIndicator.SetActive(true);
                bExplosionStarted = true;
                anim.SetTrigger("_triggerExplode")  ;
                bCenterReached = true;
            }
        }
    }
}
