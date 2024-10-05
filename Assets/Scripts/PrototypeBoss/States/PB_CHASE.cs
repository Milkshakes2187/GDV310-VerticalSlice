using UnityEngine;
using UnityEngine.AI;

public class PB_Chase : PB_FSM
{
    Timer checkForDash;

    public PB_Chase(PrototypeBoss _boss,NavMeshAgent _agent, GameObject _player, Animator _anim)
        : base(_boss, _agent, _player, _anim)
    {
        stateName = STATE.CHASE;
    }

    public override void Enter()
    {
        base.Enter();

        agent.isStopped = false;
        checkForDash.timeRequired = 1f;
    }

    public override void Update()
    {
        base.Update();

        ChooseCombo();
        
        agent.destination = player.transform.position;
    }

    public override void Exit()
    {
        base.Exit();
    }

    void ChooseCombo()
    {
        if (GetDistFromPlayer() > 25)
        {
            nextState = new PB_Combo(boss, agent, player, anim, 0);
            stage = EVENT.EXIT;
        }

        checkForDash.elapsedtime += Time.deltaTime;
        if (GetDistFromPlayer() > 13 && checkForDash.elapsedtime > checkForDash.timeRequired)
        {
            var randPercent = Random.Range(0f, 100f);

            if (randPercent < boss.fDashChance)
            {
                nextState = new PB_Combo(boss, agent, player, anim, 0);
                stage = EVENT.EXIT;
            }

            checkForDash.elapsedtime = 0;
        }

        if (GetDistFromPlayer() < 8)
        {
            var randomCombo = Random.Range(1, boss.bossCombos.Count);

            nextState = new PB_Combo(boss, agent, player, anim, randomCombo);
            stage = EVENT.EXIT;
        }
    }
}
