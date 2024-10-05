
using UnityEngine;
using UnityEngine.AI;

public class PB_Idle : PB_FSM
{
    public PB_Idle(PrototypeBoss _boss, NavMeshAgent _agent, GameObject _player, Animator _anim)
        : base(_boss, _agent, _player, _anim)
    {
        stateName = STATE.IDLE;
        agent.isStopped = false;
        boss.bStopRotation = true;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (GetDistFromPlayer() < 25f && !boss.bFightStarted)
        {
            boss.bFightStarted = true;

            nextState = new PB_BoulderSpawn(boss, agent, player, anim);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
        boss.bStopRotation = false;
    }
}
