using UnityEngine;
using UnityEngine.AI;

public class PB_BoulderSpawn : PB_FSM
{
    public PB_BoulderSpawn(PrototypeBoss _boss, NavMeshAgent _agent, GameObject _player, Animator _anim)
        : base(_boss, _agent, _player, _anim)
    {
        stateName = STATE.BOULDERSPAWN;
        agent.isStopped = true;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        agent.GetComponent<PrototypeBoss>().SpawnBoulders(boss.iBoulderCount);


        nextState = new PB_Chase(boss, agent, player, anim);
        stage = EVENT.EXIT;
    }

    public override void Exit()
    {
        base.Exit();
    }
}
